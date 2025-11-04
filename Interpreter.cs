using Qlang.AST;
using Qlang.Dependencies.QlangDependencies;

namespace Qlang;

public class Interpreter(Dictionary<string, string> stringDictionary)
{
    private readonly Dictionary<string, object> _variables = new();
    private readonly Dictionary<string, FunctionNode> _functions = new();

    public void Execute(ProgramNode program)
    {
        // Console.WriteLine($"Execution process started");
        
        // Сначала регистрируем все функции
        foreach (ASTNode statement in program.Statements)
        {
            if (statement is not FunctionNode func)
                continue;
            
            _functions[func.Name] = func;
            // Console.WriteLine($"{func.Name}: {func}");
        }

        // Запускаем функцию main
        if (_functions.TryGetValue("main", out FunctionNode? function))
            ExecuteFunction(function, []);
        else
            throw new Exception("No 'main' function found!");
    }

    private string ExecuteFunction(FunctionNode function, List<object> arguments)
    {
        // Создаём локальные переменные для параметров
        for (int i = 0; i < function.Parameters.Count; i++)
            _variables[function.Parameters[i]] = arguments[i];

        // Выполняем тело функции
        string last = null;
        foreach (ASTNode statement in function.Body)
            if (statement is ReturnNode returnNode)
            {
                last = EvaluateExpression(returnNode.ReturnValue).ToString();
                break;
            }
            else ExecuteStatement(statement);
        
        return last;
    }

    private string ExecuteStatement(ASTNode statement)
    {
        switch (statement)
        {
            case AssignmentNode assign:
                _variables[assign.VariableName] = EvaluateExpression(assign.Value);
                break;

            case MethodCallNode call:
                return ExecuteMethodCall(call);
                break;

            case IfNode ifNode:
                ExecuteIf(ifNode);
                break;

            default:
                throw new Exception($"Unknown statement type: {statement.GetType()}");
        }

        return null;
    }

    private void ExecuteIf(IfNode ifNode)
    {
        bool condition = (bool)EvaluateExpression(ifNode.Condition);

        if (condition)
            foreach (var statement in ifNode.ThenBlock)
                ExecuteStatement(statement);
        else if (ifNode.ElseBlock.Count > 0)
            foreach (var statement in ifNode.ElseBlock)
                ExecuteStatement(statement);
    }

    private string ExecuteMethodCall(MethodCallNode call)
    {
        // Пользовательские функции
        if (call.ObjectName == "this" && _functions.TryGetValue(call.MethodName, out var func))
        {
            return ExecuteFunction(func, call.Arguments.ConvertAll(EvaluateExpression));
        }
        
        // Встроенные классы
        foreach (Class qClass in Namespace.GetClassList())
        {
            if (qClass.GetName() != call.ObjectName) 
                continue;
            
            var function = qClass.GetFunctions().FirstOrDefault(fn => fn.GetName() == call.MethodName);
                
            if (function == null)
                throw new Exception($"Method {call.ObjectName} is not a function!");

            return function.Execute(call.Arguments.ConvertAll(EvaluateExpression).ToArray());
        }

        throw new Exception($"Method {call.MethodName} is not a function!");
    }

    private object EvaluateExpression(ASTNode expr)
    {
        return expr switch
        {
            VariableNode varNode => _variables[varNode.Name],
            StringRefNode strRef => stringDictionary[$"___STRING_{strRef.Index}___"],
            NumberNode num => num.Value,
            BinaryOperationNode binOp => EvaluateBinaryOp(binOp),
            MethodCallNode methodCall => ExecuteMethodCall(methodCall),
            _ => throw new Exception($"Unknown expression type: {expr.GetType()}")
        };
    }

    private object EvaluateBinaryOp(BinaryOperationNode binOp)
    {
        var left = EvaluateExpression(binOp.Left);
        var right = EvaluateExpression(binOp.Right);

        if (binOp.Operator == "Plus" && (left is string || right is string))
            return left.ToString() + right;
        
        return binOp.Operator switch
        {
            "==" => Equals(left, right),
            "Plus" => (double)left + (double)right,
            "Minus" => (double)left - (double)right,
            "Star" => (double)left * (double)right,
            "Slash" => (double)left / (double)right,
            _ => throw new Exception($"Unknown operator: {binOp.Operator}")
        };
    }
}