using System.Text;
using Qlang.Core.Lang.AST;
using Qlang.Core.Lang.Dynamic;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.Lang.Interpreter.Native;
using Qlang.Core.LangDebug;
using Math = System.Math;

namespace Qlang.Core.Lang.Interpreter;

public partial class Interpreter
{
    public Interpreter(Dictionary<string, string> stringDictionary, Dictionary<string, object> numberDictionary)
    {
        _stringDictionary = stringDictionary;
        _numberDictionary = numberDictionary;
    }

    private readonly NativeFunctionRegistry _nativeFunctions = new();

    private readonly Dictionary<string, string> _stringDictionary;

    private readonly Dictionary<string, object> _numberDictionary;

    private readonly Dictionary<string, DynamicFunction> _functions = new();

    private readonly Dictionary<string, DynamicClass> _dynamicClasses = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private bool HasContext => _contextStack.Count > 0;
    private ASTContext? CurrentContext => HasContext ? _contextStack.Peek() : null;

    public void Execute(ProgramNode program)
    {
        Logger.SetLoggerPath(Path.Combine("Logs", "Debug", "debug_interpreter.log"));
        Logger.Warn("----------- Interpreter -----------");

        foreach (var statement in program.Statements)
        {
            switch (statement)
            {
                case ClassNode classNode:
                    Logger.Log("ToDynamicClass (until): " + classNode.Name);
                    _dynamicClasses[classNode.Name] = ToDynamicClass(classNode);
                    Logger.Log("ToDynamicClass: " + _dynamicClasses[classNode.Name].Name);
                    break;
                case FunctionNode func:
                    _functions[func.Name] = ToDynamicFunction(func);
                    break;
            }
        }

        if (!_functions.TryGetValue("main", out var mainFunction))
        {
            throw new QlangRuntimeException(
                "No 'main' function found in program",
                program.Statements.FirstOrDefault() ?? new ProgramNode(),
                []);
        }

        ExecuteFunction(mainFunction, [], null);
    }

    private DynamicClass ToDynamicClass(ClassNode classNode)
    {
        DynamicClass dynamicClass = new(classNode.Name);

        foreach (var node in classNode.Body)
            if (node is AssignmentNode assignmentNode)
                dynamicClass.Variables[assignmentNode.VariableName] = new Variable(assignmentNode.VariableName,
                    EvaluateExpression(assignmentNode.Value), assignmentNode.IsStatic, assignmentNode
                    .IsPrivate, assignmentNode.IsConst);

        dynamicClass.Body = classNode.Body;

        return dynamicClass;
    }

    private DynamicFunction ToDynamicFunction(FunctionNode functionNode)
    {
        DynamicFunction dynamicFunction = new(functionNode.Name);

        foreach (var node in functionNode.Parameters)
        {
            dynamicFunction.Variables[node.VariableName] = new Variable(
                node.VariableName,
                EvaluateExpression(node.Value),
                node.IsStatic,
                node.IsPrivate,
                node.IsConst);

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

    private object? ExecuteFunction(DynamicFunction? function, List<object> arguments, DynamicClass? ownerClass)
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
                    function.Variables[function.Parameters[i]] = new Variable(
                        function.Parameters[i],
                        arguments[i],
                        function.IsStatic,
                        false,
                        false);
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
            return _returnValue;
        }
        finally
        {
            RestoreContextStack();
        }
    }

    private void ExecuteStatement(ASTNode statement)
    {
        switch (statement)
        {
            case AssignmentNode assign:
                Logger.Warn($"Context: class='{CurrentContext.Class?.Name}' function='{CurrentContext.Function?.Name}'");
                AssignmentNode(assign);
                break;

            case CallNode call:
                Logger.Log("CallNode");
                ExecuteObjectCalls(call);
                // ExecuteMethodCall(call);
                break;

            case IfNode ifNode:
                Logger.Log("IfNode");
                ExecuteIf(ifNode);
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
            for (int i = CurrentContext.Blocks.Count - 1; i >= 0; i--)
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

            if (CurrentContext.Blocks.Count > 0)
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

            CurrentContext.Function.Variables[assign.VariableName] = new Variable(
                assign.VariableName,
                value,
                assign.IsStatic,
                assign.IsPrivate,
                assign.IsConst);
            return;
        }

        throw new QlangRuntimeException("Place to assign is not detected", assign, GetStackTrace());
    }

