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
                return EvaluateExpression(_return.ReturnValue) as string;
            
            if (statement is ReturnNode returnNode)
            {
                // Если встретили return - вычисляем значение и выходим
                returnValue = EvaluateExpression(returnNode.ReturnValue).ToString();
                break;
            }

            // Иначе просто выполняем statement
            ExecuteStatement(statement);
        }
        
        return returnValue;
    }

    /// <summary>
    /// ВЫПОЛНЯЕТ ДЕЙСТВИЕ (statement) - не возвращает значение
    /// Примеры: $x = 5, Term.print("Hi"), if $x == 5: ...
    /// </summary>
    private void ExecuteStatement(ASTNode statement)
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
                ExecuteIf(ifNode);
                break;
            
            case WhileNode whileNode:
                ExecuteWhile(whileNode);
                break;
            
            default:
                throw new Exception($"Unknown statement type: {statement.GetType()}");
        }
    }
    
    /// <summary>
    /// Выполняет while-блок
    /// </summary>
    private void ExecuteWhile(WhileNode whileNode)
    {
        // 1. Вычисляем условие (expression -> bool)
        bool condition = (bool)EvaluateExpression(whileNode.Condition);

        // 2. Выполняем нужный блок
        bool isBreak = false;
        while (condition)
        {
            foreach (ASTNode statement in whileNode.Body)
            {
                if (_break)
                    return;

                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    return;
                }
                
                ExecuteStatement(statement);
            }
            
            condition = (bool)EvaluateExpression(whileNode.Condition);
        }
    }

    /// <summary>
    /// Выполняет if-блок
    /// </summary>
    private void ExecuteIf(IfNode ifNode)
    {
        // 1. Вычисляем условие (expression -> bool)
        bool condition = (bool)EvaluateExpression(ifNode.Condition);

        // 2. Выполняем нужный блок
        bool isBreak = false;
        if (condition)
        {
            foreach (ASTNode statement in ifNode.ThenBlock)
            {
                if (_break)
                    return;

                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    return;
                }
                
                ExecuteStatement(statement);
            }
        }
        else if (ifNode.ElseBlock.Count > 0)
        {
            foreach (ASTNode statement in ifNode.ElseBlock)
            {
                if (_break)
                    return;
                
                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    return;
                }
                
                ExecuteStatement(statement);
            }
        }
    }

    /// <summary>
    /// Выполняет вызов метода или функции
    /// </summary>
    private string ExecuteMethodCall(MethodCallNode call)
    {
        // 1. ПОЛЬЗОВАТЕЛЬСКИЕ ФУНКЦИИ: this.myFunc(...)
        if (call.ObjectName == "this" && _functions.TryGetValue(call.MethodName, out FunctionNode? func))
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
            Function? function = qClass.GetFunctions().FirstOrDefault(fn => fn.GetName() == call.MethodName);
                
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
            BinaryOperationNode binOp => EvaluateBinaryOperation(binOp),
            
            // Вызов функции/метода: ask("Name?")
            MethodCallNode methodCall => ExecuteMethodCall(methodCall),
            
            _ => throw new Exception($"Unknown expression type: {expr.GetType()}")
        };
    }

    /// <summary>
    /// Вычисляет бинарные операторы: +, -, *, /, ==
    /// </summary>
    private object EvaluateBinaryOperation(BinaryOperationNode binOp)
    {
        // 1. Вычисляем обе стороны
        object left = EvaluateExpression(binOp.Left);
        object right = EvaluateExpression(binOp.Right);

        // 2. Конкатенация строк (если хотя бы одна сторона - строка)
        if (binOp.Operator == "Plus" && (left is string || right is string))
            return left.ToString() + right;
        
        // 3. Обычные операторы
        return binOp.Operator switch
        {
            "==" => Equals(left, right),           // Сравнение
            "!=" => !Equals(left, right),
            "Less" => double.Parse(left.ToString()) < double.Parse(right.ToString()),
            "<=" => double.Parse(left.ToString()) <= double.Parse(right.ToString()),
            "Greater" => double.Parse(left.ToString()) > double.Parse(right.ToString()),
            ">=" => double.Parse(left.ToString()) >= double.Parse(right.ToString()),
            "Plus" => (double)left + (double)right,   // Сложение
            "Minus" => (double)left - (double)right,  // Вычитание
            "Star" => (double)left * (double)right,   // Умножение
            "Slash" => (double)left / (double)right,  // Деление
            _ => throw new Exception($"Unknown operator: {binOp.Operator}")
        };
    }
}