using System.Text;
using Core;
using Core.AST;
using Core.Dynamic;
using Core.Exceptions;
using Core.Native;
using Core.Tables;
using Math = System.Math;

namespace Interpreter;

public partial class Interpreter
{
    public Interpreter(List<double> numberList, NativeFunctionRegistry nativeFunctions, SourceFileTable? sourceFileTable, DebugTable? debugTable, StringPoolTable stringPoolTable)
    {
        _numberList = numberList;
        _nativeFunctions = nativeFunctions;
        _sourceFileTable = sourceFileTable;
        _sourceFileTable?.RebuildCache();
        _debugTable = debugTable;
        _stringPoolTable = stringPoolTable;
    }

    private readonly SourceFileTable? _sourceFileTable;
    private readonly DebugTable? _debugTable;
    private readonly StringPoolTable _stringPoolTable;

    private readonly NativeFunctionRegistry _nativeFunctions;

    private readonly List<double> _numberList;

    private readonly Dictionary<string, DynamicNamespace> _dynamicNamespaces = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private bool HasContext => _contextStack.Count > 0;
    private ASTContext? CurrentContext => HasContext ? _contextStack.Peek() : null;

    private int _currentDebugIndex = -1;

    private const string GlobalNamespaceName = "~global";

    public void Execute(ProgramNode program, List<string?>? args = null)
    {
        // First is load global namespace with core classes
        var globalNamespace = program.Statements.OfType<NamespaceNode>().FirstOrDefault(x => _stringPoolTable[x.NameId] == GlobalNamespaceName);

        if (globalNamespace is not null)
        {
            var dynamicNamespace = new DynamicNamespace(GlobalNamespaceName);
            _dynamicNamespaces[GlobalNamespaceName] = dynamicNamespace;
            
            _dynamicNamespaces[GlobalNamespaceName] = 
                ToDynamicNamespace(globalNamespace, dynamicNamespace);
            program.Statements.Remove(globalNamespace);
        }

        // Load namespaces
        foreach (var namespaceNode in program.Statements.OfType<NamespaceNode>().Where(x => _stringPoolTable[x.NameId] != GlobalNamespaceName))
            _dynamicNamespaces[_stringPoolTable[namespaceNode.NameId]] = ToDynamicNamespace(namespaceNode);
        

        // Search main function
        var function = _dynamicNamespaces[GlobalNamespaceName].Functions.FirstOrDefault(f => _stringPoolTable[f.NameId] == "main");
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
    /// <exception cref="QlangRuntimeException">will call exception if arguments is not valid or during execution function exception will occur</exception>
    private object? ExecuteFunction(DynamicFunction? function, List<object?> arguments, DynamicClass? ownerClass, DynamicNamespace? ownerNamespace)
    {
        if (function is null)
            return null;
        
        var contextClass = function.Context?.Class ?? ownerClass ?? (HasContext ? CurrentContext?.Class : null);
        var contextNamespace = function.Context?.Namespace ?? ownerNamespace ?? (HasContext ? CurrentContext?.Namespace : null);
        ASTContext newContext = new() { Function = function, ParentFunction = function.Context?.Function, Class = contextClass, Namespace = contextNamespace};

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
                        false);
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
            return _returnValue;
        }
        finally
        {
            RestoreContextStack();
        }
    }

    private void ExecuteStatement(ASTNode? statement)
    {
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
            
            case LineNode lineNode:
                EvaluateLine(lineNode);
                break;
            
            default:
                throw new QlangRuntimeException($"Unknown statement type: {statement?.GetType().Name ?? "<null>"}", GetCurrentDebug(), GetStackTrace());
        }
    }

    private void AssignmentNode(AssignmentNode assignNode)
    {
        if (_contextStack.Count == 0)
            return;

        var value = EvaluateExpression(assignNode.Value);

        var path = assignNode.Path;

        if (path.Count == 0)
            throw new QlangRuntimeException("Assignment path cannot be empty", GetCurrentDebug(), GetStackTrace());

        var lastNode = (ObjectPointerNode)path[^1];

        object? currentObject = null;

        if (path.Count > 1)
        {
            var callNode = new CallNode
            {
                Objects = path.SkipLast(1).ToList(),
            };

            currentObject = ExecuteObjectCalls(callNode);
        }
        
        var assignName = _stringPoolTable[assignNode.GetLastNameId()];

        // Change value
        if (!assignNode.IsNew)
        {
            var obj = FindObject(lastNode, currentObject, currentObject is null);

            if (obj.@object is Variable var)
            {
                if (var.IsConst && !assignNode.IsNew)
                    throw new QlangRuntimeException($"Cannot re-assign const property '{_stringPoolTable[lastNode.NameId]}'", GetCurrentDebug(),
                        GetStackTrace());

                if (currentObject is not null && var.IsPrivate && !_allowPrivateCall)
                    throw new QlangRuntimeException("Cannot access to private variable from external source",
                        GetCurrentDebug(), GetStackTrace());

                var.Value = value;
                return;
            }

            throw new QlangRuntimeException($"Invalid assignment target: {obj.@object?.GetType().Name}", GetCurrentDebug(),
                GetStackTrace());
        }

        if (HasContext)
        {
            if (CurrentContext.Function is not null)
            {
                CurrentContext.Function.Variables[assignName] =
                    Variable.FromAssignmentNode(assignNode, value, _stringPoolTable);
                return;
            }

            if (CurrentContext.Class is not null)
            {
                CurrentContext.Class.Variables[assignName] =
                    Variable.FromAssignmentNode(assignNode, value, _stringPoolTable);
                return;
            }

            if (CurrentContext.Namespace is not null)
            {
                CurrentContext.Namespace.Variables[assignName] =
                    Variable.FromAssignmentNode(assignNode, value, _stringPoolTable);
                return;
            }
        }

        _dynamicNamespaces[GlobalNamespaceName].Variables[assignName] = Variable.FromAssignmentNode(assignNode, value, _stringPoolTable);
    }

    private DynamicClass GetNewClass(DynamicClass dynamicClass, List<object?> args)
    {
        var dClass = dynamicClass.Clone();

        var fromClass = GetFunctionFromClass(dClass, _stringPoolTable.Add(Keywords.CreateClassInstanceKeyword), args);

        var indexOfNewClass = _stringPoolTable.Add(Keywords.CreateClassInstanceKeyword);
        if (dClass.Body.OfType<FunctionNode>().Any(f => f.NameId == indexOfNewClass) &&
            fromClass.function == null)
            throw new QlangRuntimeException($"Can't initialize class '{dynamicClass.ClassName}' with this parameters.", GetStackTrace());

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
                    '\"' => "\"",
                    _ => input.Slice(i, 2).ToString()
                });

                if (next is 'n' or 't' or '\\' or '\"')
                    i++;
            }
            else
                sb.Append(input[i]);
        }

        var processed = sb.ToString();
        return csharpString ? $"{processed}" : processed;
    }

    private FunctionNode PrepareFunctionNodePointer(FunctionNode node)
    {
        node.Context = CurrentContext;
        return node;
    }

    private object? EvaluateExpression(ASTNode? expr)
    {
        if (expr is null)
            return null;

        if (HasContext)
            CurrentContext.CurrentNode = expr;

        try
        {
            return expr switch
            {
                LineNode ln => EvaluateLine(ln),
                CastNode cast => CastObject(cast),
                StringRefNode strRef => GetStringRef(strRef),
                NumberRefNode numberRef => GetNumberRef(numberRef),
                ClassNode classNode => ToDynamicClass(classNode),
                BooleanNode booleanNode => booleanNode.Value,
                NumberNode num => num.Value,
                BinaryOperationNode binOp => EvaluateBinaryOperation(binOp),
                CollectionNode collection => collection.Collection.ConvertAll(EvaluateExpression),
                KeywordNode node when _stringPoolTable[node.KeywordId] == Keywords.NullKeyword => null,
                CallNode call => ExecuteObjectCalls(call),
                FunctionNode fn => PrepareFunctionNodePointer(fn),
                _ => throw new QlangRuntimeException(
                    $"Unknown expression type: {expr.GetType().Name}",
                    GetCurrentDebug(),
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
                GetCurrentDebug(),
                GetStackTrace());
        }
    }

    private object? EvaluateLine(LineNode ln)
    {
        _currentDebugIndex = ln.DebugIndex;
        
        if (HasContext)
            CurrentContext.CurrentDebugIndex = _currentDebugIndex;
        
        if (ln.Content is AssignmentNode assignment)
        {
            ExecuteStatement(assignment);
            return null;
        }
        
        return EvaluateExpression(ln.Content);
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
        
        throw new QlangRuntimeException($"Cannot cast object to type '{typeStr}'", GetCurrentDebug(),
            GetStackTrace());
    }
    
    private double DivideWithCheck(object left, object right, BinaryOperationNode node)
    {
        var divisor = right.ToString().ParseNumber();

        if (Math.Abs(divisor) < double.Epsilon)
        {
            throw new QlangRuntimeException(
                "Division by zero",
                GetCurrentDebug(),
                GetStackTrace());
        }
        return left.ToString().ParseNumber() / divisor;
    }

    private string GetStringRef(StringRefNode strStringRef)
    {
        return _stringPoolTable[strStringRef.Index];
    }

    private double GetNumberRef(NumberRefNode numberRef)
    {
        if (numberRef.Index >= _numberList.Count || numberRef.Index < 0)
        {
            throw new QlangRuntimeException(
                $"Undefined number reference: {numberRef.Index}",
                GetCurrentDebug(),
                GetStackTrace());
        }

        var number = _numberList[numberRef.Index];
        
        return numberRef.IsNegative ? -number : number;
    }

    private DynamicClass CreateClassFrom(object? obj, DynamicClass copy, ASTNode context)
    {
        if (obj is DynamicClass dynamic && dynamic.ClassName == copy.ClassName)
            return dynamic;
        
        var createFunction = copy.Body
            .OfType<FunctionNode>()
            .FirstOrDefault(f => _stringPoolTable[f.NameId] == "_createFrom");

        if (createFunction is null)
            throw new QlangRuntimeException(
                "Second value is incompatible",
                GetCurrentDebug(),
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
                GetCurrentDebug(),
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

        leftClass ??= CreateClassFrom(left, rightClass!, binOp);
        rightClass ??= CreateClassFrom(right, leftClass, binOp);
        
        // Console.WriteLine("\nOperator: " + binOp.Operator);
        // Console.WriteLine($"First operand: '{leftClass.Variables["_value"].Value}'");
        // Console.WriteLine($"Second operand: '{rightClass.Variables["_value"].Value}'");

        var originalOperator = _stringPoolTable[binOp.OperatorId];
        if (originalOperator.Any(c => c is '=' or '>' or '<' or '!' or '*' or '/' or '%' or '+' or '-'))
        {
            var @operator = "";
            for (var i = 0; i < originalOperator.Length; i++)
            {
                @operator += originalOperator[i] switch
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
                    _ => throw new QlangRuntimeException("Undefined operator: " + originalOperator[i], GetStackTrace())
                    
                };

                if (i == 0 && originalOperator.Length == 2 && originalOperator[i] == '=' && originalOperator[i + 1] == '=')
                    break;

                if (i == 0 && originalOperator.Length == 2 && originalOperator[i] == '!' && originalOperator[i + 1] == '=')
                {
                    @operator += "Equal";
                    break;
                }
                
                if (i !=  originalOperator.Length - 1)
                    @operator += "Or";
            }

            binOp.OperatorId = _stringPoolTable.Add(@operator);
            originalOperator = @operator;
        }
        
        var opFunction = leftClass.Body
            .OfType<FunctionNode>()
            .FirstOrDefault(f =>
                _stringPoolTable[f.NameId] == $"_operator{originalOperator}" && f.Parameters.Count == 2);

        if (opFunction is null)
            throw new QlangRuntimeException(
                $"Class '{leftClass.ClassName}' is incompatible for operator '{originalOperator}'",
                GetCurrentDebug(),
                GetStackTrace());

        var result = ExecuteFunction(
            ToDynamicFunction(opFunction),
            [leftClass, rightClass],
            leftClass, null);

        if ((result is not DynamicClass dynamicClass || dynamicClass.ClassName != leftClass.ClassName) &&
            (originalOperator.Contains("Division") || originalOperator.Contains("Subtraction") || originalOperator.Contains("Multiplication") || originalOperator.Contains("Addition")))
            throw new QlangRuntimeException(
                $"Return value of '_operator{originalOperator.ToLower()}' must be equal to type '{leftClass.ClassName}'", GetCurrentDebug(),
                GetStackTrace()); 
        
        if (result is not bool &&
            (originalOperator.Contains("Equal") ||  originalOperator.Contains("Greater") || 
             originalOperator.Contains("Less")))
            throw new QlangRuntimeException(
                $"Return value of '_operator{originalOperator.ToLower()}' must be equal to type 'bool'", GetCurrentDebug(),
                GetStackTrace()); 
        
        return result;
    }

    private object? EvaluateBinaryOperation(BinaryOperationNode binOp)
    {
        var left = EvaluateExpression(binOp.Left);
        var right = EvaluateExpression(binOp.Right);

        var @operator = _stringPoolTable[binOp.OperatorId];
        if (left is null || right is null)
        {
            return @operator switch
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
        switch (@operator)
        {

            case "&&":
                {
                    if (!bool.TryParse(left.ToString(), out leftBool))
                        throw new QlangRuntimeException(
                            $"Type error: Left operand of '&&' must be boolean, got '{left}'",
                            GetCurrentDebug(), GetStackTrace());

                    if (!leftBool)
                        return false;

                    if (!bool.TryParse(right.ToString(), out rightBool))
                        throw new QlangRuntimeException(
                            $"Type error: Right operand of '&&' must be boolean, got '{right}'",
                            GetCurrentDebug(), GetStackTrace());

                    return rightBool;
                }
            case "||":
                {
                    if (!bool.TryParse(left.ToString(), out leftBool))
                        throw new QlangRuntimeException(
                            $"Type error: Left operand of '||' must be boolean, got '{left}'",
                            GetCurrentDebug(), GetStackTrace());

                    if (leftBool)
                        return true;

                    if (!bool.TryParse(right.ToString(), out rightBool))
                        throw new QlangRuntimeException(
                            $"Type error: Right operand of '||' must be boolean, got '{right}'",
                            GetCurrentDebug(), GetStackTrace());

                    return rightBool;
                }
        }

        // If it's bool condition
        if (bool.TryParse(left.ToString(), out leftBool) && bool.TryParse(right.ToString(), out rightBool))
        {
            return @operator switch
            {
                "==" => Equals(leftBool, rightBool),
                "!=" => !Equals(leftBool, rightBool),
                _ => throw new QlangRuntimeException(
                    $"Unknown operator for boolean: {@operator}",
                    GetCurrentDebug(),
                    GetStackTrace())
            };
        }

        if (@operator == "+" && (left is string || right is string))
        {
            if (left is DynamicClass leftClassStr)
            {
                var leftToString = leftClassStr.Body.OfType<FunctionNode>().FirstOrDefault(f => _stringPoolTable[f.NameId] == "toString");

                if (leftToString is not null)
                    left = ExecuteFunction(ToDynamicFunction(leftToString), [left], leftClassStr, null);
            }
            
            if (right is DynamicClass rightClassStr)
            {
                var rightToString = rightClassStr.Body.OfType<FunctionNode>().FirstOrDefault(f => _stringPoolTable[f.NameId] == "toString");

                if (rightToString is not null)
                    right = ExecuteFunction(ToDynamicFunction(rightToString), [right], rightClassStr, null);
            }
            
            return left?.ToString() + right;
        }

        if (!left.ToString().IsNumber() || !right.ToString().IsNumber())
        {
            if (@operator is "==" or "!=")
                return @operator switch
                {
                    "==" => Equals(left, right),
                    "!=" => !Equals(left, right),
                    _ => throw new ArgumentOutOfRangeException()
                };

            throw new QlangRuntimeException(
                $"Type error: Cannot apply operator '{@operator}' to " +
                $"'{left.ToString() ?? "null"}' ({left.GetType().Name}) and '{right.ToString() ?? "null"}' ({right.GetType().Name})",
                GetCurrentDebug(),
                GetStackTrace());
        }

        // Double operations
        try
        {
            var leftNum = Convert.ToDouble(left);
            var rightNum = Convert.ToDouble(right);
            return @operator switch
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
                    $"Unknown operator: {@operator}",
                    GetCurrentDebug(),
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
                GetCurrentDebug(),
                GetStackTrace());
        }
    }
    
    private (FunctionNode? function, List<object?> args) GetFunctionFromGlobal
        (int nameId, List<object?>? args = null)
    {
        args ??= [];

        foreach (var function in _dynamicNamespaces[GlobalNamespaceName].Functions.Where(f => f.NameId == nameId))
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null)!;
    }
        
    private (FunctionNode? function, List<object?> Args) GetFunctionFromFunctionVariables
        (DynamicFunction? func, int nameId, List<object?>? args = null)
    {
        if (func is null)
            return (null, null)!;
        
        args ??= [];
        
        var name = _stringPoolTable[nameId];
        var functions = func.Variables.Where(f => f.Key == name).Select(var => var.Value.Value).OfType<FunctionNode>();
        
        foreach (var function in functions)
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null)!;
    }
    
    private (FunctionNode? function, List<object?> Args) GetFunctionFromClass
        (DynamicClass? @class, int nameId, List<object?>? args = null)
    {
        if (@class is null)
            return (null, null)!;
        
        args ??= [];

        var name = _stringPoolTable[nameId];
        var functions = @class.Body.OfType<FunctionNode>().Where(f => f.NameId == nameId).ToList();
        var values = @class.Variables.Where(f => f.Key == name).Select(var => var.Value.Value);
        
        functions.AddRange(values.OfType<FunctionNode>());
        
        foreach (var function in functions)
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null)!;
    }
    
    private (FunctionNode? function, List<object?> Args) GetFunctionFromNamespace
        (DynamicNamespace? @namespace, int nameId, List<object?>? args = null)
    {
        if (@namespace is null)
            return (null, null)!;
        
        args ??= [];

        var name = _stringPoolTable[nameId];
        var functions = @namespace.Functions.Where(f => f.NameId == nameId).ToList();
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

        var totalParamsCount = function.Parameters.Count;
    
        var requiredParamsCount = function.Parameters.Count(param => param.Value == null);

        if (args.Count < requiredParamsCount || args.Count > totalParamsCount)
            return false;
    
        for (var i = 0; i < totalParamsCount; i++)
            if (i < args.Count)
                finalArgs.Add(args[i]);
            else
            {
                var param = function.Parameters[i];
                if (param.Value != null)
                {
                    var defaultValue = EvaluateExpression(param.Value);
                    finalArgs.Add(defaultValue);
                }
                else
                    return false;
            }
    
        return true;
    }
    
    private (int, string) GetCurrentDebug() => GetDebug(_currentDebugIndex);
    
    private (int, string) GetDebug(int index)
    {
        if (_debugTable is null || _sourceFileTable is null)
            return (-1, "Debug file reference is not found.");

        if (index == -1)
            return (-1, "Debug index is -1");
        
        return (_debugTable.GetLineIndex(index) + 1, _sourceFileTable[_debugTable.GetFileId(index)]);
    }
}