    private void AssignToPath(List<ASTNode> path, object value, AssignmentNode assignNode)
    {
        if (path.Count == 0)
            throw new QlangRuntimeException("Assignment path cannot be empty", assignNode, GetStackTrace());

        // Start with the first object in the path
        object? currentObject = null;
        var firstNode = path[0];

        // Get the root object
        switch (firstNode)
        {
            case ObjectPointerNode objPtr:
                // Look up the variable in the current context
                if (HasContext)
                {
                    // Check blocks first
                    if (CurrentContext.Blocks.Count > 0)
                    {
                        for (var i = CurrentContext.Blocks.Count - 1; i >= 0; i--)
                        {
                            if (CurrentContext.Blocks[i].Variables.TryGetValue(objPtr.Name, out var blockVar))
                            {
                                currentObject = blockVar.Value;
                                break;
                            }
                        }
                    }

                    // Check function variables
                    if (currentObject == null && CurrentContext.Function?.Variables.TryGetValue(objPtr.Name, out var funcVar) == true)
                        currentObject = funcVar.Value;

                    // Check class variables
                    if (currentObject == null && CurrentContext.Class?.Variables.TryGetValue(objPtr.Name, out var classVar) == true)
                        currentObject = classVar.Value;

                    // Check static classes
                    if (currentObject == null && _dynamicClasses.TryGetValue(objPtr.Name, out var staticClass))
                        currentObject = staticClass;
                }

                if (currentObject == null)
                    throw new QlangRuntimeException($"Undefined variable in assignment path: '{objPtr.Name}'", assignNode, GetStackTrace());
                break;

            case FunctionPointerNode funcPtr:
                // Execute function to get the object
                currentObject = ExecuteFunction(
                    _functions.GetValueOrDefault(funcPtr.Name),
                    funcPtr.Arguments.ConvertAll(EvaluateExpression),
                    null);
                break;

            default:
                throw new QlangRuntimeException($"Invalid path start: {firstNode.GetType().Name}", assignNode, GetStackTrace());
        }

        // Navigate through the rest of the path (except the last element)
        for (int i = 1; i < path.Count - 1; i++)
        {
            var pathNode = path[i];

            switch (pathNode)
            {
                case ObjectPointerNode objPtr:
                    if (currentObject is DynamicClass dynamicClass)
                    {
                        if (dynamicClass.Variables.TryGetValue(objPtr.Name, out var variable))
                        {
                            if (variable.IsPrivate)
                                throw new QlangRuntimeException("Cannot access to private variable from external source",
                                    objPtr, GetStackTrace());
                            currentObject = variable.Value;
                        }
                        else
                            throw new QlangRuntimeException($"Property '{objPtr.Name}' not found on object", assignNode, GetStackTrace());
                    }
                    else
                        throw new QlangRuntimeException($"Cannot access property '{objPtr.Name}' on non-object type {currentObject?.GetType().Name ?? "null"}", assignNode, GetStackTrace());
                    break;

                case FunctionPointerNode funcPtr:
                    // Execute method on current object
                    if (currentObject is DynamicClass objClass)
                    {
                        var method = objClass.Body.OfType<FunctionNode>().FirstOrDefault(f => f.Name == funcPtr.Name);
                        if (method != null)
                        {
                            if (method.IsPrivate)
                                throw new QlangRuntimeException("Cannot access to private variable from external source",
                                    funcPtr, GetStackTrace());
                            currentObject = ExecuteFunction(
                                ToDynamicFunction(method),
                                funcPtr.Arguments.ConvertAll(EvaluateExpression),
                                objClass);
                        }
                        else
                            throw new QlangRuntimeException($"Method '{funcPtr.Name}' not found on object", assignNode, GetStackTrace());
                    }
                    else
                        throw new QlangRuntimeException($"Cannot call method '{funcPtr.Name}' on non-object type {currentObject?.GetType().Name ?? "null"}", assignNode, GetStackTrace());
                    break;

                default:
                    throw new QlangRuntimeException($"Invalid path element: {pathNode.GetType().Name}", assignNode, GetStackTrace());
            }
        }

        // Handle the final assignment
        var lastNode = path[^1];
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
                {
                    throw new QlangRuntimeException($"Cannot assign property '{objPtr.Name}' to non-object type {currentObject?.GetType().Name ?? "null"}", assignNode, GetStackTrace());
                }
                break;

            default:
                throw new QlangRuntimeException($"Invalid assignment target: {lastNode.GetType().Name}", assignNode, GetStackTrace());
        }

