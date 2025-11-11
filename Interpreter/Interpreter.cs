using Qlang.AST;
using Qlang.Dependencies.QlangDependencies;
using Qlang.Dependencies.QlangDependencies.Classes;
using Qlang.Dependencies.QlangDependencies.Functions;
using Math = System.Math;

namespace Qlang.Interpreter;

public partial class Interpreter
{
    public Interpreter(Dictionary<string, string> stringDictionary)
    {
        _stringDictionary = stringDictionary;
    }

    private readonly Dictionary<string, string> _stringDictionary;
    
    private readonly Dictionary<string, object> _variables = new();
    
    private readonly Dictionary<string, FunctionNode> _functions = new();
    
    private readonly Dictionary<string, ClassNode> _classes = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private ASTContext CurrentContext => _contextStack.Count > 0 ? _contextStack.Peek() : new ASTContext();
    
    public void Execute(ProgramNode program)
    {
        try
        {
            foreach (ASTNode statement in program.Statements)
            {
                switch (statement)
                {
                    case ClassNode classNode:
                        _classes[classNode.Name] = classNode;
                        break;
                    case FunctionNode func:
                        _functions[func.Name] = func;
                        break;
                }
            }

            if (!_functions.TryGetValue("main", out FunctionNode? mainFunction))
            {
                throw new QlangRuntimeException(
                    "No 'main' function found in program",
                    program.Statements.FirstOrDefault() ?? new ProgramNode(),
                    []);
            }
        
            ExecuteFunction(mainFunction, []);
        }
        catch (QlangRuntimeException ex)
        {
            Logger.Logger.Error(ex.ToString());
            throw;
        }
    }

    private bool _break;
    private ReturnNode _return;
    private object ExecuteFunction(FunctionNode? function, List<object> arguments)
    {
        Logger.Logger.Log(function?.GetTree());
    
        ClassNode? node = _contextStack.Count > 0 ? CurrentContext.Class : new ClassNode();
        ASTContext newContext = new() { Function = function, Class = node };
        _contextStack.Push(newContext);

        try
        {
            if (arguments.Count == function.Parameters.Count)
                for (int i = 0; i < function.Parameters.Count; i++)
                    _variables[function.Parameters[i]] = arguments[i];

            string returnValue = null;
            _break = false;
            foreach (ASTNode statement in function.Body)
            {
                if (_break)
                {
                    _break = false;
                    return EvaluateExpression(_return.ReturnValue).ToString();
                }
            
                if (statement is ReturnNode returnNode)
                {
                    returnValue = EvaluateExpression(returnNode.ReturnValue).ToString();
                    break;
                }

                ExecuteStatement(statement);
            }
        
            return returnValue;
        }
        finally
        {
            Logger.Logger.Log("Restored stack: class=" + _contextStack.Pop().Class?.Name);
        }
    }

    private void ExecuteStatement(ASTNode statement)
    {
        switch (statement)
        {
            case AssignmentNode assign:
                object value = EvaluateExpression(assign.Value);
                _variables[assign.VariableName] = value;
                break;

            case MethodCallNode call:
                ExecuteMethodCall(call);
                break;

            case IfNode ifNode:
                ExecuteIf(ifNode);
                break;
            
            case WhileNode whileNode:
                ExecuteWhile(whileNode);
                break;
            
            default:
                throw new QlangRuntimeException($"Unknown statement type: {statement.GetType()}", statement, GetStackTrace());
        }
    }
    
    /// <summary>
    /// Выполняет while-блок
    /// </summary>
    

    /// <summary>
    /// Выполняет вызов метода или функции
    /// </summary>
    private object ExecuteMethodCall(MethodCallNode call)
{
    object[] args = call.Arguments.ConvertAll(EvaluateExpression).ToArray();
    
    Logger.Logger.Log($"@Info<MethodCallNode>: class={call.ObjectName}, method={call.MethodName}");
    Logger.Logger.Log($"@Info<ASTContext>: class={CurrentContext.Class?.Name}, method={CurrentContext.Function?.Name}");

    if (_classes.ContainsKey(call.ObjectName) && call.MethodName == "new")
        return _classes[call.ObjectName];
    
    if (call.ObjectName is "this" or "")
    {
        if (CurrentContext.Class?.Body?
                .FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) is FunctionNode classMethod)
            return ExecuteFunction(classMethod, args.ToList());

        if (_functions.TryGetValue(call.MethodName, out FunctionNode? func))
            return ExecuteFunction(func, args.ToList());
    }
    
    foreach (Class qClass in Namespace.GetClassList())
    {
        if (qClass.GetName() != call.ObjectName) 
            continue;
        
        Function? function = qClass.GetFunctions().FirstOrDefault(fn => fn.GetName() == call.MethodName);
            
        if (function == null)
            throw new QlangRuntimeException($"Method '{call.MethodName}' not found in {call.ObjectName}!", call, 
                GetStackTrace());

        return function.Execute(args);
    }

