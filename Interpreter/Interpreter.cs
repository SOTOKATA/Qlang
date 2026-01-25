using System.Text;
using Core;
using Core.AST;
using Core.Debug;
using Core.Dynamic;
using Core.Exceptions;
using Core.Native;
using Math = System.Math;

namespace Interpreter;

public partial class Interpreter
{
    public Interpreter(List<string> stringList, List<double> numberList, NativeFunctionRegistry nativeFunctions, SourceFileTable sourceFileTable)
    {
        _stringList = stringList;
        _numberList = numberList;
        _nativeFunctions = nativeFunctions;
        _sourceFileTable = sourceFileTable;
        _sourceFileTable.RebuildCache();
    }

    private readonly SourceFileTable _sourceFileTable;

    private readonly NativeFunctionRegistry _nativeFunctions;

    private readonly List<string> _stringList;

    private readonly List<double> _numberList;

    private readonly Dictionary<string, DynamicNamespace> _dynamicNamespaces = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private bool HasContext => _contextStack.Count > 0;
    private ASTContext CurrentContext => HasContext ? _contextStack.Peek() : null;
    
    public const string GlobalNamespaceName = "0global";

    public void Execute(ProgramNode program, List<string?>? args = null)
    {
        // Debug initialize
        Logger.Initialize(false);
        Logger.SetLoggerPath(Path.Combine("debug_log_interpreter.txt"));

        // Load namespaces
        foreach (var statement in program.Statements)
        {
            if (statement is not NamespaceNode namespaceNode)
                    throw new QlangRuntimeException("Undefined structure." + statement, statement.Line, _sourceFileTable[statement.SourceFileId], GetStackTrace());
            
            _dynamicNamespaces[namespaceNode.Name] = ToDynamicNamespace(namespaceNode);
        }

        // Search main function
        var function = _dynamicNamespaces[GlobalNamespaceName].Functions.FirstOrDefault(f => f.Name == "main");
        if (function is null)
        {
            throw new QlangRuntimeException(
                "No 'main' function found in program",
                0, "",
                []);
        }
        
        // Run main function (and send arguments if exists)
        if (function.Parameters.Count == 0)
            ExecuteFunction(ToDynamicFunction(function), [], null, _dynamicNamespaces[GlobalNamespaceName]);
        else 
            ExecuteFunction(ToDynamicFunction(function), [args?.Cast<object?>().ToList()], null, _dynamicNamespaces[GlobalNamespaceName]);
    }

    private void AddContext(ASTContext context)
    {
        _contextStack.Push(context);
    }

    /// <summary>
    /// Runs function
    /// </summary>
    /// <param name="function">dynamic function to execute</param>
    /// <param name="arguments">argument (similar to other arguments in other languages)</param>
    /// <param name="ownerClass">owner class if exists (required for context)</param>
    /// <param name="ownerNamespace">owner namespace if exists (required for context)</param>
    /// <returns></returns>
    /// <exception cref="QlangRuntimeException">will call exception if arguments is not valid or during execution function expcetion will occur</exception>
    private object? ExecuteFunction(DynamicFunction? function, List<object?> arguments, DynamicClass? ownerClass, DynamicNamespace? ownerNamespace)
    {
        if (function is null)
            return null;

        Logger.Log($"EXECF'{function.Name}'({string.Join(", ", arguments)})");
        Logger.Log($"Function:\nName={function.Name}\nArguments={string.Join(", ", arguments)}");

        var contextClass = ownerClass ?? (HasContext ? CurrentContext.Class : null);
        var contextNamespace = ownerNamespace ?? (HasContext ? CurrentContext.Namespace : null);
        ASTContext newContext = new() { Function = function, Class = contextClass, Namespace = contextNamespace};

        AddContext(newContext);

