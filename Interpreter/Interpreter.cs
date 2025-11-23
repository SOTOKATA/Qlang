using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Qlang.AST;
using Qlang.Dependencies;
using Qlang.Dynamic;
using Math = System.Math;

namespace Qlang.Interpreter;

public partial class Interpreter
{
    public Interpreter(Dictionary<string, string> stringDictionary, Dictionary<string, string> numberDictionary)
    {
        _stringDictionary = stringDictionary;
        _numberDictionary = numberDictionary;
    }

    private readonly NativeFunctionRegistry _nativeFunctions = new();
    
    private readonly Dictionary<string, string> _stringDictionary;
    
    private readonly Dictionary<string, string> _numberDictionary;
    
    private readonly Dictionary<string, DynamicFunction> _functions = new();
    
    private readonly Dictionary<string, DynamicClass> _dynamicClasses = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private ASTContext CurrentContext => _contextStack.Count > 0 ? _contextStack.Peek() : new ASTContext();

    public void Execute(ProgramNode program)
    {
        Logger.Logger.SetLoggerPath(Path.Combine("Logs", "Debug", "debug_interpreter.log"));
        Logger.Logger.Warn("----------- Interpreter -----------");
        
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
        catch (QlangRuntimeException ex)
        {
            Logger.Logger.Error("Interpreter error:");
            Logger.Logger.Error(ex.ToString());
            throw;
        }
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

    private bool _break;
    private ReturnNode _return;
    private object ExecuteFunction(DynamicFunction? function, List<object> arguments, DynamicClass? ownerClass)
    {
        if (function is null)
            return null;
        
        Logger.Logger.Log($"'{function.Name}'({string.Join(", ", arguments)})");
        
        var contextClass = ownerClass ?? (_contextStack.Count > 0 ? CurrentContext.Class : null);
        ASTContext newContext = new() { Function = function, Class = contextClass };
        _contextStack.Push(newContext);

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
            RestoreContextStack();
        }
    }

