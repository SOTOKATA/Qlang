using Microsoft.CodeAnalysis.CSharp.Scripting;
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

    private readonly Dictionary<string, string> _stringDictionary;
    
    private readonly Dictionary<string, string> _numberDictionary;
    
    private readonly Dictionary<string, Variable> _variables = new();
    
    private readonly Dictionary<string, DynamicFunction> _functions = new();
    
    private readonly Dictionary<string, DynamicClass> _dynamicClasses = new();

    private readonly Stack<ASTContext> _contextStack = new();
    private ASTContext CurrentContext => _contextStack.Count > 0 ? _contextStack.Peek() : new ASTContext();
    private ASTContext PreviousCurrentContext => _contextStack.Count > 0 ? _contextStack.ToArray()[^2] : new ASTContext();
    
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
        
            ExecuteFunction(mainFunction, []);
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
                    .IsPrivate);
        
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
                node.IsPrivate);
            
            dynamicFunction.Parameters.Add(node.VariableName);
        }
        
        dynamicFunction.Body = functionNode.Body;
        dynamicFunction.IsStatic = functionNode.IsStatic;
        dynamicFunction.IsPrivate = functionNode.IsPrivate;
        
        return dynamicFunction;
    }

    private bool _break;
    private ReturnNode _return;
    private object ExecuteFunction(DynamicFunction? function, List<object> arguments)
    {
        if (function is null)
            return null;
        
        var node = _contextStack.Count > 0 ? CurrentContext.Class : new DynamicClass("");
        ASTContext newContext = new() { Function = function, Class = node };
        _contextStack.Push(newContext);

        try
        {
            // if (arguments.Count == function.Variables.Count)
            //     for (var i = 0; i < function.Variables.Count; i++)
            //         _variables[function.Parameters[i]] = arguments[i];

            if (arguments.Count == function.Variables.Count)
                for (var i = 0; i < function.Variables.Count; i++)
                    function.Variables[function.Parameters[i]] = new Variable(
                        function.Parameters[i], 
                        arguments[i],
                        function.IsStatic,
                        false);
            
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
                CurrentContext.Function.Variables[assign.VariableName] = new Variable(
                    assign.VariableName, 
                    value, 
                    assign.IsStatic,
                    assign.IsPrivate);
                // _variables[assign.VariableName] = value;
                break;

            case MethodCallNode call:
                Logger.Logger.Log("Interpreter: Execute statement (MethodCallNode)");
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
        var args = call.Arguments.ConvertAll(EvaluateExpression).ToArray();
        
        Logger.Logger.Log($"Interpreter.ExecuteMethodCall: class = '{call.ObjectName}'; method = '{call.MethodName}'");
        Logger.Logger.Log($"Interpreter.CurrentContext: class = '{CurrentContext.Class?.Name}', method = '{CurrentContext.Function?.Name}'");

        if (_dynamicClasses.TryGetValue(call.ObjectName, out var value) && call.MethodName == "new")
            return value;

        if (call is { ObjectName: "", MethodName: "csharp" })
        {
            //CSharp.Execute(args.FirstOrDefault());
            var returnValue = CSharpCall($"{string.Join(",", args)}");

            returnValue.Wait();
            
            Logger.Logger.Warn("Interpreter.ExecuteMethodCall: csharp.return_value: " + (returnValue == null ? "null" :
                returnValue.Result?.ToString()));
            
            _contextStack.Push(new ASTContext
            {
                Class = null,
                Function = null,
                CurrentNode = call
            });
            
            return returnValue?.Result;
        }
        
        if (call.ObjectName is "this" or "")
        {
            if (CurrentContext.Class?.Body?
                    .FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) is FunctionNode classMethod)
                return ExecuteFunction(ToDynamicFunction(classMethod), args.ToList());

            if (_functions.TryGetValue(call.MethodName, out var func))
                return ExecuteFunction(func, args.ToList());
        }

        if (_dynamicClasses.TryGetValue(call.ObjectName, out var classNode))
            return ExecuteMethodCallClass(classNode, call);

        if (_variables.TryGetValue(call.ObjectName, out var variable))
        {
            var varValue = variable;
            
            Logger.Logger.Log("Interpreter.ExecuteMethodCall: Object detected as class pointer");
            
            var @class = varValue.Value as DynamicClass;

            if (@class?.Body.FirstOrDefault(node => node is FunctionNode fn && fn.Name == call.MethodName) is FunctionNode
                function)
            {
                CurrentContext.Class = @class;
                return ExecuteFunction(ToDynamicFunction(function), args.ToList());
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

        if (node.IsPrivate)
            throw new QlangRuntimeException("This function is private but called from external class", call,
                GetStackTrace());
        
        var previousClass = CurrentContext?.Class;
    
        if (_contextStack.Count > 0)
        {
            
            
            // CurrentContext.Class = classNode;
        }
        
        _contextStack.Push(new ASTContext
        {
            Class = classNode,
            Function = ToDynamicFunction(node),
            CurrentNode = call
        });
        Logger.Logger.Succ("ExecuteMethodCallClass.ContextClass: " + classNode.Name);

        try
        {
            var args = call.Arguments?.ConvertAll(EvaluateExpression) ?? [];
            return ExecuteFunction(ToDynamicFunction(node), args);
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
                NumberRefNode numberRef => GetNumberRef(numberRef),
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
            Logger.Logger.Error("Exception from EvaluateExpression");
            Logger.Logger.Error("Expression: " + expr);
            
            if (expr is BinaryOperationNode binOp)
                Logger.Logger.Error($"expr is BinaryOperationNode [{(binOp.Left as VariableNode)?.Name},{binOp
                .Operator},{(binOp.Right as StringRefNode)?.Index}]");
            
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
        try
        {
            if (_contextStack.Count > 0 && CurrentContext?.Function != null &&
                CurrentContext.Function.Variables.TryGetValue
                    (varNode.Name, out var var))
                return var.Value;

            if (_contextStack.Count > 0 && CurrentContext?.Class != null && CurrentContext.Class.Variables.TryGetValue
                    (varNode.Name, out var))
                return var.Value;
            
            if (_dynamicClasses.TryGetValue(varNode.ClassName, out var dynamicClass) &&
                dynamicClass.Variables.TryGetValue(varNode.Name, out var))
            {
                if (var.IsPrivate)
                    throw new QlangRuntimeException("Scope can't be call from external class " +
                                                    "because of is private scope" + 
                                                    $" (class: {dynamicClass.Name}, scope: {var.Name})", 
                        varNode, GetStackTrace());
                
                Logger.Logger.Error($"Is not private variable ({var.IsPrivate})");
                return var.Value;
            }

            if (_variables.TryGetValue(varNode.Name, out var))
                return var.Value;

            if (CurrentContext?.Class != null)
            {
                Logger.Logger.Log("Current var count: " + CurrentContext.Class.Variables.Count);
                Logger.Logger.Log("Current class name: " + CurrentContext.Class.Name);
                foreach (var var2 in CurrentContext.Class.Variables)
                {
                    object val = var2.Value.Value;
                    string name = var2.Value.Name;
                    Logger.Logger.Log($"Variable: {name} {val}");
                }
            } else Logger.Logger.Log($"Context Class is null");
        }
        catch (QlangRuntimeException)
        {
            Logger.Logger.Error("GetVariable exception");
            throw;
        }

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
    
    private string GetNumberRef(NumberRefNode numberRef)
    {
        if (!_numberDictionary.TryGetValue($"___NUMBER_{numberRef.Index}___", out var value))
        {
            throw new QlangRuntimeException(
                $"Undefined number reference: {value}", 
                numberRef, 
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
    }

    private Task<object> CSharpCall(string call)
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
            return Task.FromResult(CSharpScript.EvaluateAsync(call).Result);
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