        try
        {
            if (arguments.Count == function.Parameters.Count)
                for (var i = 0; i < function.Parameters.Count; i++)
                {
                    var var = function.Variables[function.Parameters[i]];

                    if (var.Type is not null && Typeof(var.Type) != Typeof(arguments[i]))
                        throw new QlangRuntimeException($"The type of param is '{(Typeof(arguments[i]) ?? "<null>")}' but must be '{Typeof(var.Type)}'", GetStackTrace());
                    
                    function.Variables[function.Parameters[i]] = new Variable(
                        function.Parameters[i],
                        arguments[i],
                        function.IsStatic,
                        false,
                        var.IsConst);
                }
            else
                throw new QlangRuntimeException("The number of arguments must be equal to the number of params", GetStackTrace());

            _return = false;
            _isBreakKeyword = false;
            _isContinueKeyword = false;
            _returnValue = null;
            foreach (var statement in function.Body.TakeWhile(_ => !_return))
            {
                if (statement is ReturnNode returnNode)
                {
                    if (returnNode.ReturnValue is not null)
                        _returnValue = EvaluateExpression(returnNode.ReturnValue);

                    break;
                }

                ExecuteStatement(statement);
            }

            _return = false;
            _isBreakKeyword = false;
            _isContinueKeyword = false;
            Logger.Log($"({function.Name}) Return:\nValue=" + _returnValue);
            return _returnValue;
        }
        finally
        {
            RestoreContextStack();
        }
    }

    private void ExecuteStatement(ASTNode statement)
    {
        Logger.Log("Statement: " + statement.GetTree(), "ExecuteStatement");
        
        if (HasContext)
            CurrentContext.CurrentNode = statement;
        
        switch (statement)
        {
            case AssignmentNode assign:
                AssignmentNode(assign);
                break;

            case CallNode call:
                ExecuteObjectCalls(call);
                break;

            case IfNode ifNode:
                ExecuteIf(ifNode);
                break;
            
            case SwitchNode switchNode:
                ExecuteSwitch(switchNode);
                break;

            case WhileNode whileNode:
                ExecuteWhile(whileNode);
                break;

            case ForNode forNode:
                ExecuteFor(forNode);
                break;

            default:
                throw new QlangRuntimeException($"Unknown statement type: {statement.GetType()}", statement.Line, _sourceFileTable[statement.SourceFileId], GetStackTrace());
        }
    }

    private void AssignmentNode(AssignmentNode assignNode)
    {
        if (_contextStack.Count == 0)
            return;

        var value = EvaluateExpression(assignNode.Value);

        var path = assignNode.Path;

        if (path.Count == 0)
            throw new QlangRuntimeException("Assignment path cannot be empty", assignNode.Line, _sourceFileTable[assignNode.SourceFileId], GetStackTrace());

        var lastNode = (ObjectPointerNode)path[^1];

        object? currentObject = null;

        if (path.Count > 1)
        {
            var callNode = new CallNode(lastNode.Line, lastNode.SourceFileId)
            {
                Objects = path.SkipLast(1).ToList(),
                Arguments = path.SkipLast(1).ElementAt(^1) is FunctionPointerNode ptr ? ptr.Arguments : default
            };

            currentObject = ExecuteObjectCalls(callNode);
        }

        // Change value
        if (!assignNode.IsNew)
        {
            var obj = FindObject(lastNode, currentObject, currentObject is null);

            if (obj.@object is Variable var)
            {
                if (var.IsConst && !assignNode.IsNew)
                    throw new QlangRuntimeException($"Cannot re-assign const property '{lastNode.Name}'", assignNode.Line, _sourceFileTable[assignNode.SourceFileId],
                        GetStackTrace());

                if (currentObject is not null && var.IsPrivate)
                    throw new QlangRuntimeException("Cannot access to private variable from external source",
                        lastNode.Line, _sourceFileTable[lastNode.SourceFileId], GetStackTrace());

                var.Value = value;
                return;
            }

            throw new QlangRuntimeException($"Invalid assignment target: {obj.@object?.GetType().Name}", assignNode.Line, _sourceFileTable[assignNode.SourceFileId],
                GetStackTrace());
        }

        if (HasContext)
        {
            if (CurrentContext.Function is not null)
            {
                CurrentContext.Function.Variables[assignNode.GetLastName()] =
                    Variable.FromAssignmentNode(assignNode, value);
                return;
            }

            if (CurrentContext.Class is not null)
            {
                CurrentContext.Class.Variables[assignNode.GetLastName()] =
                    Variable.FromAssignmentNode(assignNode, value);
                return;
            }

            if (CurrentContext.Namespace is not null)
            {
                CurrentContext.Namespace.Variables[assignNode.GetLastName()] =
                    Variable.FromAssignmentNode(assignNode, value);
                return;
            }
        }

        _dynamicNamespaces[GlobalNamespaceName].Variables[assignNode.GetLastName()] = Variable.FromAssignmentNode(assignNode, value);
    }