        Logger.Log($"Successfully assigned value to path: {string.Join(".", path.Select(p => p switch { ObjectPointerNode op => op.Name, FunctionPointerNode fp => $"{fp.Name}()", _ => "?" }))}", "PathAssignment");
    }

    private DynamicClass GetNewClass(DynamicClass dynamicClass, List<object> args)
    {
        Logger.Warn("Is new instance class");
        var functionNode = dynamicClass.Body.FirstOrDefault(node => node is FunctionNode { Name: "new" });

        var dClass = dynamicClass.Clone();

        if (functionNode != null)
            ExecuteFunction(ToDynamicFunction(functionNode as FunctionNode), args, dClass);

        return dClass;
    }

    private void RestoreContextStack()
    {
        if (HasContext)
        {
            var context = _contextStack.Pop();
            Logger.Warn($"class='{context.Class?.Name}' function='{context.Function?.Name}'", "RestoreContextStack");
        }
    }

    private List<object> GetCollection(List<object>? arg)
    {
        if (arg is null)
            throw new QlangRuntimeException("collection is null", null, GetStackTrace());

        if (arg.Count == 0)
            return arg;

        var sb = new StringBuilder(arg.Count * 10);
        sb.Append('[');

        for (int i = 0; i < arg.Count; i++)
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

    // WARNING: is ChatGPT code
    private static string ParseString(ReadOnlySpan<char> input, bool csharpString = false)
    {
        var sb = new StringBuilder(input.Length);

        for (int i = 0; i < input.Length; i++)
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

        try
        {
            List<object> args = [];
            if (expr is CollectionNode collectionNode)
            {
                args = collectionNode.Collection.ConvertAll(EvaluateExpression);
                Logger.Log("Is collection expression: " + args);
            }

            return expr switch
            {
                VariableNode varNode => GetVariable(varNode),
                StringRefNode strRef => GetStringRef(strRef),
                NumberRefNode numberRef => GetNumberRef(numberRef),
                BooleanNode booleanNode => booleanNode.Value,
                NumberNode num => num.Value,
                BinaryOperationNode binOp => EvaluateBinaryOperation(binOp),
                CollectionNode collection => GetCollection(collection.Collection.ConvertAll(EvaluateExpression)),
                NullNode => null,
                CallNode call => ExecuteObjectCalls(call),
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

    private object? GetVariable(VariableNode varNode)
    {
        try
        {
            Variable? var;

            if (HasContext && CurrentContext.Blocks.Count > 0)
                for (var i = CurrentContext.Blocks.Count; i > 0; i--)
                    if (CurrentContext.Blocks[i].Variables.TryGetValue(varNode.Name, out var))
                        return var.Value;

            if (HasContext && CurrentContext.Function?.Variables.TryGetValue(varNode.Name, out var) == true)
                return var.Value;

            if (HasContext && CurrentContext.Class?.Variables.TryGetValue(varNode.Name, out var) == true)
                return var.Value;


            // static classes
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

            if (CurrentContext?.Class != null)
            {
                Logger.Log("Current var count: " + CurrentContext.Class.Variables.Count);
                Logger.Log("Current class name: " + CurrentContext.Class.Name);
                foreach (var var2 in CurrentContext.Class.Variables)
                {
                    object val = var2.Value.Value;
                    string name = var2.Value.Name;
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
                    object val = var2.Value.Value;
                    string name = var2.Value.Name;
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

    private object? EvaluateBinaryOperation(BinaryOperationNode binOp)
    {
        Logger.Warn("Detected binary operation");
        Logger.Warn($"Params: {binOp.Left} {binOp.Operator} {binOp.Right}");
        var left = EvaluateExpression(binOp.Left);
        var right = EvaluateExpression(binOp.Right);
        Logger.Warn($"ExpressionParams: {left}: {left?.GetType().Name}; {right}: {right?.GetType().Name}");

        if (left is null)
            throw new QlangRuntimeException("Left part of binary operation is null", binOp, GetStackTrace());
        if (right is null)
            throw new QlangRuntimeException("Right part of binary operation is null", binOp, GetStackTrace());

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
                        Logger.Warn("LeftRight: " + binOp.Left.GetTree() + " and " + binOp.Right.GetTree());
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
            return left.ToString() + right.ToString();

        if (!left.ToString().IsNumber() || !right.ToString().IsNumber())
        {
            if (binOp.Operator is "==" or "!=")
                return binOp.Operator switch
                {
                    "==" => Equals(left, right),
                    "!=" => !Equals(left, right),
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
}
