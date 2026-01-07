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
        switch (arg)
        {
            case DynamicClass @class:
                return @class.ClassName;
            case List<object?>:
                return "Collection";
            case float or double or int or long or decimal:
                return "Number";
            default:
            {
                var type = arg?.GetType().Name;
        
                return type;
            }
        }
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
                    var name = args[0].ToString();
                
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

    private object? ExecuteObjectCall(ASTNode obj, object? lastReturnValue, bool isPathStart = false)
    {
        switch (obj)
        {
            case NamespacePointerNode namespacePointer:
            {
                return FindNamespace(namespacePointer, lastReturnValue, isPathStart);
                
                if (!isPathStart)
                    throw new QlangRuntimeException("Namespace is not found in current context", namespacePointer,
                        GetStackTrace());

                return _dynamicNamespaces.FirstOrDefault(@namespace => @namespace.Key == namespacePointer.Name).Value;
            }
            case FunctionPointerNode fn:
            {
                Logger.Log("Detected function pointer: " + fn.Name);
             
                return FindFunction(fn, lastReturnValue, isPathStart);
                
                switch (lastReturnValue)
                {
                    case string when !isPathStart:
                    {
                        var str = lastReturnValue;
                        lastReturnValue = _dynamicClasses["String"];
                        (lastReturnValue as DynamicClass).Variables["_value"].Value = str;
                        break;
                    }
                    case List<object?> when !isPathStart:
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
                if (lastReturnValue is DynamicClass @class && !isPathStart)
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
                
                if (lastReturnValue is DynamicClass dynamicClass && !isPathStart &&
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

                if (!isPathStart)
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
                if (lastReturnValue is string && !isPathStart)
                {
                    var str = lastReturnValue;
                    lastReturnValue = _dynamicClasses["String"];
                    (lastReturnValue as DynamicClass).Variables["_value"].Value = str;
                }
                else if (lastReturnValue is List<object?> && !isPathStart)
                {
                    var arr = lastReturnValue;
                    lastReturnValue = _dynamicClasses["Array"];
                    (lastReturnValue as DynamicClass).Variables["_value"].Value = arr;
                }
                
                Logger.Log($"Detected object pointer: {objCall.Name}");

                return FindObject(objCall, lastReturnValue, isPathStart);
                
                if (lastReturnValue is DynamicClass dClass && !isPathStart &&
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
                
                if (objCall.Name == Keywords.ThisKeyword && HasContext && isPathStart)
                    return CurrentContext?.Class;

                if (!isPathStart)
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

    private (DynamicFunction? function, List<object?>? args) FindFunction(FunctionPointerNode node, object? lastObject, bool isPathStart)
    {
        var args = node.Arguments.ConvertAll(EvaluateExpression);
        var pair = TryGetFunctionFromClassContext(null, null);
        
        // if is path start like: call().., other..
        if (isPathStart)
        {
            pair = TryGetFunctionFromClassContext(node.Name, args);
            // Try to get function from class context;
            if (pair.function is not null)
                return pair;
            
            // Try to get function from namespace context;
            pair = TryGetFunctionFromNamespaceContext(node.Name, args);
            if (pair.function is not null)
                return pair;
            
            // Try to get function from variable pointers
            var variableFunction = GetVariableValue(new VariableNode { Name = node.Name });
            if (variableFunction is FunctionNode functionNode)
                return (ToDynamicFunction(functionNode), args);
            
            // Try to get function from global list
            var funcPair = GetFunctionFromFunctionList(node.Name, args);
            if (funcPair.function is not null)
                return (ToDynamicFunction(funcPair.function), funcPair.args);
            
            throw new QlangRuntimeException($"Function '{node.Name}' is not found in current context",
                node, GetStackTrace());
        }
        
        // Try to get function from class function list;
        if (lastObject is DynamicClass dClass)
        {
            pair = TryGetFunctionFromClass(dClass, node.Name, args);
            if (pair.function is not null)
                return pair;
        }
        
        // Try to get function from namespace function list;
        if (lastObject is DynamicNamespace dNamespace)
        {
            pair = TryGetFunctionFromNamespace(dNamespace, node.Name, args);
            if (pair.function is not null)
                return pair;
        }
        
        throw new QlangRuntimeException($"Function '{node.Name}' is not found in current context",
            node, GetStackTrace());
    }

    private object? FindObject(ObjectPointerNode node, object? lastObject, bool isPathStart)
    {
        if (isPathStart)
        {
            // Get 'this'
            if (node.Name == "this" && HasContext)
                return CurrentContext.Class;
            
            // Try to get VAR from current (class or namespace or function or blocks) context;
            if (HasContext)
            {
                if (CurrentContext.Class is not null &&
                    CurrentContext.Class.Variables.TryGetValue(node.Name, out var var) || CurrentContext.Namespace is not null &&
                    CurrentContext.Namespace.Variables.TryGetValue(node.Name, out var) || 
                    (CurrentContext.Function is not null && CurrentContext.Function.Variables.TryGetValue(node.Name, out var)) || 
                    CurrentContext.Blocks.FirstOrDefault(block => block.Variables.ContainsKey(node.Name)).Variables.TryGetValue(node.Name, out var))
                    return var.Value;
            }

            // Try to get VAR from global variable list;
            var variable = _globalVariables.FirstOrDefault(var => var.Name == node.Name);
            if (variable is not null)
                return variable.Value;
            
            // Try to get CLASS from context namespace
            if (HasContext)
            {
                var @class = CurrentContext.Namespace.Classes.FirstOrDefault(@class => @class.ClassName == node.Name);
                if (@class is not null)
                    return @class;
            }
            
            // Try to get CLASS from global class list;
            if (_dynamicClasses.TryGetValue(node.Name, out var dynamicClass))
                return dynamicClass;
                
            throw new QlangRuntimeException($"Object '{node.Name}' is not found in current context",
                node, GetStackTrace());
        }

        switch (lastObject)
        {
            case DynamicClass dynamicClass:
                // Try to get VAR from class
                if (dynamicClass.Variables.TryGetValue(node.Name, out var var))
                {
                    if (var.IsPrivate)
                        throw new QlangRuntimeException("Cannot get access to private variable.",
                            node, GetStackTrace());
                        
                    return var.Value;
                }
                break;
            case DynamicNamespace dynamicNamespace:
                // Try to get VAR from namespace
                if (dynamicNamespace.Variables.TryGetValue(node.Name, out var))
                {
                    if (var.IsPrivate)
                        throw new QlangRuntimeException("Cannot get access to private variable.",
                            node, GetStackTrace());
                    
                    return var.Value;
                }
            
                // Try to get CLASS from namespace
                var @class = dynamicNamespace.Classes.FirstOrDefault(@class => @class.ClassName == node.Name);
                if (@class is not null)
                    return @class;
                break;
        }
        
        throw new QlangRuntimeException($"Object '{node.Name}' is not found in current context",
            node, GetStackTrace());
    }

    private DynamicNamespace FindNamespace(NamespacePointerNode node, object? lastObject, bool isPathStart)
    {
        if (!_dynamicNamespaces.TryGetValue(node.Name, out var @namespace))
            throw new QlangRuntimeException($"Namespace '{node.Name}' is not found in current context", node, GetStackTrace());
        
        return @namespace;
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromNamespace(DynamicNamespace source, string functionName, List<object?>? args)
    {
        var func = GetFunctionFromNamespace(source, functionName, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function), finalArgs: func.Args);
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromNamespaceContext(string functionName, List<object?>? args)
    {
        if (!HasContext)
            return (null, null);
        
        var currentNamespace = CurrentContext.Namespace;

        if (currentNamespace is null)
            return (null, null);

        var func = GetFunctionFromNamespace(currentNamespace, functionName, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function), finalArgs: func.Args);
    }

    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromClassContext(string? functionName, List<object?>? args)
    {
        if (!HasContext || functionName is null)
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