using Qlang.Core.Lang.AST;
using Qlang.Core.Lang.Compiler;
using Qlang.Core.Lang.Dynamic;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.LangDebug;

namespace Qlang.Core.Lang.Interpreter;

public partial class Interpreter
{
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
                Logger.Log("Detected function pointer: " + fn.Name);
                var args = fn.Arguments.ConvertAll(EvaluateExpression);
                DynamicFunction? function;
                
                // If previous object is DynamicClass
                // Ex.: Console.clear()
                if (lastReturnValue is DynamicClass @class)
                {
                    function = TryGetFunctionFromClass(fn.Name, @class);

                    if (function is not null)
                    {
                        if (function.IsPrivate)
                            throw new QlangRuntimeException(
                                $"Can't call private function '{function.Name}' from '{@class.ClassName}'", fn,
                                GetStackTrace());
                        
                        Logger.Log("Detected as function from lastReturnValue");
                        
                        // Create new instance or just call
                        return function.Name == "new" 
                            ? 
                            GetNewClass(@class, args) 
                            : 
                            ExecuteFunction(function, args, @class);
                    }

                    if (fn.Name == "new")
                        return GetNewClass(@class, args);
                }
                
                // Local function without class
                // Ex.: func()
                if (_functions.TryGetValue(fn.Name, out var func) && lastReturnValue is null)
                {
                    Logger.Log("Detected as global function without class");
                    return ExecuteFunction(func, args, null);
                }
                
                // Call from class context
                // Ex.: func() with context ClassExample
                function = TryGetFunctionFromClassContext(fn.Name);
                if (function is not null && lastReturnValue is null)
                {
                    Logger.Log("Detected as function from class context");
                    return ExecuteFunction(function, args, CurrentContext.Class);
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

                if (lastReturnValue is DynamicClass dynamicClass &&
                    dynamicClass.Variables.TryGetValue(objCall.Name, out var var))
                {
                    Logger.Log($"Detected as class from temporary (lastReturnValue)");
                    return var?.Value;
                }
                
                Logger.Log($"Detected as variable");
                Logger.Log($"GetVariableParams: {objCall.Name}");
                return GetVariable(new VariableNode { Name = objCall.Name });
        }
        
        foreach (var item in _contextStack)
            Logger.Log($"StackItem: class = '{item.Class?.Name}' method = '{item.Function?.Name}'");
        Logger.Log($"CurrentContext (After call): class = '{CurrentContext.Class?.Name}' method = '{CurrentContext.Function?.Name}'");

        foreach (var dictItem in _dynamicClasses)
            Logger.Warn("DynamicClasses: " + dictItem.Key + " : " + dictItem.Value);
        
        throw new QlangRuntimeException($"Unknown type of object/function: {obj.GetType().Name}", obj, GetStackTrace());
    }

    private DynamicFunction? TryGetFunctionFromClassContext(string functionName)
    {
        if (!HasContext)
            return null;
        
        var currentClass = CurrentContext.Class;

        if (currentClass is null)
            return null;

        var func = currentClass.Body
            .FirstOrDefault(node => node is FunctionNode func && func.Name == functionName) as FunctionNode;
        
        return func is null ? null : ToDynamicFunction(func);
    }
    
    private DynamicFunction? TryGetFunctionFromClass(string functionName, DynamicClass source)
    {
        var func = source.Body
            .FirstOrDefault(node => node is FunctionNode func && func.Name == functionName) as FunctionNode;
        
        return func is null ? null : ToDynamicFunction(func);
    }
}