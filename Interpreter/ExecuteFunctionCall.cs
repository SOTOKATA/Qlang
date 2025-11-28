using System.Reflection.Emit;
using Qlang.AST;
using Qlang.Dynamic;

namespace Qlang.Interpreter;

public partial class Interpreter
{
    public object? ExecuteObjectCalls(CallNode call)
    {
        Logger.Logger.Log($"Objects: " + string.Join(".", call.Objects));
        Logger.Logger.Log($"CurrentContext: class = '{CurrentContext.Class?.Name}', function = '{CurrentContext.Function?.Name}'");

        // overriding system calls
        if (call.Objects.Count > 0 && call.Objects[0] is FunctionPointerNode fn)
        {
            var args = fn.Arguments.ConvertAll(EvaluateExpression).ToArray();
            switch (fn.Name)
            {
                case "_str":
                    Console.WriteLine($"[{string.Join(" ", args)}]");
                    return ParseString(string.Join("", args.Select(a => a.ToString())));
                case "_native":
                    string name = args[0].ToString();
                
                    args = args.Skip(1).ToArray();
                
                    Logger.Logger.Log("_native: " + string.Join(", ", args));
                
                    var returnValue = _nativeFunctions.Call(name, args);
            
                    Logger.Logger.Warn($"Native call return: value='{returnValue}' type='{returnValue?.GetType().Name}'");
            
                    return returnValue;
            }
        }
        
        object? lastReturnValue = null;
        foreach (var obj in call.Objects)
        {
            lastReturnValue = ExecuteObjectCall(obj, lastReturnValue);
        }

        return lastReturnValue;
    }

    private object ExecuteObjectCall(ASTNode obj, object? lastReturnValue)
    {
        switch (obj)
        {
            case FunctionPointerNode fn:
            {
                var args = fn.Arguments.ConvertAll(EvaluateExpression);
                DynamicFunction? function;
                
                // If previous object is DynamicClass
                // Ex.: Console.clear()
                if (lastReturnValue is DynamicClass @class)
                {
                    function = TryGetFunctionFromClass(fn.Name, @class);

                    if (function is not null)
                    {
                        // Create new instance or just call
                        return function.Name == "new" 
                            ? 
                            GetNewClass(@class, args) 
                            : 
                            ExecuteFunction(function, args, @class);
                    }
                }
                
                // Local function without class
                // Ex.: func()
                if (_functions.TryGetValue(fn.Name, out var func) && lastReturnValue is null)
                    ExecuteFunction(func, args, null);
                
                // Call from class context
                // Ex.: func() with context ClassExample
                function = TryGetFunctionFromClassContext(fn.Name);
                if (function is not null && lastReturnValue is null)
                    return ExecuteFunction(function, args, CurrentContext.Class);
                break;
            }
            case ObjectPointerNode objCall:
                foreach (var VARIABLE in _dynamicClasses)
                {
                    Logger.Logger.Warn($"VARIABLE: {VARIABLE.Key} responsible {VARIABLE.Value.Name}");
                }
                
                if (_dynamicClasses.TryGetValue(objCall.Name, out var classNode))
                    return classNode;
                
                if (lastReturnValue is DynamicClass dynamicClass &&
                    dynamicClass.Variables.TryGetValue(objCall.Name, out var var))
                    return var?.Value;
                
                return GetVariable(new VariableNode { Name = objCall.Name });
                        
        }
        
        foreach (var item in _contextStack)
            Logger.Logger.Log($"StackItem: class = '{item.Class?.Name}' method = '{item.Function?.Name}'");
        Logger.Logger.Log($"CurrentContext (After call): class = '{CurrentContext.Class?.Name}' method = '{CurrentContext.Function?.Name}'");

        foreach (var dictItem in _dynamicClasses)
            Logger.Logger.Warn("DynamicClasses: " + dictItem.Key + " : " + dictItem.Value);
        
        throw new QlangRuntimeException($"Unknown type of object/function: {obj.GetType().Name}", obj, GetStackTrace());
    }

    private DynamicFunction? TryGetFunctionFromClassContext(string functionName)
    {
        if (_contextStack.Count == 0)
            return null;
        
        var currentClass = CurrentContext.Class;

        if (currentClass is null)
            return null;
        
        return ToDynamicFunction(currentClass.Body
            .FirstOrDefault(node => node is FunctionNode func && func.Name == functionName) as FunctionNode);
    }
    
    private DynamicFunction? TryGetFunctionFromClass(string functionName, DynamicClass source)
    {
        return ToDynamicFunction(source.Body
            .FirstOrDefault(node => node is FunctionNode func && func.Name == functionName) as FunctionNode);
    }
}