// using Qlang.AST;
// using Qlang.Dependencies.QlangDependencies;
//
// namespace Qlang;
//
// public class Interpreter(Dictionary<string, string> stringDictionary)
// {
//     private readonly Dictionary<string, object> _variables = new();
//     private readonly Dictionary<string, FunctionNode> _functions = new();
//
//     public void Execute(ProgramNode program)
//     {
//         // Console.WriteLine($"Execution process started");
//         
//         // Сначала регистрируем все функции
//         foreach (ASTNode statement in program.Statements)
//         {
//             if (statement is not FunctionNode func)
//                 continue;
//             
//             _functions[func.Name] = func;
//             // Console.WriteLine($"{func.Name}: {func}");
//         }
//
//         // Запускаем функцию main
//         if (_functions.TryGetValue("main", out FunctionNode? function))
//             ExecuteFunction(function, []);
//         else
//             throw new Exception("No 'main' function found!");
//     }
//
//     private string ExecuteFunction(FunctionNode function, List<object> arguments)
//     {
//         // Создаём локальные переменные для параметров
//         for (int i = 0; i < function.Parameters.Count; i++)
//             _variables[function.Parameters[i]] = arguments[i];
//
//         // Выполняем тело функции
//         string last = null;
//         foreach (ASTNode statement in function.Body)
//             if (statement is ReturnNode returnNode)
//             {
//                 last = EvaluateExpression(returnNode.ReturnValue).ToString();
//                 break;
//             }
//             else ExecuteStatement(statement);
//         
//         return last;
//     }
//
//     private string ExecuteStatement(ASTNode statement)
//     {
//         switch (statement)
//         {
//             case AssignmentNode assign:
//                 _variables[assign.VariableName] = EvaluateExpression(assign.Value);
//                 break;
//
//             case MethodCallNode call:
//                 return ExecuteMethodCall(call);
//                 break;
//
//             case IfNode ifNode:
//                 ExecuteIf(ifNode);
//                 break;
//
//             default:
//                 throw new Exception($"Unknown statement type: {statement.GetType()}");
//         }
//
//         return null;
//     }
//
//     private void ExecuteIf(IfNode ifNode)
//     {
//         bool condition = (bool)EvaluateExpression(ifNode.Condition);
//
//         if (condition)
//             foreach (var statement in ifNode.ThenBlock)
//                 ExecuteStatement(statement);
//         else if (ifNode.ElseBlock.Count > 0)
//             foreach (var statement in ifNode.ElseBlock)
//                 ExecuteStatement(statement);
//     }
//
//     private string ExecuteMethodCall(MethodCallNode call)
//     {
//         // Пользовательские функции
//         if (call.ObjectName == "this" && _functions.TryGetValue(call.MethodName, out var func))
//         {
//             return ExecuteFunction(func, call.Arguments.ConvertAll(EvaluateExpression));
//         }
//         
//         // Встроенные классы
//         foreach (Class qClass in Namespace.GetClassList())
//         {
//             if (qClass.GetName() != call.ObjectName) 
//                 continue;
//             
//             var function = qClass.GetFunctions().FirstOrDefault(fn => fn.GetName() == call.MethodName);
//                 
//             if (function == null)
//                 throw new Exception($"Method {call.ObjectName} is not a function!");
//
//             return function.Execute(call.Arguments.ConvertAll(EvaluateExpression).ToArray());
//         }
//
//         throw new Exception($"Method {call.MethodName} is not a function!");
//     }
//
//     private object EvaluateExpression(ASTNode expr)
//     {
//         return expr switch
//         {
//             VariableNode varNode => _variables[varNode.Name],
//             StringRefNode strRef => stringDictionary[$"___STRING_{strRef.Index}___"],
//             NumberNode num => num.Value,
//             BinaryOperationNode binOp => EvaluateBinaryOp(binOp),
//             MethodCallNode methodCall => ExecuteMethodCall(methodCall),
//             _ => throw new Exception($"Unknown expression type: {expr.GetType()}")
//         };
//     }
//
//     private object EvaluateBinaryOp(BinaryOperationNode binOp)
//     {
//         var left = EvaluateExpression(binOp.Left);
//         var right = EvaluateExpression(binOp.Right);
//
//         if (binOp.Operator == "Plus" && (left is string || right is string))
//             return left.ToString() + right;
//         
//         return binOp.Operator switch
//         {
//             "==" => Equals(left, right),
//             "Plus" => (double)left + (double)right,
//             "Minus" => (double)left - (double)right,
//             "Star" => (double)left * (double)right,
//             "Slash" => (double)left / (double)right,
//             _ => throw new Exception($"Unknown operator: {binOp.Operator}")
//         };
//     }
// }

