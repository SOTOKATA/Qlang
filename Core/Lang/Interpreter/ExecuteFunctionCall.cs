using Qlang.Core.Lang.AST;
using Qlang.Core.Lang.Compiler;
using Qlang.Core.Lang.Dynamic;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.LangDebug;

namespace Qlang.Core.Lang.Interpreter;

public partial class Interpreter
{
    private static string? Typeof(object? arg)
    {
        if (arg is DynamicClass @class)
            return @class.ClassName;
        
        if (arg is List<object?>)
            return "Collection";

        if (arg is float or double or int or long or decimal)
            return "Number";
        
        var type = arg?.GetType().Name;
        
        return type;
    }
    private object? ExecuteObjectCalls(CallNode call)
    {
        Logger.Log($"Objects: " + string.Join(".", call.Objects));
        
        Logger.Log($"CurrentContext: class = '{CurrentContext?.Class?.Name}', function = '{CurrentContext?.Function?.Name}'");

        // overriding system calls
        if (call.Objects.Count > 0 && call.Objects[0] is FunctionPointerNode fn)
        {
            var args = fn.Arguments.ConvertAll(EvaluateExpression).ToArray();
            
            switch (fn.Name)
            {
                case "_str":
                    return ParseString(string.Join("", args.Select(a => a is null ? "" : a.ToString())));
                case "_native":
                    string name = args[0].ToString();
                
                    args = args.Skip(1).ToArray();
                
                    Logger.Log("_native: " + string.Join(", ", args));

                    object? returnValue;
                    try
                    {
                        returnValue = _nativeFunctions.Call(name, args);
                    }
                    catch (Exception ex)
                    {
                        if (ex is QlangRuntimeException)
                            throw new QlangRuntimeException(ex.Message, fn, GetStackTrace(1));
                        throw new QlangRuntimeException(ex.Message, fn, GetStackTrace());
                    }

                    Logger.Warn($"Native call return: value='{returnValue}' type='{returnValue?.GetType().Name}'");
            
                    return returnValue;
                case "typeof":
                    return Typeof(args[0]);
                case "nameof":
                    return args[0]?.ToString();
            }
        }

        return call.Objects.Aggregate<ASTNode?, object?>(null, (current, obj) => ExecuteObjectCall(obj, current));
    }

    private object? ExecuteObjectCall(ASTNode obj, object? lastReturnValue)
    {
        switch (obj)
        {
            case FunctionPointerNode fn:
            {
                if (lastReturnValue is string)
                {
                    var str = lastReturnValue;
                    lastReturnValue = _dynamicClasses["String"];
                    (lastReturnValue as DynamicClass).Variables["_value"].Value = str;
                }
                
                Logger.Log("Detected function pointer: " + fn.Name);
                var args = fn.Arguments.ConvertAll(EvaluateExpression);
                
                // If previous object is DynamicClass
                // Ex.: Console.clear()
                if (lastReturnValue is DynamicClass @class)
                {
                    var fromClass = TryGetFunctionFromClass(@class, fn.Name, args);

                    if (fromClass.function is not null)
                    {
                        if (fromClass.function.IsPrivate)
                            throw new QlangRuntimeException(
                                $"Cannot call private function '{fromClass.function.Name}' from class '{@class.ClassName}'", fn,
                                GetStackTrace());
                        
                        Logger.Log("Detected as function from lastReturnValue");
                        
                        // Create new instance or just call
                        return fromClass.function.Name == "new" 
                            ? 
                            GetNewClass(@class, fromClass.args) 
                            : 
                            ExecuteFunction(fromClass.function, fromClass.args, @class);
                    }

                    if (fn.Name == "new")
                        return GetNewClass(@class, args);
                }
                
                // Local function without class
                // Ex.: func()
                var fromList = GetFunctionFromFunctionList(fn.Name, args);
                if (fromList.function is not null && lastReturnValue is null)
                {
                    Logger.Log("Detected as global function without class");
                    return ExecuteFunction(ToDynamicFunction(fromList.function), fromList.args, null);
                }
                
                // Call from class context
                // Ex.: func() with context ClassExample
                var fromClassFn = TryGetFunctionFromClassContext(fn.Name, args);
                if (fromClassFn.function is not null && lastReturnValue is null)
                {
                    Logger.Log("Detected as function from class context");
                    return ExecuteFunction(fromClassFn.function, fromClassFn.args, CurrentContext.Class);
                }
                
                if (lastReturnValue is DynamicClass dynamicClass &&
                    dynamicClass.Variables.TryGetValue(fn.Name, out var variable))
                {
                    Logger.Log($"Detected as class from temporary (lastReturnValue)");
                    if (variable?.Value is FunctionNode fnNode)
                    {
                        args = fn.Arguments.ConvertAll(EvaluateExpression);
                        return ExecuteFunction(ToDynamicFunction(fnNode), args, null);
                    }
                }

                if (GetVariableValue(new VariableNode { Name = fn.Name }) is FunctionNode varFnNode)
                {
                    args = fn.Arguments.ConvertAll(EvaluateExpression);
                    return ExecuteFunction(ToDynamicFunction(varFnNode), args, null); 
                }
                
                throw new QlangRuntimeException("Unknown function: " + fn.Name, fn, GetStackTrace());
            }
            case ObjectPointerNode objCall:
                Logger.Log($"Detected object pointer: {objCall.Name}");

                if (objCall.Name == Keywords.ThisKeyword && HasContext)
                    return CurrentContext?.Class;
                
                if (_dynamicClasses.TryGetValue(objCall.Name, out var classNode))
                {
                    Logger.Log($"Detected as static class");
                    return classNode;
                }

                if (lastReturnValue is DynamicClass dClass &&
                    dClass.Variables.TryGetValue(objCall.Name, out var var))
                {
                    Logger.Log($"Detected as class from temporary (lastReturnValue)");
                    return var?.Value;
                }
                
                Logger.Log($"Detected as variable");
                Logger.Log($"GetVariableParams: {objCall.Name}");
                return GetVariableValue(new VariableNode { Name = objCall.Name });
            default:
                var evaluated = EvaluateExpression(obj);

                return evaluated;
        }
        
        foreach (var item in _contextStack)
            Logger.Log($"StackItem: class = '{item.Class?.Name}' method = '{item.Function?.Name}'");
        Logger.Log($"CurrentContext (After call): class = '{CurrentContext.Class?.Name}' method = '{CurrentContext.Function?.Name}'");

        foreach (var dictItem in _dynamicClasses)
            Logger.Warn("DynamicClasses: " + dictItem.Key + " : " + dictItem.Value);
        
        throw new QlangRuntimeException($"Unknown type of object/function: {obj.GetType().Name}", obj, GetStackTrace());
    }

    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromClassContext(string functionName, List<object?>? args)
    {
        if (!HasContext)
            return (null, null);
        
        var currentClass = CurrentContext.Class;

        if (currentClass is null)
            return (null, null);

        var func = GetFunctionFromClass(currentClass, functionName, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function), finalArgs: func.Args);
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromClass(DynamicClass source, string functionName, List<object?>? args)
    {
        var func = GetFunctionFromClass(source, functionName, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function), finalArgs: func.Args);
    }
}