    if (_classes.TryGetValue(call.ObjectName, out ClassNode? classNode))
        return ExecuteMethodCallClass(classNode, call);

    if (_variables.TryGetValue(call.ObjectName, out object? variable))
    {
        Logger.Logger.Log("Object detected as class pointer", ConsoleColor.Magenta);
        
        ClassNode? @class = variable as ClassNode;

        if (@class?.Body.FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) is FunctionNode
            function)
        {
            CurrentContext.Class = @class;
            return ExecuteFunction(function, args.ToList());
        }
    }
    
    foreach (ASTContext item in _contextStack)
    {
        Logger.Logger.Log($"StackItem: class='{item.Class?.Name}' method='{item.Function?.Name}'");
    }
    Logger.Logger.Log($"CurrentStackItem: class='{CurrentContext.Class?.Name}' method='{CurrentContext.Function?.Name}'");
    
    throw new QlangRuntimeException($"Unknown object/function: {call.ObjectName}.{call.MethodName}({string.Join(",", call.Arguments)})", call, 
        GetStackTrace());
}

    private object ExecuteMethodCallClass(ClassNode classNode, MethodCallNode call)
    {
        if (classNode.Body.FirstOrDefault(astNode => astNode is FunctionNode fn && fn.Name == call.MethodName) is not FunctionNode node)
            throw new QlangRuntimeException($"User class detection error: Unknown object/function: {call.ObjectName}.{call.MethodName}", call, GetStackTrace());
        
        ClassNode? previousClass = CurrentContext?.Class;
    
        if (_contextStack.Count > 0)
        {
            CurrentContext.Class = classNode;
            Logger.Logger.Log("@ContextClass=" + classNode.Name, ConsoleColor.Green);
        }

        try
        {
            List<object> args = call.Arguments?.ConvertAll(EvaluateExpression) ?? [];
            return ExecuteFunction(node, args);
        }
        finally
        {
            // Восстанавливаем предыдущий контекст класса
            if (_contextStack.Count > 0)
                CurrentContext.Class = previousClass;
        }
    }
    
    private object EvaluateExpression(ASTNode expr)
    {
        if (_contextStack.Count > 0)
            CurrentContext.CurrentNode = expr;
    
        try
        {
            return expr switch
            {
                VariableNode varNode => GetVariable(varNode),
                StringRefNode strRef => GetStringRef(strRef),
                NumberNode num => num.Value,
                BinaryOperationNode binOp => EvaluateBinaryOperation(binOp),
                MethodCallNode methodCall => ExecuteMethodCall(methodCall),
                var _ => throw new QlangRuntimeException(
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
            throw new QlangRuntimeException(
                $"Internal error: {ex.Message}", 
                expr, 
                GetStackTrace());
        }
    }
    
    private double DivideWithCheck(object left, object right, BinaryOperationNode node)
    {
        double divisor = Convert.ToDouble(right);
        if (Math.Abs(divisor) < double.Epsilon)
        {
            throw new QlangRuntimeException(
                "Division by zero",
                node,
                GetStackTrace());
        }
        return Convert.ToDouble(left) / divisor;
    }

    private static bool IsNumeric(object value)
    {
        return value is int or long or float or double or decimal;
    }
    
    private object GetVariable(VariableNode varNode)
    {
        if (!_variables.TryGetValue(varNode.Name, out object? value))
        {
            throw new QlangRuntimeException(
                $"Undefined variable: {varNode.Name}", 
                varNode, 
                GetStackTrace());
        }
        return value;
    }
    
    private string GetStringRef(StringRefNode strRef)
    {
        if (!_stringDictionary.TryGetValue($"___STRING_{strRef.Index}___", out string? value))
        {
            throw new QlangRuntimeException(
                $"Undefined string reference: {value}", 
                strRef, 
                GetStackTrace());
        }
        return value;
    }

    private object EvaluateBinaryOperation(BinaryOperationNode binOp)
    {
        object? left = EvaluateExpression(binOp.Left);
        object? right = EvaluateExpression(binOp.Right);

        if (binOp.Operator == "Plus" && (left is string || right is string))
            return left.ToString() + right;
    
        if (!IsNumeric(left) || !IsNumeric(right))
        {
            throw new QlangRuntimeException(
                $"Type error: Cannot apply operator '{binOp.Operator}' to " +
                $"'{left?.GetType().Name ?? "null"}' and '{right?.GetType().Name ?? "null"}'",
                binOp,
                GetStackTrace());
        }
    
        try
        {
            return binOp.Operator switch
            {
                "==" => Equals(left, right),
                "!=" => !Equals(left, right),
                "Less" => Convert.ToDouble(left) < Convert.ToDouble(right),
                "Plus" => Convert.ToDouble(left) + Convert.ToDouble(right),
                "Slash" => DivideWithCheck(left, right, binOp),
                var _ => throw new QlangRuntimeException(
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