using Qlang.AST;
using Qlang.Dependencies.QlangDependencies;

namespace Qlang;

public class Interpreter(Dictionary<string, string> stringDictionary)
{
    // Хранилище переменных: "$x" -> значение
    private readonly Dictionary<string, object> _variables = new();
    
    // Хранилище пользовательских функций: "myFunc" -> FunctionNode
    private readonly Dictionary<string, FunctionNode> _functions = new();

    /// <summary>
    /// Главная точка входа: выполняет всю программу
    /// </summary>
    public void Execute(ProgramNode program)
    {
        // ШАГ 1: Регистрируем все функции (чтобы можно было вызывать до объявления)
        foreach (ASTNode statement in program.Statements)
        {
            if (statement is FunctionNode func)
                _functions[func.Name] = func;
        }

        // ШАГ 2: Запускаем функцию main()
        if (_functions.TryGetValue("main", out FunctionNode? mainFunction))
            ExecuteFunction(mainFunction, []);
        else
            throw new Exception("No 'main' function found!");
    }

    /// <summary>
    /// Выполняет пользовательскую функцию с аргументами
    /// </summary>
    ///
    private bool _break;
    private ReturnNode _return;
    private string ExecuteFunction(FunctionNode function, List<object> arguments)
    {
        // Привязываем аргументы к параметрам функции
        // Например: function greet($name) -> arguments[0] = "Alice"
        for (int i = 0; i < function.Parameters.Count; i++)
            _variables[function.Parameters[i]] = arguments[i];

        // Выполняем тело функции построчно
        string returnValue = null;
        _break = false;
        foreach (ASTNode statement in function.Body)
        {
            if (_break)
                return _return.ReturnValue.ToString();
            if (statement is ReturnNode returnNode)
            {
                // Если встретили return - вычисляем значение и выходим
                returnValue = EvaluateExpression(returnNode.ReturnValue).ToString();
                break;
            }
            else
            {
                // Иначе просто выполняем statement
                ExecuteStatement(statement);
            }
        }
        
        return returnValue;
    }

    /// <summary>
    /// ВЫПОЛНЯЕТ ДЕЙСТВИЕ (statement) - не возвращает значение
    /// Примеры: $x = 5, Term.print("Hi"), if $x == 5: ...
    /// </summary>
    private bool ExecuteStatement(ASTNode statement)
    {
        switch (statement)
        {
            // Присваивание: $x = выражение
            case AssignmentNode assign:
                // 1. Вычисляем правую часть (выражение)
                object value = EvaluateExpression(assign.Value);
                // 2. Сохраняем в переменную
                _variables[assign.VariableName] = value;
                break;

            // Вызов метода: Term.print(...) или ask(...)
            case MethodCallNode call:
                // Выполняем вызов (может быть side effect, например print)
                ExecuteMethodCall(call);
                break;

            // Условие: if $x == 5: ...
            case IfNode ifNode:
                _break = ExecuteIf(ifNode);
                return _break;

            default:
                throw new Exception($"Unknown statement type: {statement.GetType()}");
        }

        return false; // Statements не возвращают значения
    }

