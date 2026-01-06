using Core;
using Core.AST;
using Core.Debug;
using Core.Dynamic;
using Core.Exceptions;

namespace Interpreter;

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
                    return Interpreter.ParseString(string.Join("", args.Select(a => a is null ? "" : a.ToString())));
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

        object? returnVal = null;
        for (var i = 0; i < call.Objects.Count; i++)
            returnVal = ExecuteObjectCall(call.Objects[i], returnVal, i == 0);
        return returnVal;
    }

    private object? ExecuteObjectCall(ASTNode obj, object? lastReturnValue, bool isFirstCall = false)
    {
        switch (obj)
        {
            case NamespacePointerNode namespacePointer:
            {
                if (!isFirstCall)
                    throw new QlangRuntimeException("Namespace is not found in current context", namespacePointer,
                        GetStackTrace());

                return _dynamicNamespaces.FirstOrDefault(@namespace => @namespace.Key == namespacePointer.Name).Value;
            }
            case FunctionPointerNode fn:
            {
                Logger.Log("Detected function pointer: " + fn.Name);
             
                switch (lastReturnValue)
                {
                    case string when !isFirstCall:
                    {
                        var str = lastReturnValue;
                        lastReturnValue = _dynamicClasses["String"];
                        (lastReturnValue as DynamicClass).Variables["_value"].Value = str;
                        break;
                    }
                    case List<object?> when !isFirstCall:
                    {
                        var arr = lastReturnValue;
                        lastReturnValue = _dynamicClasses["Array"];
                        (lastReturnValue as DynamicClass).Variables["_value"].Value = arr;
                        break;
                    }
                }
                
                var args = fn.Arguments.ConvertAll(EvaluateExpression);
                
                // If previous object is DynamicClass
                // Ex.: Console.clear()
                if (lastReturnValue is DynamicClass @class && !isFirstCall)
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
                            ExecuteFunction(fromClass.function, fromClass.args, @class, null);
                    }

                    if (fn.Name == "new")
                        return GetNewClass(@class, args);
                }
                
                if (lastReturnValue is DynamicClass dynamicClass && !isFirstCall &&
                    dynamicClass.Variables.TryGetValue(fn.Name, out var variable))
                {
                    Logger.Log($"Detected as class from temporary (lastReturnValue)");
                    if (variable?.Value is FunctionNode fnNode)
                    {
                        args = fn.Arguments.ConvertAll(EvaluateExpression);
                        return ExecuteFunction(ToDynamicFunction(fnNode), args, null, null);
                    }
                }

                if (lastReturnValue is DynamicNamespace @namespace)
                {
                    var fromNamespace = TryGetFunctionFromNamespace(@namespace, fn.Name, args);

                    if (fromNamespace.function is not null)
                    {
                        if (fromNamespace.function.IsPrivate)
                            throw new QlangRuntimeException(
                                $"Cannot call private function '{fromNamespace.function.Name}' from namespace '{@namespace.Name}'", fn,
                                GetStackTrace());
                        
                        return ExecuteFunction(fromNamespace.function, fromNamespace.args ?? [], null, @namespace);
                    }
                }

                if (!isFirstCall)
                    throw new QlangRuntimeException("Function is not found in current context", fn, GetStackTrace());
                
                // Call from class context
                // Ex.: func() with context ClassExample
                var fromClassFn = TryGetFunctionFromClassContext(fn.Name, args);
                if (fromClassFn.function is not null && lastReturnValue is null)
                {
                    Logger.Log("Detected as function from class context");
                    return ExecuteFunction(fromClassFn.function, fromClassFn.args, CurrentContext.Class, null);
                }
                
                // Call from class context
                // Ex.: func() with context ClassExample
                var fromNamespaceFn = TryGetFunctionFromNamespace(CurrentContext.Namespace, fn.Name, args);
                if (fromNamespaceFn.function is not null && lastReturnValue is null)
                {
                    Logger.Log("Detected as function from namespace context");
                    return ExecuteFunction(fromNamespaceFn.function, fromNamespaceFn.args, null, CurrentContext.Namespace);
                }
                
                // Local function without class
                // Ex.: func()
                var fromList = GetFunctionFromFunctionList(fn.Name, args);
                if (fromList.function is not null && lastReturnValue is null)
                {
                    Logger.Log("Detected as global function without class");
                    return ExecuteFunction(ToDynamicFunction(fromList.function), fromList.args, null, null);
                }

                if (GetVariableValue(new VariableNode { Name = fn.Name }) is FunctionNode varFnNode)
                {
                    args = fn.Arguments.ConvertAll(EvaluateExpression);
                    return ExecuteFunction(ToDynamicFunction(varFnNode), args, null, null); 
                }
                
                throw new QlangRuntimeException("Unknown function: " + fn.Name, fn, GetStackTrace());
            }
            case ObjectPointerNode objCall:
                if (lastReturnValue is string && !isFirstCall)
                {
                    var str = lastReturnValue;
                    lastReturnValue = _dynamicClasses["String"];
                    (lastReturnValue as DynamicClass).Variables["_value"].Value = str;
                }
                else if (lastReturnValue is List<object?> && !isFirstCall)
                {
                    var arr = lastReturnValue;
                    lastReturnValue = _dynamicClasses["Array"];
                    (lastReturnValue as DynamicClass).Variables["_value"].Value = arr;
                }
                
                Logger.Log($"Detected object pointer: {objCall.Name}");

                if (lastReturnValue is DynamicClass dClass && !isFirstCall &&
                    dClass.Variables.TryGetValue(objCall.Name, out var var))
                {
                    Logger.Log($"Detected as class from temporary (lastReturnValue)");
                    return var.Value;
                }
                
                if (lastReturnValue is DynamicNamespace dNamespace)
                {
                    Logger.Log($"Detected as namespace from temporary (lastReturnValue)");
                    if (dNamespace.Variables.TryGetValue(objCall.Name, out var))
                        return var.Value;
                    
                    var @class = dNamespace.Classes.FirstOrDefault(c => c.ClassName == objCall.Name);

                    if (@class is not null)
                        return @class;
                }
                
                if (objCall.Name == Keywords.ThisKeyword && HasContext && isFirstCall)
                    return CurrentContext?.Class;

                if (!isFirstCall)
                    throw new QlangRuntimeException($"Class '{objCall.Name}' is not found in current context", obj, GetStackTrace());
                
                if (_dynamicClasses.TryGetValue(objCall.Name, out var classNode))
                {
                    Logger.Log($"Detected as static class");
                    return classNode;
                }
                
                if (CurrentContext.Namespace is not null)
                {
                    var @class = CurrentContext.Namespace.Classes.FirstOrDefault(@class => @class.ClassName == objCall.Name);

                    if (@class is not null)
                        return @class;
                    
                    var = CurrentContext.Namespace.Variables.FirstOrDefault(@class => @class.Key == objCall.Name).Value;

                    if (var is not null)
                        return var;
                }
                
                Logger.Log($"Detected as variable");
                Logger.Log($"GetVariableParams: {objCall.Name}");
                return GetVariableValue(new VariableNode { Name = objCall.Name });
            default:
                return EvaluateExpression(obj);
        }
        // foreach (var item in _contextStack)
        //     Logger.Log($"StackItem: class = '{item.Class?.Name}' method = '{item.Function?.Name}'");
        // Logger.Log($"CurrentContext (After call): class = '{CurrentContext.Class?.Name}' method = '{CurrentContext.Function?.Name}'");
        //
        // foreach (var dictItem in _dynamicClasses)
        //     Logger.Warn("DynamicClasses: " + dictItem.Key + " : " + dictItem.Value);
        //
        // throw new QlangRuntimeException($"Unknown type of object/function: {obj.GetType().Name}", obj, GetStackTrace());
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromNamespace(DynamicNamespace source, string functionName, List<object?>? args)
    {
        var func = GetFunctionFromNamespace(source, functionName, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function), finalArgs: func.Args);
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