    private void ExecuteStatement(ASTNode statement)
    {
        switch (statement)
        {
            case AssignmentNode assign:
                Logger.Logger.Warn($"Context: class='{CurrentContext.Class?.Name}' function='{CurrentContext.Function?.Name}'");
                AssignmentNode(assign);
                break;

            case MethodCallNode call:
                Logger.Logger.Log("MethodCallNode");
                ExecuteMethodCall(call);
                break;

            case IfNode ifNode:
                Logger.Logger.Log("IfNode");
                ExecuteIf(ifNode);
                break;
            
            case WhileNode whileNode:
                Logger.Logger.Log("WhileNode");
                ExecuteWhile(whileNode);
                break;
            
            case ForNode forNode:
                Logger.Logger.Log("ForNode");
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
        Logger.Logger.Log($"name='{assign.VariableName}' value='{assign.Value}' value(after evaluating)='{value}'", "AssignmentNode");
        
        if (value is DynamicClass dynamicClass)
        {
            Logger.Logger.Log($"Change name old='{dynamicClass.Name}' new='{assign.VariableName}'", "AssignmentNode");
            dynamicClass.Name = assign.VariableName;
        }

        // Context class
        if (CurrentContext.Class != null)
        {
            if (CurrentContext.Class.Variables.TryGetValue(assign.VariableName, out var var))
            {
                if (var.IsConst)
                    throw new QlangRuntimeException($"Can't re-assign const variable '{assign.VariableName}'",
                        assign, GetStackTrace());

                CurrentContext.Class.Variables[assign.VariableName] = new Variable(
                    assign.VariableName,
                    value,
                    assign.IsStatic,
                    assign.IsPrivate,
                    assign.IsConst);
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
                    throw new QlangRuntimeException($"Can't re-assign const variable '{assign.VariableName}'",
                        assign, GetStackTrace());

                CurrentContext.Blocks[i].Variables[assign.VariableName] = new Variable(
                    assign.VariableName, 
                    value, 
                    assign.IsStatic,
                    assign.IsPrivate,
                    assign.IsConst);
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

                CurrentContext.Function.Variables[assign.VariableName] = new Variable(
                    assign.VariableName,
                    value,
                    assign.IsStatic,
                    assign.IsPrivate,
                    assign.IsConst);
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
    

    /// <summary>
    /// Выполняет вызов метода или функции
    /// </summary>
    private object ExecuteMethodCall(MethodCallNode call)
    {
        var args = call.Arguments.ConvertAll(EvaluateExpression).ToArray();
        
        Logger.Logger.Log($"Args count: " + args.Length);
        Logger.Logger.Log($"class = '{call.ObjectName}'; method = '{call.MethodName}'");
        Logger.Logger.Log($"CurrentContext: class = '{CurrentContext.Class?.Name}', method = '{CurrentContext.Function?.Name}'");

        // Create new class instance (non-linked)
        if (_dynamicClasses.TryGetValue(call.ObjectName, out var value) && call.MethodName == "new")
        {
            // CurrentContext.Class = value;
            return GetNewClass(value, args.ToList());
        }

        switch (call)
        {
            case { ObjectName: "", MethodName: "_str" }:
                return ParseString(string.Join("", args.Select(a => a.ToString())));
            case { ObjectName: "", MethodName: "_str_csharp" }:
                return ParseString(string.Join("", args.Select(a => a.ToString())), true);
            case { ObjectName: "", MethodName: "_csharp" }:
            {
                var returnValue = CSharpCall($"{string.Join(",", args)}");
            
                Logger.Logger.Warn("csharp.return_value: " + (returnValue == null ? "null" :
                    returnValue.ToString()));
            
                return returnValue;
            }
            case { ObjectName: "", MethodName: "_native" } when args.Length > 0:
            {
                string name = args[0].ToString();
                
                args = args.Skip(1).ToArray();
                
                Logger.Logger.Log("_native: " + string.Join(", ", args));
                
                var returnValue = _nativeFunctions.Call(name, args);
            
                Logger.Logger.Warn("Native.ReturnValue: " + (returnValue == null ? "null" :
                    returnValue.ToString()));
            
                return returnValue;
            }
        }

        if (call.ObjectName is "this" or "")
        {
            if (CurrentContext.Class?.Body?
                    .FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) is FunctionNode classMethod)
                return ExecuteFunction(ToDynamicFunction(classMethod), args.ToList(), CurrentContext.Class);

            if (_functions.TryGetValue(call.MethodName, out var func))
                return ExecuteFunction(func, args.ToList(), null);
        }

        if (_dynamicClasses.TryGetValue(call.ObjectName, out var classNode))
            return ExecuteMethodCallClass(classNode, call);

        object? variable = null;
        try
        {
            variable = GetVariable(new VariableNode { Name = call.ObjectName });
        }
        catch (Exception e)
        {
            Logger.Logger.Error(e.ToString());
        }

        if (variable is DynamicClass @class)
        {
            Logger.Logger.Log("Object detected as class pointer");
            
            if (@class?.Body.FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) is FunctionNode
                function)
            {
                CurrentContext.Class = @class;
                Logger.Logger.Warn("VariableClass:Class: " + CurrentContext.Class.Name);
                return ExecuteFunction(ToDynamicFunction(function), args.ToList(), CurrentContext.Class);
            }

            Logger.Logger.Error("VariableClass: function is not found");
        }
        
        foreach (var item in _contextStack)
            Logger.Logger.Log($"StackItem: class = '{item.Class?.Name}' method = '{item.Function?.Name}'");
        Logger.Logger.Log($"CurrentContext (After call): class = '{CurrentContext.Class?.Name}' method = '{CurrentContext.Function?.Name}'");

        foreach (var dictItem in _dynamicClasses)
            Logger.Logger.Warn("DynamicClasses: " + dictItem.Key + " : " + dictItem.Value);
        
        throw new QlangRuntimeException($"Unknown object/function: {call.ObjectName}.{call.MethodName}({string.Join(",", call.Arguments)})", call, 
            GetStackTrace());
    }

    private DynamicClass GetNewClass(DynamicClass dynamicClass, List<object> args)
    {
        Logger.Logger.Warn("Is new instance class");
        var functionNode = dynamicClass.Body.FirstOrDefault(node => node is FunctionNode { Name: "new" });

        var dClass = dynamicClass.Clone();
        
        if (functionNode != null)
            ExecuteFunction(ToDynamicFunction(functionNode as FunctionNode), args, dClass);

        return dClass;
    }

    private void RestoreContextStack()
    {
        if (_contextStack.Count > 0)
        {
            var context = _contextStack.Pop();
            Logger.Logger.Warn($"class='{context.Class?.Name}' function='{context.Function?.Name}'", "RestoreContextStack");
        }
    }

    private List<object> GetCollection(List<object>? arg)
    {
        if (arg is null)
            throw new QlangRuntimeException("collection is null", null, GetStackTrace());
        //new List<object>(
        string result = "[";
                
        foreach (var obj in arg)
        {
            if (obj is not string)
                result += obj + ",";
            else
                result += $"\"{obj}\",";
        }
        // Remove ',' )
        result = (arg.Count > 0 ? result[..^1] : result) + "]";
                
        Logger.Logger.Log(result);
        return arg;
    }

    // WARNING: is ChatGPT code
    private static string ParseString(string input, bool csharpString = false)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '\\' && i + 1 < input.Length)
            {
                var next = input[i + 1];
                switch (next)
                {
                    case 'n': sb.Append('\n');
                        break;
                    case 't': sb.Append('\t');
                        break;
                    case '\\': sb.Append('\\');
                        break;
                    case '"': sb.Append('"');
                        break;
                    default:
                        sb.Append('\\').Append(next);
                        break; // оставляем неизвестные
                }

                i++;
            }
            else
                sb.Append(input[i]);
        }