    /// <summary>
    /// Выполняет if-блок
    /// </summary>
    private bool ExecuteIf(IfNode ifNode)
    {
        // 1. Вычисляем условие (expression -> bool)
        bool condition = (bool)EvaluateExpression(ifNode.Condition);

        // 2. Выполняем нужный блок
        bool isBreak = false;
        if (condition)
        {
            foreach (var statement in ifNode.ThenBlock)
            {
                if (statement is ReturnNode || isBreak)
                    return true;
                isBreak = ExecuteStatement(statement);
            }
        }
        else if (ifNode.ElseBlock.Count > 0)
        {
            foreach (var statement in ifNode.ElseBlock)
            {
                if (statement is ReturnNode || isBreak)
                    return true;
                isBreak = ExecuteStatement(statement);
            }
        }

        return false;
    }

    /// <summary>
    /// Выполняет вызов метода или функции
    /// </summary>
    private string ExecuteMethodCall(MethodCallNode call)
    {
        // 1. ПОЛЬЗОВАТЕЛЬСКИЕ ФУНКЦИИ: this.myFunc(...)
        if (call.ObjectName == "this" && _functions.TryGetValue(call.MethodName, out var func))
        {
            // Вычисляем все аргументы и вызываем функцию
            List<object> args = call.Arguments.ConvertAll(EvaluateExpression);
            return ExecuteFunction(func, args);
        }
        
        // 2. ВСТРОЕННЫЕ КЛАССЫ: Term.print(...), IO.readFile(...)
        foreach (Class qClass in Namespace.GetClassList())
        {
            if (qClass.GetName() != call.ObjectName) 
                continue;
            
            // Ищем метод в классе
            var function = qClass.GetFunctions().FirstOrDefault(fn => fn.GetName() == call.MethodName);
                
            if (function == null)
                throw new Exception($"Method '{call.MethodName}' not found in {call.ObjectName}!");

            // Вычисляем аргументы и вызываем встроенный метод
            object[] args = call.Arguments.ConvertAll(EvaluateExpression).ToArray();
            return function.Execute(args);
        }

        throw new Exception($"Unknown object/function: {call.ObjectName}.{call.MethodName}");
    }

    /// <summary>
    /// ВЫЧИСЛЯЕТ ЗНАЧЕНИЕ (expression) - ВСЕГДА возвращает результат
    /// Примеры: $x, "hello", 2 + 3, ask("Name?")
    /// </summary>
    private object EvaluateExpression(ASTNode expr)
    {
        return expr switch
        {
            // Переменная: $x -> её значение
            VariableNode varNode => _variables[varNode.Name],
            
            // Строка: ___STRING_0___ -> "actual string"
            StringRefNode strRef => stringDictionary[$"___STRING_{strRef.Index}___"],
            
            // Число: 42
            NumberNode num => num.Value,
            
            // Бинарная операция: 2 + 3, $x == 5
            BinaryOperationNode binOp => EvaluateBinaryOp(binOp),
            
            // Вызов функции/метода: ask("Name?")
            MethodCallNode methodCall => ExecuteMethodCall(methodCall),
            
            _ => throw new Exception($"Unknown expression type: {expr.GetType()}")
        };
    }

    /// <summary>
    /// Вычисляет бинарные операторы: +, -, *, /, ==
    /// </summary>
    private object EvaluateBinaryOp(BinaryOperationNode binOp)
    {
        // 1. Вычисляем обе стороны
        var left = EvaluateExpression(binOp.Left);
        var right = EvaluateExpression(binOp.Right);

        // 2. Конкатенация строк (если хотя бы одна сторона - строка)
        if (binOp.Operator == "Plus" && (left is string || right is string))
            return left.ToString() + right;
        
        // 3. Обычные операторы
        return binOp.Operator switch
        {
            "==" => Equals(left, right),           // Сравнение
            "Plus" => (double)left + (double)right,   // Сложение
            "Minus" => (double)left - (double)right,  // Вычитание
            "Star" => (double)left * (double)right,   // Умножение
            "Slash" => (double)left / (double)right,  // Деление
            _ => throw new Exception($"Unknown operator: {binOp.Operator}")
        };
    }
}