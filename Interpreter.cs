using Qlang.AST;
using Qlang.Dependencies.QlangDependencies;
using Qlang.Dependencies.QlangDependencies.Classes;
using Qlang.Dependencies.QlangDependencies.Functions;
using Math = System.Math;

namespace Qlang;

public class Interpreter(Dictionary<string, string> stringDictionary)
{
    private readonly Dictionary<string, object> _variables = new();
    
    private readonly Dictionary<string, FunctionNode> _functions = new();
    
    private readonly Dictionary<string, ClassNode> _classes = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private ASTContext CurrentContext => _contextStack.Count > 0 ? _contextStack.Peek() : new ASTContext();
    
    public void Execute(ProgramNode program)
    {
        try
        {
            // Регистрация функций и классов
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

            // Запуск main()
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
            // Красивый вывод ошибки
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.ToString());
            Console.ResetColor();
            throw;
        }
    }

    /// <summary>
    /// Выполняет пользовательскую функцию с аргументами
    /// </summary>
    ///
    private bool _break;
    private ReturnNode _return;
    private string ExecuteFunction(FunctionNode function, List<object> arguments)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(function.GetTree());
        Console.ResetColor();
    
        // Создаём новый контекст и помещаем в стек
        ASTContext newContext = new() { Function = function };
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
            // ВАЖНО: Всегда восстанавливаем предыдущий контекст
            Console.WriteLine("Restored stack: class=" + _contextStack.Pop().Class?.Name);
        }
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
                throw new QlangRuntimeException($"Unknown statement type: {statement.GetType()}", statement, GetStackTrace());
        }
    }
    
    /// <summary>
    /// Выполняет while-блок
    /// </summary>
    private void ExecuteWhile(WhileNode whileNode)
    {
        // 1. Вычисляем условие (expression -> bool)
        bool condition = whileNode.IsDoWhile || (bool)EvaluateExpression(whileNode.Condition);

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
    Console.WriteLine($"@Info<MethodCallNode>: class={call.ObjectName}, method={call.MethodName}");
    Console.WriteLine($"@Info<ASTContext>: class={CurrentContext.Class?.Name}, method={CurrentContext.Function?.Name}");
    
    // 1. ПОЛЬЗОВАТЕЛЬСКИЕ ФУНКЦИИ: this.myFunc(...) или вызов внутри класса
    if (call.ObjectName is "this" or "")
    {
        // Сначала проверяем, есть ли метод в текущем контексте класса
        if (CurrentContext.Class != null)
        {
            var classMethod = CurrentContext.Class.Body
                .FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) as FunctionNode;
            
            if (classMethod != null)
            {
                List<object> args = call.Arguments.ConvertAll(EvaluateExpression);
                return ExecuteFunction(classMethod, args);
            }
        }
       
        if (_functions.TryGetValue(call.MethodName, out FunctionNode? func))
        {
            List<object> args = call.Arguments.ConvertAll(EvaluateExpression);
            return ExecuteFunction(func, args);
        }
    }
    
    foreach (Class qClass in Namespace.GetClassList())
    {
        if (qClass.GetName() != call.ObjectName) 
            continue;
        
        Function? function = qClass.GetFunctions().FirstOrDefault(fn => fn.GetName() == call.MethodName);
            
        if (function == null)
            throw new QlangRuntimeException($"Method '{call.MethodName}' not found in {call.ObjectName}!", call, 
                GetStackTrace());

        object[] args = call.Arguments.ConvertAll(EvaluateExpression).ToArray();
        return function.Execute(args);
    }

    if (_classes.TryGetValue(call.ObjectName, out ClassNode? classNode))
        return ExecuteMethodCallClass(classNode, call);

    foreach (ASTContext item in _contextStack)
    {
        Console.WriteLine($"StackItem: class='{item.Class?.Name}' method='{item.Function?.Name}'");
    }
    Console.WriteLine($"CurrentStackItem: class='{CurrentContext.Class?.Name}' method='{CurrentContext.Function?.Name}'");
    
    throw new QlangRuntimeException($"Unknown object/function: {call.ObjectName}.{call.MethodName}({string.Join(",", call.Arguments)})", call, 
        GetStackTrace());
}

    private string ExecuteMethodCallClass(ClassNode classNode, MethodCallNode call)
    {
        if (classNode.Body.FirstOrDefault(astNode => astNode is FunctionNode fn && fn.Name == call.MethodName) is not FunctionNode node)
            
            throw new QlangRuntimeException($"User class detection error: Unknown object/function: {call.ObjectName}.{call.MethodName}", call, GetStackTrace());
        
        ClassNode previousClass = CurrentContext.Class;
    
        if (_contextStack.Count > 0)
        {
            CurrentContext.Class = classNode;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("@ContextClass=" + classNode.Name);
            Console.ResetColor();
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
        var divisor = Convert.ToDouble(right);
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
        if (!_variables.TryGetValue(varNode.Name, out var value))
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
        if (!stringDictionary.TryGetValue($"___STRING_{strRef.Index}___", out var value))
        {
            throw new QlangRuntimeException(
                $"Undefined string reference: {value}", 
                strRef, 
                GetStackTrace());
        }
        return value;
    }

    /// <summary>
    /// Вычисляет бинарные операторы: +, -, *, /, ==
    /// </summary>
    private object EvaluateBinaryOperation(BinaryOperationNode binOp)
    {
        object? left = EvaluateExpression(binOp.Left);
        object? right = EvaluateExpression(binOp.Right);

        // Конкатенация строк
        if (binOp.Operator == "Plus" && (left is string || right is string))
            return left.ToString() + right;
    
        // Проверка типов для числовых операций
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
    
    private List<string> GetStackTrace()
    {
        return (from context in _contextStack.Reverse()
            let location = context.CurrentNode != null
                ? $"{context.CurrentNode.SourceFile}:{context.CurrentNode.Line}"
                : "unknown"
            let funcName = context.Function?.Name ?? "global"
            let className = context.Class?.Name
            select className != null
                ? $"at {className}.{funcName} ({location})"
                : $"at {funcName} ({location})").ToList();
    }
}