using Qlang.AST;
using Qlang.Dependencies;
using Qlang.Dependencies.QlangDependencies;
using Qlang.Dependencies.QlangDependencies.Classes;
using Qlang.Dependencies.QlangDependencies.Functions;
using Qlang.Dynamic;
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
    
    private readonly Dictionary<string, DynamicClass> _dynamicClasses = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private ASTContext CurrentContext => _contextStack.Count > 0 ? _contextStack.Peek() : new ASTContext();
    
    public void Execute(ProgramNode program)
    {
        try
        {
            foreach (var statement in program.Statements)
            {
                switch (statement)
                {
                    case ClassNode classNode:
                        _dynamicClasses[classNode.Name] = ToDynamicClass(classNode);
                        break;
                    case FunctionNode func:
                        _functions[func.Name] = func;
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
        
            ExecuteFunction(mainFunction, []);
        }
        catch (QlangRuntimeException ex)
        {
            Logger.Logger.Error(ex.ToString());
            throw;
        }
    }

    private DynamicClass ToDynamicClass(ClassNode classNode)
    {
        DynamicClass dynamicClass = new(classNode.Name);

        foreach (var node in classNode.Body)
            if (node is AssignmentNode assignmentNode)
                dynamicClass.Variables[assignmentNode.VariableName] = EvaluateExpression(assignmentNode.Value);
        
        dynamicClass.Body = classNode.Body;
        
        return dynamicClass;
    }

    private string GetQType(object? o)
    {
        if (o is null)
            return "null";
        
        if (o is ClassNode classNode)
            return classNode.Name.Trim();
        return o.GetType().ToString().StartsWith("System.") ? 
            o.GetType().ToString()["System.".Length..].Trim() 
            : o.GetType().ToString().Trim();
    }

    private bool _break;
    private ReturnNode _return;
    private object ExecuteFunction(FunctionNode? function, List<object> arguments)
    {
        if (function is null)
            return null;
        
        var node = _contextStack.Count > 0 ? CurrentContext.Class : new DynamicClass("");
        ASTContext newContext = new() { Function = function, Class = node };
        _contextStack.Push(newContext);

        try
        {
            if (arguments.Count == function.Parameters.Count)
                for (var i = 0; i < function.Parameters.Count; i++)
                    _variables[function.Parameters[i].VariableName] = arguments[i];

            string returnValue = null;
            _break = false;
            foreach (var statement in function.Body)
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
                Logger.Logger.Log("Interpreter: Execute statement (AssignmentNode)");
                var value = EvaluateExpression(assign.Value);
                _variables[assign.VariableName] = value;
                break;

            case MethodCallNode call:
                Logger.Logger.Log("Interpreter: Execute statement (MehtodCallNode)");
                ExecuteMethodCall(call);
                break;

            case IfNode ifNode:
                Logger.Logger.Log("Interpreter: Execute statement (IfNode)");
                ExecuteIf(ifNode);
                break;
            
            case WhileNode whileNode:
                Logger.Logger.Log("Interpreter: Execute statement (WhileNode)");
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
        
        Logger.Logger.Log($"Interpreter.ExecuteMethodCall: class = '{call.ObjectName}'; method = '{call.MethodName}'");
        Logger.Logger.Log($"Interpreter.CurrentContext: class = '{CurrentContext.Class?.Name}', method = '{CurrentContext.Function?.Name}'");

        if (_dynamicClasses.TryGetValue(call.ObjectName, out var value) && call.MethodName == "new")
            return value;

        if (call is { ObjectName: "", MethodName: "csharp" })
        {
            var returnValue = CSharp.Execute(args.FirstOrDefault());
            Logger.Logger.Warn("Interpreter.ExecuteMethodCall: csharp.return_value: " + (returnValue == null ? "null" :
                returnValue.ToString()));
            return returnValue;
        }
        
        if (call.ObjectName is "this" or "")
        {
            if (CurrentContext.Class?.Body?
                    .FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) is FunctionNode classMethod)
                return ExecuteFunction(classMethod, args.ToList());

            if (_functions.TryGetValue(call.MethodName, out var func))
                return ExecuteFunction(func, args.ToList());
        }

        if (_dynamicClasses.TryGetValue(call.ObjectName, out var classNode))
            return ExecuteMethodCallClass(classNode, call);

        if (_variables.TryGetValue(call.ObjectName, out var variable))
        {
            var varValue = variable;
            
            Logger.Logger.Log("Interpreter.ExecuteMethodCall: Object detected as class pointer");
            
            var @class = varValue as DynamicClass;

            if (@class?.Body.FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) is FunctionNode
                function)
            {
                CurrentContext.Class = @class;
                return ExecuteFunction(function, args.ToList());
            }
        }
        
        foreach (var item in _contextStack)
            Logger.Logger.Log($"Interpreter.ExecuteMethodCall.StackItem: class = '{item.Class?.Name}' method = '{item.Function?.Name}'");
        Logger.Logger.Log($"Interpreter.CurrentContext (After call): class = '{CurrentContext.Class?.Name}' method = '{CurrentContext.Function?.Name}'");
        
        throw new QlangRuntimeException($"Unknown object/function: {call.ObjectName}.{call.MethodName}({string.Join(",", call.Arguments)})", call, 
            GetStackTrace());
    }

    private object ExecuteMethodCallClass(DynamicClass classNode, MethodCallNode call)
    {
        if (classNode.Body.FirstOrDefault(astNode => astNode is FunctionNode fn && fn.Name == call.MethodName) is not FunctionNode node)
            throw new QlangRuntimeException($"User class detection error: Unknown object/function: {call.ObjectName}.{call.MethodName}", call, GetStackTrace());
        
        var previousClass = CurrentContext?.Class;
    
        if (_contextStack.Count > 0)
        {
            CurrentContext.Class = classNode;
            Logger.Logger.Succ("ExecuteMethodCallClass.ContextClass: " + classNode.Name);
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
    
    private object EvaluateExpression(ASTNode? expr)
    {
        if (expr is null)
            return null;
        
        if (_contextStack.Count > 0)
            CurrentContext.CurrentNode = expr;
        
        Logger.Logger.Log("TypeofExpression: " + expr.GetType().Name);
    
        try
        {
            return expr switch
            {
                VariableNode varNode => GetVariable(varNode),
                StringRefNode strRef => GetStringRef(strRef),
                BooleanNode booleanNode => booleanNode.Value,
                NumberNode num => num.Value,
                BinaryOperationNode binOp => EvaluateBinaryOperation(binOp),
                MethodCallNode methodCall => ExecuteMethodCall(methodCall),
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
            throw new QlangRuntimeException(
                $"Internal error: {ex.Message}", 
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
    
    private object GetVariable(VariableNode varNode)
    {
        if (_variables.TryGetValue(varNode.Name, out var value))
            return value;

        if (_contextStack.Count > 0 && CurrentContext?.Class != null && CurrentContext.Class.Variables.TryGetValue(varNode.Name, out value))
            return value;
        
        throw new QlangRuntimeException(
            $"Undefined variable: {varNode.Name}", 
            varNode, 
            GetStackTrace());
        
    }
    
    private string GetStringRef(StringRefNode strRef)
    {
        if (!_stringDictionary.TryGetValue($"___STRING_{strRef.Index}___", out var value))
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
        Logger.Logger.Warn("Detected binary operation");
        var left = EvaluateExpression(binOp.Left);
        var right = EvaluateExpression(binOp.Right);
        
        Logger.Logger.Log($"EvaluateBinaryOperation.IsNumeric: ({left.ToString()})=" + left.ToString().IsNumber());
        Logger.Logger.Log($"EvaluateBinaryOperation.IsNumeric: ({right.ToString()})=" + right.ToString().IsNumber());

        if (bool.TryParse(left.ToString(), out var leftBool) && bool.TryParse(right.ToString(), out var rightBool))
        {
            Logger.Logger.Warn($"IsBooleanOperation: {left}{binOp.Operator}{right}");
            return binOp.Operator switch
            {
                "==" => Equals(leftBool, rightBool),
                "!=" => !Equals(leftBool, rightBool)
            };
        }

        if (binOp.Operator == "Plus" && (left.ToString().IsNumber() == false || right.ToString().IsNumber() == false))
            return left.ToString() + right.ToString();

        if (left.ToString().IsNumber() == false || right.ToString().IsNumber() == false)
        {
            
            throw new QlangRuntimeException(
                $"Type error: Cannot apply operator '{binOp.Operator}' to " +
                $"'{left?.ToString() ?? "null"}' (type={left?.GetType().Name}) and '{right?.ToString() ?? "null"}' (type={right?.GetType().Name})",
                binOp,
                GetStackTrace());
        }
    
        try
        {
            var leftNum = left.ToString().ParseNumber();
            var rightNum = right.ToString().ParseNumber();
            return binOp.Operator switch
            {
                "==" => Equals(left, right),
                "!=" => !Equals(left, right),
                "Less" => leftNum < rightNum,
                "Greater" => leftNum > rightNum,
                ">=" => leftNum >= rightNum,
                "<=" => leftNum <= rightNum,
                "Plus" => leftNum + rightNum,
                "Minus" => leftNum - rightNum,
                "Star" => leftNum * rightNum,
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