        var processed = sb.ToString();

        // 2) Экранируем кавычки для verbatim-строки (@"" -> двойные "")
        processed = processed.Replace("\"", "\"\"");

        // 3) Возвращаем вербатим-строку, которую можно вставлять в C# код
        return csharpString ? $"@\"{processed}\"" : processed;
        // return $"@\"{processed}\"";
    }

    private object ExecuteMethodCallClass(DynamicClass classNode, MethodCallNode call)
    {
        if (classNode.Body.FirstOrDefault(astNode => astNode is FunctionNode fn && fn.Name == call.MethodName) is not FunctionNode node)
            throw new QlangRuntimeException($"User class detection error: Unknown object/function: {call.ObjectName}.{call.MethodName}", call, GetStackTrace());

        if (node.IsPrivate)
            throw new QlangRuntimeException("This function is private but called from external class", call,
                GetStackTrace());
        
        // var previousClass = CurrentContext?.Class;
    
        // if (_contextStack.Count > 0)
        // {
        //     CurrentContext.Class = classNode;
        //     Logger.Logger.Warn("ExecuteMethodClass:Class: " + CurrentContext.Class.Name);
        // }

        var dynamicFunction = ToDynamicFunction(node);
        
        var args = call.Arguments?.ConvertAll(EvaluateExpression) ?? [];
        return ExecuteFunction(dynamicFunction, args, classNode);
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
            List<object> args = [];
            if (expr is CollectionNode collectionNode)
            {
                args = collectionNode.Collection.ConvertAll(EvaluateExpression);   
                Logger.Logger.Log("Is collection expression: " + args);
            }
            
            return expr switch
            {
                VariableNode varNode => GetVariable(varNode),
                StringRefNode strRef => GetStringRef(strRef),
                NumberRefNode numberRef => GetNumberRef(numberRef),
                BooleanNode booleanNode => booleanNode.Value,
                NumberNode num => num.Value,
                BinaryOperationNode binOp => EvaluateBinaryOperation(binOp),
                MethodCallNode methodCall => ExecuteMethodCall(methodCall),
                CollectionNode => GetCollection(args),
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
            Logger.Logger.Error("Exception from EvaluateExpression");
            Logger.Logger.Error("Expression: " + expr);
            
            if (expr is BinaryOperationNode binOp)
                Logger.Logger.Error($"expr is BinaryOperationNode [{(binOp.Left as VariableNode)?.Name},{binOp
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
    
    private object GetVariable(VariableNode varNode)
    {
        try
        {
            if (_contextStack.Count > 0 && CurrentContext.Blocks.Count > 0)
                for (var i = CurrentContext.Blocks.Count; i > 0; i--)
                {
                    if (!CurrentContext.Blocks[^1].Variables.TryGetValue(varNode.Name, out var var1))
                        continue;
                    
                    return var1.Value;
                }  
            
            
            if (_contextStack.Count > 0 && CurrentContext?.Function != null &&
                CurrentContext.Function.Variables.TryGetValue
                    (varNode.Name, out var var))
                return var.Value;

            if (_contextStack.Count > 0 && CurrentContext?.Class != null && CurrentContext.Class.Variables.TryGetValue
                    (varNode.Name, out var))
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
                Logger.Logger.Log("Current var count: " + CurrentContext.Class.Variables.Count);
                Logger.Logger.Log("Current class name: " + CurrentContext.Class.Name);
                foreach (var var2 in CurrentContext.Class.Variables)
                {
                    object val = var2.Value.Value;
                    string name = var2.Value.Name;
                    Logger.Logger.Log($"\tVariable: '{name}' = '{val}'");
                }
            }
            else Logger.Logger.Error($"Context Class is null");
            
            if (CurrentContext?.Function != null)
            {
                Logger.Logger.Log("Current var count: " + CurrentContext.Function.Variables.Count);
                Logger.Logger.Log("Current function name: " + CurrentContext.Function.Name);
                foreach (var var2 in CurrentContext.Function.Variables)
                {
                    object val = var2.Value.Value;
                    string name = var2.Value.Name;
                    Logger.Logger.Log($"\tVariable: '{name}' = '{val}'");
                }
            }
            else Logger.Logger.Error($"Context Class is null");
        }
        catch (QlangRuntimeException)
        {
            Logger.Logger.Error("GetVariable exception");
            throw;
        }
        catch (Exception ex)
        {
            Logger.Logger.Error(ex.ToString());
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
    
    private string GetNumberRef(NumberRefNode numberRef)
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

    private object EvaluateBinaryOperation(BinaryOperationNode binOp)
    {
        Logger.Logger.Warn("Detected binary operation");
        Logger.Logger.Warn($"Params: {binOp.Left} {binOp.Operator} {binOp.Right}");
        var left = EvaluateExpression(binOp.Left);
        object? right = EvaluateExpression(binOp.Right);
        bool leftBool;
        bool rightBool;
        
        switch (binOp.Operator)
        {
            // Обработка логических операторов с ленивой оценкой
            case "&&":
            {
                left = EvaluateExpression(binOp.Left);
                if (!bool.TryParse(left.ToString(), out leftBool))
                    throw new QlangRuntimeException(
                        $"Type error: Left operand of '&&' must be boolean, got '{left}'",
                        binOp, GetStackTrace());
            
                // Short-circuit: если левая часть false, правую не вычисляем
                if (!leftBool) 
                {
                    Logger.Logger.Warn($"Short-circuit &&: left is false, returning false");
                    return false;
                }
                
                if (!bool.TryParse(right.ToString(), out rightBool))
                    throw new QlangRuntimeException(
                        $"Type error: Right operand of '&&' must be boolean, got '{right}'",
                        binOp, GetStackTrace());
            
                Logger.Logger.Warn($"Operation &&: {leftBool} && {rightBool} = {rightBool}");
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
                    Logger.Logger.Warn("LeftRight: " + binOp.Left.GetTree() + " and " + binOp.Right.GetTree());
                    Logger.Logger.Warn($"Short-circuit ||: left is true, returning true");
                    return true;
                }
                
                Logger.Logger.Warn($"Short-circuit ||: left not is true, continue");
            
                if (!bool.TryParse(right.ToString(), out rightBool))
                    throw new QlangRuntimeException(
                        $"Type error: Right operand of '||' must be boolean, got '{right}'",
                        binOp, GetStackTrace());
            
                Logger.Logger.Warn($"Operation ||: {leftBool} || {rightBool} = {rightBool}");
                return rightBool;
            }
        }

        if (left is null || right is null)
            return null;

        Logger.Logger.Log($"EvaluateBinaryOperation.IsNumeric: ({left})=" + left.ToString()?.IsNumber());
        Logger.Logger.Log($"EvaluateBinaryOperation.IsNumeric: ({right})=" + right.ToString()?.IsNumber());

        if (bool.TryParse(left.ToString(), out leftBool) && bool.TryParse(right.ToString(), out rightBool))
        {
            Logger.Logger.Warn($"IsBooleanOperation: {left}{binOp.Operator}{right}");
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

        if (binOp.Operator == "Plus" && (left.ToString().IsNumber() == false || right.ToString().IsNumber() == false))
            return left.ToString() + right.ToString();


        if (left.ToString().IsNumber() == false || right.ToString().IsNumber() == false)
        {
            if (binOp.Operator is "==" or "!=")
                return binOp.Operator switch
                {
                    "==" => Equals(left.ToString(), right.ToString()),
                    "!=" => !Equals(left.ToString(), right.ToString()),
                };
            
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
            Logger.Logger.Warn($"Operation: {left}{binOp.Operator}{right}");
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
    
   /* private object EvaluateBinaryOperation(BinaryOperationNode binOp)
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
            Logger.Logger.Warn($"Operation: {left}{binOp.Operator}{right}");
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
    }*/

    private object CSharpCall(string call)
    {
        //
        // var type = Type.GetType(className);
        // var method = type?.GetMethod(methodName, [typeof(string)]);
        //
        // return method?.Invoke(null, args ?? []);
        call = call.Replace("\\\"", "\"");
        Logger.Logger.Warn("C#_Script: " + call);
        
        try
        {
            return CSharpCaller.Call(call);
            // return Task.FromResult(CSharpScript.EvaluateAsync(call).Result);
        }
        catch (AggregateException ex)
        {
            var inner = ex.InnerException;

            if (inner is CompilationErrorException cex)
            {
                Logger.Logger.Error("CompilationErrorException:");
                throw new Exception(string.Join("\n", cex.Diagnostics));
            }

            Logger.Logger.Error("AggregateException:");
            throw new QlangRuntimeException(inner?.Message ?? ex.Message, null, GetStackTrace());
        }
        catch (CompilationErrorException cex)
        {
            Logger.Logger.Error("CompilationErrorException:");
            throw new QlangRuntimeException(string.Join("\n", cex.Diagnostics), null, GetStackTrace());
        }
    }
    
    
    
}