    private DynamicClass GetNewClass(DynamicClass dynamicClass, List<object?> args)
    {
        var dClass = dynamicClass.Clone();

        var fromClass = GetFunctionFromClass(dClass, Keywords.CreateClassInstanceKeyword, args);

        if (dClass.Body.OfType<FunctionNode>().Any(f => f.Name == Keywords.CreateClassInstanceKeyword) &&
            fromClass.function == null)
        {
            // Console.WriteLine("Functions: " + string.Join(", ", dClass.Body.OfType<FunctionNode>().Where(x => x.Name == "new").Select(f => $"{f.Name}({string.Join(", ", f.Parameters.Select(x => x.Path))})")));
            throw new QlangRuntimeException($"Can't initialize class '{dynamicClass.ClassName}' with this parameters.", GetStackTrace());
        }

        if (fromClass.function != null)
            ExecuteFunction(ToDynamicFunction(fromClass.function), fromClass.Args, dClass, null);

        return dClass;
    }

    private void RestoreContextStack()
    {
        if (!HasContext) 
            return;
        
        _contextStack.Pop();
    }

	// TODO: Bug fix with '"' and other specials
    private static string ParseString(ReadOnlySpan<char> input, bool csharpString = false)
    {
        var sb = new StringBuilder(input.Length);

        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '\\' && i + 1 < input.Length)
            {
                var next = input[i + 1];
                
                sb.Append(next switch
                {
                    'n' => '\n',
                    't' => '\t',
                    '\\' => '\\',
                    '"' => '"',
                    _ => input.Slice(i, 2).ToString()
                });

                if (next is 'n' or 't' or '\\' or '"')
                    i++;
            }
            else
                sb.Append(input[i]);
        }

        var processed = sb.ToString();
        return csharpString ? $"{processed}" : processed;
    }

    private object? EvaluateExpression(ASTNode? expr)
    {
        if (expr is null)
            return null;

        if (HasContext)
            CurrentContext.CurrentNode = expr;

        Logger.Log("Expression: " + expr.GetTree(), "EvaluateExpression");

        try
        {
            return expr switch
            {
                CastNode cast => CastObject(cast),
                StringRefNode strRef => GetStringRef(strRef),
                NumberRefNode numberRef => GetNumberRef(numberRef),
                ClassNode classNode => ToDynamicClass(classNode),
                BooleanNode booleanNode => booleanNode.Value,
                NumberNode num => num.Value,
                BinaryOperationNode binOp => EvaluateBinaryOperation(binOp),
                CollectionNode collection => collection.Collection.ConvertAll(EvaluateExpression),
                KeywordNode node when node.Value == Keywords.NullKeyword => null,
                CallNode call => ExecuteObjectCalls(call),
                FunctionNode => expr,
                _ => throw new QlangRuntimeException(
                    $"Unknown expression type: {expr.GetType().Name}",
                    expr.Line, _sourceFileTable[expr.SourceFileId],
                    GetStackTrace())
            };
        }
        catch (QlangRuntimeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new QlangRuntimeException(
                $"Internal error: {ex}",
                expr.Line, _sourceFileTable[expr.SourceFileId],
                GetStackTrace());
        }
    }

    // TODO: Finish work with casting (adding casting to DynamicClass)
    private object? CastObject(CastNode cast)
    {
        var type = ExecuteObjectCalls(cast.TypeCastPath);

        var @object = ExecuteObjectCalls(cast.ToCastObject);

        if (Typeof(type) == "Number" && @object is DynamicClass { ClassName: QlSystemClasses.StringClassName } @class &&
            @class.Variables["_value"].Value!.ToString().TryParseNumber(out var number))
            return number;

        // cast string to number
        if (Typeof(type) == "Number" && @object is string str 
                                     && str.TryParseNumber(out number)) 
            return number;
        
        if (Typeof(type) == "Number" && @object is int or long or double or float)
            return Convert.ToDouble(@object);
        
        // cast any object to string
        if (Typeof(type) == "String" && @object is not null)
            return @object.ToString();

        var typeStr = type switch
        {
            DynamicClass dw => dw.ClassName,
            null => "<null>",
            _ => type.ToString()!
        };
        
        throw new QlangRuntimeException($"Cannot cast object to type '{typeStr}'", cast.Line, _sourceFileTable[cast.SourceFileId],
            GetStackTrace());
    }
    
    private double DivideWithCheck(object left, object right, BinaryOperationNode node)
    {
        var divisor = right.ToString().ParseNumber();

        if (Math.Abs(divisor) < double.Epsilon)
        {
            throw new QlangRuntimeException(
                "Division by zero",
                node.Line, _sourceFileTable[node.SourceFileId],
                GetStackTrace());
        }
        return left.ToString().ParseNumber() / divisor;
    }

    private string GetStringRef(StringRefNode strStringRef)
    {
        if (strStringRef.Index >= _stringList.Count || strStringRef.Index < 0)
        {
            throw new QlangRuntimeException(
                $"Undefined string reference: {strStringRef.Index}",
                strStringRef.Line, _sourceFileTable[strStringRef.SourceFileId],
                GetStackTrace());
        }
        return _stringList[strStringRef.Index];
    }

    private double GetNumberRef(NumberRefNode numberRef)
    {
        if (numberRef.Index >= _numberList.Count || numberRef.Index < 0)
        {
            throw new QlangRuntimeException(
                $"Undefined number reference: {numberRef.Index}",
                numberRef.Line, _sourceFileTable[numberRef.SourceFileId],
                GetStackTrace());
        }

        var number = _numberList[numberRef.Index];
        
        return numberRef.IsNegative ? -number : number;
    }

    private DynamicClass? CreateClassFrom(object? obj, DynamicClass copy, ASTNode context)
    {
        if (obj is DynamicClass dynamic && dynamic.ClassName == copy.ClassName)
            return dynamic;
        
        var createFunction = copy.Body
            .OfType<FunctionNode>()
            .FirstOrDefault(f => f.Name == "_createFrom");

        if (createFunction is null)
            throw new QlangRuntimeException(
                "Second value is incompatible",
                context.Line, _sourceFileTable[context.SourceFileId],
                GetStackTrace());

        var created = ExecuteFunction(
            ToDynamicFunction(createFunction),
            [obj],
            copy,
            null);

        if (created is not DynamicClass dClass ||
            dClass.ClassName != copy.ClassName)
            throw new QlangRuntimeException(
                "Second value is null or incompatible",
                context.Line, _sourceFileTable[context.SourceFileId],
                GetStackTrace());

        return dClass;
    }

    private object? EvaluateClassBinaryOperation(object left, object right, BinaryOperationNode binOp)
    {
        DynamicClass? rightClass = null;
        DynamicClass? leftClass = null;

        if (left is DynamicClass lc)
            leftClass = lc;
        else
            rightClass = (DynamicClass)right;

        leftClass ??= CreateClassFrom(left, rightClass, binOp);
        rightClass ??= CreateClassFrom(right, leftClass, binOp);
        
        // Console.WriteLine("\nOperator: " + binOp.Operator);
        // Console.WriteLine($"First operand: '{leftClass.Variables["_value"].Value}'");
        // Console.WriteLine($"Second operand: '{rightClass.Variables["_value"].Value}'");

        if (binOp.Operator.Any(c => c is '=' or '>' or '<' or '!' or '*' or '/' or '%' or '+' or '-'))
        {
            var @operator = "";
            for (var i = 0; i < binOp.Operator.Length; i++)
            {
                @operator += binOp.Operator[i] switch
                {
                    '=' => "Equal",
                    '>' => "Greater",
                    '<' => "Less",
                    '!' => "Not",
                    '+' => "Addition",
                    '-' => "Subtraction",
                    '/' => "Division",
                    '*' => "Multiplication",
                    '%' => "Percent",
                };

                if (i == 0 && binOp.Operator.Length == 2 && binOp.Operator[i] == '=' && binOp.Operator[i + 1] == '=')
                    break;

                if (i == 0 && binOp.Operator.Length == 2 && binOp.Operator[i] == '!' && binOp.Operator[i + 1] == '=')
                {
                    @operator += "Equal";
                    break;
                }
                
                if (i !=  binOp.Operator.Length - 1)
                    @operator += "Or";
            }
            binOp.Operator = @operator;
        }
        
        var opFunction = leftClass.Body
            .OfType<FunctionNode>()
            .FirstOrDefault(f =>
                f.Name == $"_operator{binOp.Operator}" && f.Parameters.Count == 2);

        if (opFunction is null)
            throw new QlangRuntimeException(
                $"Class '{leftClass.ClassName}' is incompatible for operator '{binOp.Operator}'",
                binOp.Line, _sourceFileTable[binOp.SourceFileId],
                GetStackTrace());

        var result = ExecuteFunction(
            ToDynamicFunction(opFunction),
            [leftClass, rightClass],
            leftClass, null);

        if ((result is not DynamicClass dynamicClass || dynamicClass.ClassName != leftClass.ClassName) && 
            (binOp.Operator.Contains("Division") ||binOp.Operator.Contains("Subtraction") || binOp.Operator.Contains("Multiplication") || binOp.Operator.Contains("Addition")))
            throw new QlangRuntimeException(
                $"Return value of '_operator{binOp.Operator.ToLower()}' must be equal to type '{leftClass.ClassName}'", binOp.Line, _sourceFileTable[binOp.SourceFileId],
                GetStackTrace()); 
        
        if (result is not bool && 
            (binOp.Operator.Contains("Equal") ||  binOp.Operator.Contains("Greater") || 
             binOp.Operator.Contains("Less")))
            throw new QlangRuntimeException(
                $"Return value of '_operator{binOp.Operator.ToLower()}' must be equal to type 'bool'", binOp.Line, _sourceFileTable[binOp.SourceFileId],
                GetStackTrace()); 
        
        // Console.WriteLine("Result: " + (result as DynamicClass)?.Variables["_value"]);
        return result;
    }

    private object? EvaluateBinaryOperation(BinaryOperationNode binOp)
    {
        Logger.Log("Detected binary operation");
        Logger.Log($"Params: {binOp.Left} {binOp.Operator} {binOp.Right}");
        var left = EvaluateExpression(binOp.Left);
        var right = EvaluateExpression(binOp.Right);
        Logger.Log($"ExpressionParams: {left}: {left?.GetType().Name}; {right}: {right?.GetType().Name}");


        if (left is null || right is null)
        {
            return binOp.Operator switch
            {
                "==" => left == right,
                "!=" => left != right,
                _ => null
            };
        }

        if ((left is DynamicClass || right is DynamicClass))
            return EvaluateClassBinaryOperation(left, right, binOp);

        
        bool leftBool;
        bool rightBool;
        switch (binOp.Operator)
        {

            // Обработка логических операторов с ленивой оценкой
            case "&&":
                {
                    if (!bool.TryParse(left.ToString(), out leftBool))
                        throw new QlangRuntimeException(
                            $"Type error: Left operand of '&&' must be boolean, got '{left}'",
                            binOp.Line, _sourceFileTable[binOp.SourceFileId], GetStackTrace());

                    // Short-circuit: если левая часть false, правую не вычисляем
                    if (!leftBool)
                    {
                        Logger.Log($"Short-circuit &&: left is false, returning false");
                        return false;
                    }

                    if (!bool.TryParse(right.ToString(), out rightBool))
                        throw new QlangRuntimeException(
                            $"Type error: Right operand of '&&' must be boolean, got '{right}'",
                            binOp.Line, _sourceFileTable[binOp.SourceFileId], GetStackTrace());

                    Logger.Log($"Operation &&: {leftBool} && {rightBool} = {rightBool}");
                    return rightBool;
                }
            case "||":
                {
                    if (!bool.TryParse(left.ToString(), out leftBool))
                        throw new QlangRuntimeException(
                            $"Type error: Left operand of '||' must be boolean, got '{left}'",
                            binOp.Line, _sourceFileTable[binOp.SourceFileId], GetStackTrace());

                    // Short-circuit: если левая часть true, правую не вычисляем
                    if (leftBool)
                    {
                        Logger.Log("LeftRight: " + binOp.Left?.GetTree() + " and " + binOp.Right?.GetTree());
                        Logger.Log($"Short-circuit ||: left is true, returning true");
                        return true;
                    }

                    Logger.Log($"Short-circuit ||: left not is true, continue");

                    if (!bool.TryParse(right.ToString(), out rightBool))
                        throw new QlangRuntimeException(
                            $"Type error: Right operand of '||' must be boolean, got '{right}'",
                            binOp.Line, _sourceFileTable[binOp.SourceFileId], GetStackTrace());

                    Logger.Log($"Operation ||: {leftBool} || {rightBool} = {rightBool}");
                    return rightBool;
                }
        }

        // If it's bool condition
        if (bool.TryParse(left.ToString(), out leftBool) && bool.TryParse(right.ToString(), out rightBool))
        {
            Logger.Log($"IsBooleanOperation: {left}{binOp.Operator}{right}");
            return binOp.Operator switch
            {
                "==" => Equals(leftBool, rightBool),
                "!=" => !Equals(leftBool, rightBool),
                _ => throw new QlangRuntimeException(
                    $"Unknown operator for boolean: {binOp.Operator}",
                    binOp.Line, _sourceFileTable[binOp.SourceFileId],
                    GetStackTrace())
            };
        }

        if (binOp.Operator == "+" && (left is string || right is string))
        {
            if (left is DynamicClass leftClassStr)
            {
                var leftToString = leftClassStr.Body.OfType<FunctionNode>().FirstOrDefault(f => f.Name == "toString");

                if (leftToString is not null)
                    left = ExecuteFunction(ToDynamicFunction(leftToString), [left], leftClassStr, null);
            }
            
            if (right is DynamicClass rightClassStr)
            {
                var rightToString = rightClassStr.Body.OfType<FunctionNode>().FirstOrDefault(f => f.Name == "toString");

                if (rightToString is not null)
                    right = ExecuteFunction(ToDynamicFunction(rightToString), [right], rightClassStr, null);
            }
            
            return left.ToString() + right.ToString();
        }

        if (!left.ToString().IsNumber() || !right.ToString().IsNumber())
        {
            if (binOp.Operator is "==" or "!=")
                return binOp.Operator switch
                {
                    "==" => Equals(left, right),
                    "!=" => !Equals(left, right),
                    _ => throw new ArgumentOutOfRangeException()
                };

            throw new QlangRuntimeException(
                $"Type error: Cannot apply operator '{binOp.Operator}' to " +
                $"'{left?.ToString() ?? "null"}' ({left?.GetType().Name}) and '{right?.ToString() ?? "null"}' ({right?.GetType().Name})",
                binOp.Line, _sourceFileTable[binOp.SourceFileId],
                GetStackTrace());
        }

        try
        {
            var leftNum = Convert.ToDouble(left);
            var rightNum = Convert.ToDouble(right);
            Logger.Log($"Operation: {left}{binOp.Operator}{right}");
            return binOp.Operator switch
            {
                "==" => Equals(leftNum, rightNum),
                "!=" => !Equals(leftNum, rightNum),
                "<" => leftNum < rightNum,
                ">" => leftNum > rightNum,
                ">=" => leftNum >= rightNum,
                "<=" => leftNum <= rightNum,
                "+" => leftNum + rightNum,
                "-" => leftNum - rightNum,
                "*" => leftNum * rightNum,
                "/" => DivideWithCheck(leftNum, rightNum, binOp),
                "%" => leftNum % rightNum,
                _ => throw new QlangRuntimeException(
                    $"Unknown operator: {binOp.Operator}",
                    binOp.Line, _sourceFileTable[binOp.SourceFileId],
                    GetStackTrace())
            };
        }
        catch (QlangRuntimeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new QlangRuntimeException(
                $"Error evaluating operation: {ex.Message}",
                binOp.Line, _sourceFileTable[binOp.SourceFileId],
                GetStackTrace());
        }
    }
    
    private (FunctionNode? function, List<object?> args) GetFunctionFromGlobal
        (string name, List<object?>? args = null)
    {
        args ??= [];
        
        foreach (var function in _dynamicNamespaces[GlobalNamespaceName].Functions.Where(f => f.Name == name))
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null);
    }
        
    private (FunctionNode? function, List<object?> Args) GetFunctionFromFunctionVariables
        (DynamicFunction? func, string name, List<object?>? args = null)
    {
        if (func is null)
            return (null, null)!;
        
        args ??= [];
        
        var functions = func.Variables.Where(f => f.Key == name).Select(var => var.Value.Value).OfType<FunctionNode>();
        
        foreach (var function in functions)
            // Проверяем, можно ли вызвать эту функцию с данными аргументами
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null);
    }
    
    private (FunctionNode? function, List<object?> Args) GetFunctionFromClass
        (DynamicClass? @class, string name, List<object?>? args = null)
    {
        if (@class is null)
            return (null, null)!;
        
        args ??= [];
        
        var functions = @class.Body.OfType<FunctionNode>().Where(f => f.Name == name).ToList();
        var values = @class.Variables.Where(f => f.Key == name).Select(var => var.Value.Value);
        
        functions.AddRange(values.OfType<FunctionNode>());
        
        foreach (var function in functions)
            // Проверяем, можно ли вызвать эту функцию с данными аргументами
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null);
    }
    
    private (FunctionNode? function, List<object?> Args) GetFunctionFromNamespace
        (DynamicNamespace? @namespace, string name, List<object?>? args = null)
    {
        if (@namespace is null)
            return (null, null)!;
        
        args ??= [];

        var functions = @namespace.Functions.Where(f => f.Name == name).ToList();
        var values = @namespace.Variables.Where(f => f.Key == name).Select(var => var.Value.Value);
        
        functions.AddRange(values.OfType<FunctionNode>());
        
        foreach (var function in functions)
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null)!;
    }
    private bool TryMatchFunction(FunctionNode function, List<object?> args, out List<object?> finalArgs)
    {
        finalArgs = [];
    
        var requiredParamsCount = 0;
        var totalParamsCount = function.Parameters.Count;
    
        // Подсчет обязательных параметров (без значения по умолчанию)
        foreach (var param in function.Parameters)
        {
            if (param.Value == null)
                requiredParamsCount++;
        }
    
        // Проверка количества аргументов
        if (args.Count < requiredParamsCount || args.Count > totalParamsCount)
            return false;
    
        // Заполнение финального списка аргументов
        for (var i = 0; i < totalParamsCount; i++)
        {
            if (i < args.Count)
            {
                finalArgs.Add(args[i]);
            }
            else
            {
                var param = function.Parameters[i];
                if (param.Value != null)
                {
                    var defaultValue = EvaluateExpression(param.Value);
                    finalArgs.Add(defaultValue);
                }
                else
                {
                    return false; // Обязательный параметр не предоставлен
                }
            }
        }
    
        return true;
    }
    // private bool TryMatchFunction(FunctionNode function, List<object?> args, out List<object?> finalArgs)
    // {
    //     finalArgs = [];
    //     
    //     var requiredParamsCount = 0;
    //     var totalParamsCount = function.Parameters.Count;
    //     
    //     foreach (var t in function.Parameters)
    //     {
    //         if (t.Value == null)
    //             requiredParamsCount++;
    //         else
    //             break;
    //     }
    //     
    //     if (args.Count < requiredParamsCount || args.Count > totalParamsCount)
    //         return false;
    //     
    //     for (var i = 0; i < totalParamsCount; i++)
    //     {
    //         if (i < args.Count)
    //             finalArgs.Add(args[i]);
    //         else
    //         {
    //             var param = function.Parameters[i];
    //             if (param.Value != null)
    //             {
    //                 var defaultValue = EvaluateExpression(param.Value);
    //                 finalArgs.Add(defaultValue);
    //             }
    //             else
    //                 return false;
    //         }
    //     }
    //     
    //     return true;
    // }
}
