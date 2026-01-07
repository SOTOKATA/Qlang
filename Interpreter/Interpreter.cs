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
    public Interpreter(Dictionary<string, string> stringDictionary, Dictionary<string, object> numberDictionary, NativeFunctionRegistry nativeFunctions)
    {
        _stringDictionary = stringDictionary;
        _numberDictionary = numberDictionary;
        _nativeFunctions = nativeFunctions;
    }

    private readonly NativeFunctionRegistry _nativeFunctions;

    private readonly Dictionary<string, string> _stringDictionary;

    private readonly Dictionary<string, object> _numberDictionary;

    private readonly List<FunctionNode> _globalFunctions = [];
    private readonly List<Variable> _globalVariables = [];

    private readonly Dictionary<string, DynamicClass> _dynamicClasses = new();
    
    private readonly Dictionary<string, DynamicNamespace> _dynamicNamespaces = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private bool HasContext => _contextStack.Count > 0;
    private ASTContext CurrentContext => HasContext ? _contextStack.Peek() : null;

    public void Execute(ProgramNode program, List<string?>? args = null)
    {
        Logger.SetLoggerPath(Path.Combine("Logs", "Debug", "debug_interpreter.log"));
        Logger.Warn("----------- Interpreter -----------");

        foreach (var statement in program.Statements)
        {
            switch (statement)
            {
                case ClassNode classNode:
                    _dynamicClasses[classNode.Name] = ToDynamicClass(classNode);
                    break;
                case FunctionNode func:
                    _globalFunctions.Add(func);
                    break;
                case NamespaceNode namespaceNode:
                    if (_dynamicNamespaces.TryGetValue(namespaceNode.Name, out var baseNamespace))
                    {
                        var @namespace = ToDynamicNamespace(namespaceNode);

                        baseNamespace.Classes.AddRange(@namespace.Classes);
                        baseNamespace.Functions.AddRange(@namespace.Functions);

                        foreach (var varPair in @namespace.Variables)
                            baseNamespace.Variables[varPair.Key] = varPair.Value;
                        
                        _dynamicNamespaces[namespaceNode.Name] = baseNamespace;
                    }
                    else 
                        _dynamicNamespaces[namespaceNode.Name] = ToDynamicNamespace(namespaceNode);
                    break;
                case AssignmentNode assignmentNode:
                    _globalVariables.Add(new Variable(assignmentNode.VariableName, EvaluateExpression(assignmentNode.Value), assignmentNode.IsStatic, assignmentNode.IsPrivate, assignmentNode.IsConst));
                    break;
            }
        }

        var function = _globalFunctions.FirstOrDefault(f => f.Name == "main");
        if (function is null)
        {
            throw new QlangRuntimeException(
                "No 'main' function found in program",
                program.Statements.FirstOrDefault() ?? new ProgramNode(),
                []);
        }
        
        if (function.Parameters.Count == 0)
            ExecuteFunction(ToDynamicFunction(function), [], null, null);
        else 
            ExecuteFunction(ToDynamicFunction(function), [args?.Cast<object?>().ToList()], null, null);
    }

    private DynamicNamespace ToDynamicNamespace(NamespaceNode namespaceNode)
    {
        var dynamicNamespace = new DynamicNamespace(namespaceNode.Name);

        dynamicNamespace.Classes.AddRange(
            namespaceNode.Body
                .OfType<ClassNode>()
                .Select(ToDynamicClass)
        );

        dynamicNamespace.Functions.AddRange(namespaceNode.Body.OfType<FunctionNode>());
        
        foreach (var assignmentNode in namespaceNode.Body.OfType<AssignmentNode>())
                dynamicNamespace.Variables[assignmentNode.VariableName!] = new Variable(assignmentNode.VariableName!,
                    EvaluateExpression(assignmentNode.Value), assignmentNode.IsStatic, assignmentNode
                        .IsPrivate, assignmentNode.IsConst);
        
        return dynamicNamespace;
    }

    private DynamicClass ToDynamicClass(ClassNode classNode)
    {
        DynamicClass dynamicClass = new(classNode.Name);

        foreach (var assignmentNode in classNode.Body.OfType<AssignmentNode>())
                dynamicClass.Variables[assignmentNode.VariableName] = new Variable(assignmentNode.VariableName,
                    EvaluateExpression(assignmentNode.Value), assignmentNode.IsStatic, assignmentNode
                    .IsPrivate, assignmentNode.IsConst);
        
        // Remove all AssignmentNodes from body
        classNode.Body.RemoveAll(node => node is AssignmentNode);

        dynamicClass.Body = classNode.Body;

        return dynamicClass;
    }

    private DynamicFunction ToDynamicFunction(FunctionNode? functionNode)
    {
        DynamicFunction dynamicFunction = new(functionNode.Name);

        foreach (var node in functionNode.Parameters)
        {
            dynamicFunction.Variables[node.VariableName] = new Variable(
                node.VariableName,
                EvaluateExpression(node.Value),
                node.IsStatic,
                node.IsPrivate,
                node.IsConst,
                node.Type);

            dynamicFunction.Parameters.Add(node.VariableName);
        }

        dynamicFunction.Body = functionNode.Body;
        dynamicFunction.IsStatic = functionNode.IsStatic;
        dynamicFunction.IsPrivate = functionNode.IsPrivate;

        return dynamicFunction;
    }

    private void AddContext(ASTContext context)
    {
        _contextStack.Push(context);
    }

    private object? ExecuteFunction(DynamicFunction? function, List<object?> arguments, DynamicClass? ownerClass, DynamicNamespace? ownerNamespace)
    {
        if (function is null)
            return null;

        Logger.Log($"'{function.Name}'({string.Join(", ", arguments)})");

        var contextClass = ownerClass ?? (HasContext ? CurrentContext.Class : null);
        ASTContext newContext = new() { Function = function, Class = contextClass };

        AddContext(newContext);

        try
        {
            if (arguments.Count == function.Parameters.Count)
                for (var i = 0; i < function.Parameters.Count; i++)
                {
                    var var = function.Variables[function.Parameters[i]];

                    if (!string.IsNullOrWhiteSpace(var.Type) && var.Type != Typeof(arguments[i]))
                        throw new QlangRuntimeException($"The type of param is '{Typeof(arguments[i])}' but must be '{var.Type}'", null, GetStackTrace());
                    
                    function.Variables[function.Parameters[i]] = new Variable(
                        function.Parameters[i],
                        arguments[i],
                        function.IsStatic,
                        false,
                        var.IsConst);
                }
            else
                throw new QlangRuntimeException("The number of arguments must be equal to the number of params",
                    null, GetStackTrace());

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
            Logger.Warn("Return value: " + _returnValue);
            return _returnValue;
        }
        finally
        {
            RestoreContextStack();
        }
    }

    private void ExecuteStatement(ASTNode statement)
    {
        if (HasContext)
            CurrentContext.CurrentNode = statement;
        
        switch (statement)
        {
            case AssignmentNode assign:
                Logger.Warn($"Context: class='{CurrentContext.Class?.Name}' function='{CurrentContext.Function?.Name}'");
                AssignmentNode(assign);
                break;

            case CallNode call:
                Logger.Log("CallNode");
                ExecuteObjectCalls(call);
                break;

            case IfNode ifNode:
                Logger.Log("IfNode");
                ExecuteIf(ifNode);
                break;
            
            case SwitchNode switchNode:
                Logger.Log("SwitchNode");
                ExecuteSwitch(switchNode);
                break;

            case WhileNode whileNode:
                Logger.Log("WhileNode");
                ExecuteWhile(whileNode);
                break;

            case ForNode forNode:
                Logger.Log("ForNode");
                ExecuteFor(forNode);
                break;

            default:
                throw new QlangRuntimeException($"Unknown statement type: {statement.GetType()}", statement, GetStackTrace());
        }
    }

    private void AssignmentNode(AssignmentNode assign)
    {
        if (_contextStack.Count == 0)
            return;

        var value = EvaluateExpression(assign.Value);

        // Handle path-based assignments (e.g., object.property = value)
        if (assign.IsPathAssignment)
        {
            Logger.Log($"path='{assign.GetAssignmentTarget()}' value='{assign.Value}' value(after evaluating)='{value}'", "PathAssignmentNode");
            AssignToPath(assign.Path!, value, assign);
            return;
        }

        Logger.Log($"name='{assign.VariableName}' value='{assign.Value}' value(after evaluating)='{value}'", "AssignmentNode");

        if (value is DynamicClass dynamicClass)
        {
            Logger.Log($"Change name old='{dynamicClass.Name}' new='{assign.VariableName}'", "AssignmentNode");
            dynamicClass.Name = assign.VariableName;
        }

        // Context class
        if (CurrentContext.Class != null)
        {
            if (CurrentContext.Class.Variables.TryGetValue(assign.VariableName, out var var))
            {
                if (var.IsConst)
                    throw new QlangRuntimeException($"Cannot re-assign const variable '{assign.VariableName}'",
                        assign, GetStackTrace());

                CurrentContext.Class.Variables[var.Name] = new Variable(
                    var.Name,
                    value,
                    var.IsStatic,
                    var.IsPrivate,
                    var.IsConst);
                return;
            }
        }

        // Context block
        if (CurrentContext.Blocks.Count > 0)
        {
            for (var i = CurrentContext.Blocks.Count - 1; i >= 0; i--)
            {
                if (!CurrentContext.Blocks[i].Variables.TryGetValue(assign.VariableName, out var var))
                    continue;

                if (var.IsConst)
                    throw new QlangRuntimeException($"Cannot re-assign const variable '{assign.VariableName}'",
                        assign, GetStackTrace());

                CurrentContext.Blocks[i].Variables[var.Name] = new Variable(
                    var.Name,
                    value,
                    var.IsStatic,
                    var.IsPrivate,
                    var.IsConst);
                return;
            }

            if (CurrentContext.Blocks.Count > 0 && assign.IsNew)
            {
                CurrentContext.Blocks[^1].Variables[assign.VariableName] = new Variable(
                    assign.VariableName,
                    value,
                    assign.IsStatic,
                    assign.IsPrivate,
                    assign.IsConst);
                return;
            }
        }

        // Context function
        if (CurrentContext.Function != null)
        {
            if (CurrentContext.Function.Variables.TryGetValue(assign.VariableName, out var var))
            {
                if (var.IsConst)
                    throw new QlangRuntimeException($"Can't re-assign const variable '{assign.VariableName}'",
                        assign, GetStackTrace());

                CurrentContext.Function.Variables[var.Name] = new Variable(
                    var.Name,
                    value,
                    var.IsStatic,
                    var.IsPrivate,
                    var.IsConst);
                return;
            }

            if (assign.IsNew)
            {
                CurrentContext.Function.Variables[assign.VariableName] = new Variable(
                    assign.VariableName,
                    value,
                    assign.IsStatic,
                    assign.IsPrivate,
                    assign.IsConst);
                return;
            }
        }
        
        var globalVar = _globalVariables.FirstOrDefault(v => v.Name == assign.VariableName);
        if (globalVar != null && !assign.IsNew)
        {
            if (globalVar.IsConst)
                throw new QlangRuntimeException($"Cannot re-assign const variable '{assign.VariableName}'",
                    assign, GetStackTrace());
            
            globalVar.Value = value;
            _globalVariables[_globalVariables.IndexOf(globalVar)] = globalVar;
            return;
        }
            
        throw new QlangRuntimeException($"The variable definition is incorrect or the variable has not been created (Variable: '{assign.VariableName}')", assign, GetStackTrace());
    }

    private void AssignToPath(List<ASTNode> path, object value, AssignmentNode assignNode)
    {
        if (path.Count == 0)
            throw new QlangRuntimeException("Assignment path cannot be empty", assignNode, GetStackTrace());

        var lastNode = path[^1];

        var callNode = new CallNode
        {
            Objects = path.SkipLast(1).ToList(), 
            Arguments = path.SkipLast(1).ElementAt(^1) is FunctionPointerNode ptr ? ptr.Arguments : default,
            Line =  lastNode.Line,
            SourceFile = lastNode.SourceFile,
        };

        var currentObject = ExecuteObjectCalls(callNode);

        switch (lastNode)
        {
            case ObjectPointerNode objPtr:
                if (currentObject is DynamicClass targetClass)
                {
                    if (targetClass.Variables.TryGetValue(objPtr.Name, out var existingVar))
                    {
                        if (existingVar.IsConst)
                            throw new QlangRuntimeException($"Cannot re-assign const property '{objPtr.Name}'", assignNode, GetStackTrace());
                        if (existingVar.IsPrivate)
                            throw new QlangRuntimeException("Cannot access to private variable from external source",
                                objPtr, GetStackTrace());

                        targetClass.Variables[objPtr.Name] = new Variable(
                            objPtr.Name,
                            value,
                            existingVar.IsStatic,
                            existingVar.IsPrivate,
                            existingVar.IsConst);
                    }
                    else
                    {
                        // Create new property
                        targetClass.Variables[objPtr.Name] = new Variable(
                            objPtr.Name,
                            value,
                            false,
                            false,
                            false);
                    }
                }
                else
                    throw new QlangRuntimeException($"Cannot assign property '{objPtr.Name}' to non-object type {currentObject?.GetType().Name ?? "null"}", assignNode, GetStackTrace());
                break;

            default:
                throw new QlangRuntimeException($"Invalid assignment target: {lastNode.GetType().Name}", assignNode, GetStackTrace());
        }

        Logger.Log($"Successfully assigned value to path: {string.Join(".", path.Select(p => p switch { ObjectPointerNode op => op.Name, FunctionPointerNode fp => $"{fp.Name}()", _ => "?" }))}", "PathAssignment");
    }

    private DynamicClass GetNewClass(DynamicClass dynamicClass, List<object?> args)
    {
        Logger.Warn("Is new instance class");
        var dClass = dynamicClass.Clone();
        
        var fromClass = GetFunctionFromClass(dClass, "new", args);
        
        if (fromClass.function != null)
            ExecuteFunction(ToDynamicFunction(fromClass.function), fromClass.Args, dClass, null);

        return dClass;
    }

    private void RestoreContextStack()
    {
        if (!HasContext) 
            return;
        
        var context = _contextStack.Pop();
        Logger.Warn($"class='{context.Class?.Name}' function='{context.Function?.Name}'", "RestoreContextStack");
    }

    private List<object?> GetCollection(List<object?> arg)
    {
        if (arg is null)
            throw new QlangRuntimeException("collection is null", null, GetStackTrace());

        if (arg.Count == 0)
            return arg;

        var sb = new StringBuilder(arg.Count * 10);
        sb.Append('[');

        for (var i = 0; i < arg.Count; i++)
        {
            if (i > 0) sb.Append(',');

            if (arg[i] is string)
                sb.Append('"').Append(arg[i]).Append('"');
            else
                sb.Append(arg[i]);
        }
        sb.Append(']');

        Logger.Log(sb.ToString());
        return arg;
    }

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

        var processed = sb.ToString().Replace("\"", "\"\"");
        return csharpString ? $"@\"{processed}\"" : processed;
    }

    private object? EvaluateExpression(ASTNode? expr)
    {
        if (expr is null)
            return null;

        if (HasContext)
            CurrentContext.CurrentNode = expr;

        Logger.Log("TypeofExpression: " + expr.GetType().Name);
        Logger.Log("Expression: " + expr);

        try
        {
            return expr switch
            {
                VariableNode varNode => GetVariableValue(varNode),
                StringRefNode strRef => GetStringRef(strRef),
                NumberRefNode numberRef => GetNumberRef(numberRef),
                ClassNode classNode => ToDynamicClass(classNode),
                BooleanNode booleanNode => booleanNode.Value,
                NumberNode num => num.Value,
                BinaryOperationNode binOp => EvaluateBinaryOperation(binOp),
                CollectionNode collection => GetCollection(collection.Collection.ConvertAll(EvaluateExpression)),
                NullNode => null,
                CallNode call => ExecuteObjectCalls(call),
                FunctionNode => expr,
                _ => throw new QlangRuntimeException(
                    $"Unknown expression type: {expr.GetType().Name}",
                    expr,
                    GetStackTrace())
            };
        }
        catch (QlangRuntimeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.Error("Exception from EvaluateExpression");
            Logger.Error("Expression: " + expr);

            if (expr is BinaryOperationNode binOp)
                Logger.Error($"expr is BinaryOperationNode [{(binOp.Left as VariableNode)?.Name},{binOp
                .Operator},{(binOp.Right as StringRefNode)?.Index}]");

            throw new QlangRuntimeException(
                $"Internal error: {ex}",
                expr,
                GetStackTrace());
        }
    }

    private double DivideWithCheck(object left, object right, BinaryOperationNode node)
    {
        var divisor = right.ToString().ParseNumber();

        if (Math.Abs(divisor) < double.Epsilon)
        {
            throw new QlangRuntimeException(
                "Division by zero",
                node,
                GetStackTrace());
        }
        return left.ToString().ParseNumber() / divisor;
    }

    private object? GetVariableValue(VariableNode varNode)
    {
        try
        {
            Variable? var;
            
            if (varNode.Name == Keywords.ThisKeyword && HasContext)
                return CurrentContext?.Class;

            if (HasContext && CurrentContext.Blocks.Count > 0)
                for (var i = CurrentContext.Blocks.Count; i > 0; i--)
                    if (CurrentContext.Blocks[i].Variables.TryGetValue(varNode.Name, out var))
                        return var.Value;

            if (HasContext && (CurrentContext?.Function?.Variables.TryGetValue(varNode.Name, out var) == true ||
                               CurrentContext?.Class?.Variables.TryGetValue(varNode.Name, out var) == true))
                return var.Value;

            // static classes var
            if (_dynamicClasses.TryGetValue(varNode.ClassName, out var dynamicClass) &&
                dynamicClass.Variables.TryGetValue(varNode.Name, out var))
            {
                if (var.IsPrivate)
                    throw new QlangRuntimeException("Scope can't be call from external class " +
                                                    "because of is private scope" +
                                                    $" (class: {dynamicClass.Name}, scope: {var.Name})",
                        varNode, GetStackTrace());

                return var.Value;
            }
            
            // static classes
            if (_dynamicClasses.TryGetValue(varNode.Name, out dynamicClass))
                return dynamicClass;
            
            // Global variables
            var node = _globalVariables.FirstOrDefault(x => x.Name == varNode.Name);
            if (node != null)
                return node.Value;

            if (CurrentContext?.Class != null)
            {
                Logger.Log("Current var count: " + CurrentContext.Class.Variables.Count);
                Logger.Log("Current class name: " + CurrentContext.Class.Name);
                foreach (var var2 in CurrentContext.Class.Variables)
                {
                    var val = var2.Value.Value;
                    var name = var2.Value.Name;
                    Logger.Log($"\tVariable: '{name}' = '{val}'");
                }
            }
            else Logger.Error($"Context Class is null");

            if (CurrentContext?.Function != null)
            {
                Logger.Log("Current var count: " + CurrentContext.Function.Variables.Count);
                Logger.Log("Current function name: " + CurrentContext.Function.Name);
                foreach (var var2 in CurrentContext.Function.Variables)
                {
                    var val = var2.Value.Value;
                    var name = var2.Value.Name;
                    Logger.Log($"\tVariable: '{name}' = '{val}'");
                }
            }
            else Logger.Error($"Context Class is null");
        }
        catch (QlangRuntimeException)
        {
            Logger.Error("GetVariable exception");
            throw;
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }

        throw new QlangRuntimeException(
            $"Undefined variable: '{varNode.Name}'",
            varNode,
            GetStackTrace());

    }

    private string GetStringRef(StringRefNode strStringRef)
    {
        if (!_stringDictionary.TryGetValue($"___STRING_{strStringRef.Index}___", out var value))
        {
            throw new QlangRuntimeException(
                $"Undefined string reference: {value}",
                strStringRef,
                GetStackTrace());
        }
        return value;
    }

    private object GetNumberRef(NumberRefNode numberRef)
    {
        if (!_numberDictionary.TryGetValue($"___NUMBER_{numberRef.Index}___", out var value))
        {
            throw new QlangRuntimeException(
                $"Undefined number reference: {value}",
                numberRef,
                GetStackTrace());
        }
        return numberRef.IsNegative ? $"-{value}" : value;
    }

    private DynamicClass? CreateClassFrom(object? obj, DynamicClass copy, ASTNode? context = null)
    {
        var createFunction = copy.Body
            .OfType<FunctionNode>()
            .FirstOrDefault(f => f.Name == "___create_from___");

        if (createFunction is null)
            throw new QlangRuntimeException(
                "Second value is incompatible",
                context,
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
                context,
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

        if (binOp.Operator.Any(c => c is '=' or '>' or '<' or '!'))
        {
            var @operator = "";
            for (var i = 0; i < binOp.Operator.Length; i++)
            {
                @operator += binOp.Operator[i] switch
                {
                    '=' => "equal",
                    '>' => "greater",
                    '<' => "less",
                    '!' => "not"
                };
            
                if (i !=  binOp.Operator.Length - 1)
                    @operator += "_";
            }
            binOp.Operator = @operator;
        }
        
        var opFunction = leftClass.Body
            .OfType<FunctionNode>()
            .FirstOrDefault(f =>
                f.Name == $"___operator_{binOp.Operator.ToLower()}___");

        if (opFunction is null)
            throw new QlangRuntimeException(
                $"Class '{leftClass.ClassName}' is incompatible for operator '{binOp.Operator}'",
                binOp,
                GetStackTrace());

        var result = ExecuteFunction(
            ToDynamicFunction(opFunction),
            [leftClass, rightClass],
            leftClass, null);

        if ((result is not DynamicClass dynamicClass || dynamicClass.ClassName != leftClass.ClassName) &&
            binOp.Operator.All(c => c is '+' or '-' or '*' or '/'))
            throw new QlangRuntimeException(
                $"Return value of '___operator_{binOp.Operator.ToLower()}___' must be equal to type '{leftClass.ClassName}'", binOp,
                GetStackTrace()); 
        
        if (result is not bool && 
            (binOp.Operator.Contains("equal") ||  binOp.Operator.Contains("greater") || 
             binOp.Operator.Contains("less")))
            throw new QlangRuntimeException(
                $"Return value of '___operator_{binOp.Operator.ToLower()}___' must be equal to type 'bool'", binOp,
                GetStackTrace()); 
        
        return result;
    }

    private object? EvaluateBinaryOperation(BinaryOperationNode binOp)
    {
        Logger.Warn("Detected binary operation");
        Logger.Warn($"Params: {binOp.Left} {binOp.Operator} {binOp.Right}");
        var left = EvaluateExpression(binOp.Left);
        var right = EvaluateExpression(binOp.Right);
        Logger.Warn($"ExpressionParams: {left}: {left?.GetType().Name}; {right}: {right?.GetType().Name}");

        if (left is null || right is null)
            return null;

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
                            binOp, GetStackTrace());

                    // Short-circuit: если левая часть false, правую не вычисляем
                    if (!leftBool)
                    {
                        Logger.Warn($"Short-circuit &&: left is false, returning false");
                        return false;
                    }

                    if (!bool.TryParse(right.ToString(), out rightBool))
                        throw new QlangRuntimeException(
                            $"Type error: Right operand of '&&' must be boolean, got '{right}'",
                            binOp, GetStackTrace());

                    Logger.Warn($"Operation &&: {leftBool} && {rightBool} = {rightBool}");
                    return rightBool;
                }
            case "||":
                {
                    if (!bool.TryParse(left.ToString(), out leftBool))
                        throw new QlangRuntimeException(
                            $"Type error: Left operand of '||' must be boolean, got '{left}'",
                            binOp, GetStackTrace());

                    // Short-circuit: если левая часть true, правую не вычисляем
                    if (leftBool)
                    {
                        Logger.Warn("LeftRight: " + binOp.Left?.GetTree() + " and " + binOp.Right?.GetTree());
                        Logger.Warn($"Short-circuit ||: left is true, returning true");
                        return true;
                    }

                    Logger.Warn($"Short-circuit ||: left not is true, continue");

                    if (!bool.TryParse(right.ToString(), out rightBool))
                        throw new QlangRuntimeException(
                            $"Type error: Right operand of '||' must be boolean, got '{right}'",
                            binOp, GetStackTrace());

                    Logger.Warn($"Operation ||: {leftBool} || {rightBool} = {rightBool}");
                    return rightBool;
                }
        }

        // If it's bool condition
        if (bool.TryParse(left.ToString(), out leftBool) && bool.TryParse(right.ToString(), out rightBool))
        {
            Logger.Warn($"IsBooleanOperation: {left}{binOp.Operator}{right}");
            return binOp.Operator switch
            {
                "==" => Equals(leftBool, rightBool),
                "!=" => !Equals(leftBool, rightBool),
                _ => throw new QlangRuntimeException(
                    $"Unknown operator for boolean: {binOp.Operator}",
                    binOp,
                    GetStackTrace())
            };
        }

        if (binOp.Operator == "Plus" && (left is string || right is string))
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
                $"'{left?.ToString() ?? "null"}' (type={left?.GetType().Name}) and '{right?.ToString() ?? "null"}' (type={right?.GetType().Name})",
                binOp,
                GetStackTrace());
        }

        try
        {
            var leftNum = Convert.ToDouble(left);
            var rightNum = Convert.ToDouble(right);
            Logger.Warn($"Operation: {left}{binOp.Operator}{right}");
            return binOp.Operator switch
            {
                "==" => Equals(leftNum, rightNum),
                "!=" => !Equals(leftNum, rightNum),
                "Less" => leftNum < rightNum,
                "Greater" => leftNum > rightNum,
                ">=" => leftNum >= rightNum,
                "<=" => leftNum <= rightNum,
                "Plus" => leftNum + rightNum,
                "Minus" => leftNum - rightNum,
                "Star" => leftNum * rightNum,
                "Slash" => DivideWithCheck(leftNum, rightNum, binOp),
                "Percent" => leftNum % rightNum,
                _ => throw new QlangRuntimeException(
                    $"Unknown operator: {binOp.Operator}",
                    binOp,
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
                binOp,
                GetStackTrace());
        }
    }
    
    private (FunctionNode? function, List<object?> args) GetFunctionFromFunctionList
        (string name, List<object?>? args = null)
    {
        args ??= [];
        
        foreach (var function in _globalFunctions.Where(f => f.Name == name))
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null);
    }
    
    private (FunctionNode? function, List<object?> Args) GetFunctionFromClass
        (DynamicClass? @class, string name, List<object?>? args = null)
    {
        if (@class is null)
            return (null, null);
        
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
            return (null, null);
        
        args ??= [];

        var functions = @namespace.Functions;
        var values = @namespace.Variables.Where(f => f.Key == name).Select(var => var.Value.Value);
        
        functions.AddRange(values.OfType<FunctionNode>());
        
        foreach (var function in functions)
            // Проверяем, можно ли вызвать эту функцию с данными аргументами
            if (TryMatchFunction(function, args, out var finalArgs))
                return (function, finalArgs);

        return (null, null);
    }

    private bool TryMatchFunction(FunctionNode function, List<object?> args, out List<object?> finalArgs)
    {
        finalArgs = [];
        
        var requiredParamsCount = 0;
        var totalParamsCount = function.Parameters.Count;
        
        foreach (var t in function.Parameters)
        {
            if (t.Value == null)
                requiredParamsCount++;
            else
                break;
        }
        
        if (args.Count < requiredParamsCount || args.Count > totalParamsCount)
            return false;
        
        for (var i = 0; i < totalParamsCount; i++)
        {
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
        }
        
        return true;
    }
}
