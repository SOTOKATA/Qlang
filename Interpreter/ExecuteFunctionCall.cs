using Core;
using Core.AST;
using Core.Debug;
using Core.Dynamic;
using Core.Exceptions;

namespace Interpreter;

public partial class Interpreter
{
    private string? Typeof(object? arg)
    {
        switch (arg)
        {
            case DynamicClass @class:
                return @class.ClassName;
            case List<object?>:
                return "Collection";
            case float or double or int or long or decimal:
                return "Number";
            case bool:
                return "Boolean";
            default:
            {
                if (arg is CallNode call)
                {
                    var first = call.Objects.FirstOrDefault();
                    if (first is ObjectPointerNode { Name: "Collection" or "Number" or "Boolean" } obj) 
                        return obj.Name;

                    var result = ExecuteObjectCalls(call);

                    if (result is DynamicClass dynamicClass)
                        return dynamicClass.ClassName;

                    return result?.ToString();
                }

                var type = arg?.GetType().Name;
        
                return type;
            }
        }
    }
    
    private object? ExecuteObjectCalls(CallNode call)
    {
        Logger.Log($"Objects: " + string.Join(".", call.Objects));
        
        if (HasContext)
            Logger.Log($"CurrentContext: class = '{CurrentContext.Class?.Name}', function = '{CurrentContext.Function?.Name}'");

        // overriding system calls
        if (call.Objects.Count > 0 && call.Objects[0] is FunctionPointerNode fn)
        {
            var args = fn.Arguments.ConvertAll(EvaluateExpression).ToArray();
            
            switch (fn.Name)
            {
                case "_str":
                    return ParseString(string.Join("", args.Select(a => a is null ? "" : a.ToString())));
                case "_native":
                    var name = args[0]?.ToString();
                
                    args = args.Skip(1).ToArray();
                
                    Logger.Log("_native: " + string.Join(", ", args));

                    object? returnValue;
                    try
                    {
                        returnValue = _nativeFunctions.Call(name!, args);
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
                var @namespace = FindNamespace(namespacePointer, lastReturnValue, isPathStart);

                if (@namespace is not null && HasContext)
                    CurrentContext.Namespace = @namespace;

                return @namespace;
            }
            case FunctionPointerNode fn:
            {
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
                
                Logger.Log("Detected function pointer: " + fn.Name);
                
                if (fn.Name == Keywords.CreateClassInstanceKeyword && lastReturnValue is DynamicClass @class)
                    return GetNewClass(@class, fn.Arguments.ConvertAll(EvaluateExpression));
             
                var function = FindFunction(fn, lastReturnValue, isPathStart);
                
                // Console.WriteLine("\nCURRENT CLASS: " + (function.@class is null ? "NULL" : function.@class.ClassName));
                // Console.WriteLine("CURRENT NAMES: " + (function.@namespace is null ? "NULL" : function.@namespace.Name));
                // Console.WriteLine("CURRENT FUNCT: " + (function.function is null ? "NULL" : function.function.Name));
                
                return ExecuteFunction(function.function, function.args ?? [], function.@class, function.@namespace);
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
                
                var @object = FindObject(objCall, lastReturnValue, isPathStart);

                // if (@object.@namespace is not null && HasContext)
                // {
                //     CurrentContext.Namespace = @object.@namespace;
                //     Console.WriteLine("Current context namespace is: " + @object.@namespace.Name);
                // }

                // if (@object.@object is DynamicClass dynamicClass && HasContext)
                    // CurrentContext.Class = dynamicClass;
                
                return @object.@object;
            default:
                return EvaluateExpression(obj);
        }
    }

    private (DynamicFunction? function, List<object?>? args, DynamicClass? @class, DynamicNamespace? @namespace) FindFunction(FunctionPointerNode node, object? lastObject, bool isPathStart)
    {
        var args = node.Arguments.ConvertAll(EvaluateExpression);
        (DynamicFunction? function, List<object?>? args) pair;
        
        // if is path start like: call().., other..
        if (isPathStart)
        {
            if (HasContext)
            {
                // Try to get function from class context;
                if (CurrentContext.Class is not null)
                {
                    pair = TryGetFunctionFromClass(CurrentContext.Class, node.Name, args);
                    if (pair.function is not null)
                        return (pair.function, pair.args, CurrentContext.Class, CurrentContext.Namespace);
                }
                
                // Try to get function from namespace context;
                if (HasContext && CurrentContext.Namespace is not null)
                {
                    pair = TryGetFunctionFromNamespace(CurrentContext.Namespace, node.Name, args);
                    if (pair.function is not null)
                        return (pair.function, pair.args, null, CurrentContext.Namespace);
                }
            }
            
            // Try to get function from variable pointers
            object? variableFunction = null;
            // Will send error if variable is not found
            try
            {
                variableFunction = GetVariableValue(new VariableNode { Name = node.Name });
            }
            catch
            {
                // ignored
            }

            if (variableFunction is FunctionNode functionNode)
                return (ToDynamicFunction(functionNode), args, CurrentContext.Class, CurrentContext.Namespace);
            
            // Try to get function from global list
            var funcPair = GetFunctionFromFunctionList(node.Name, args);
            if (funcPair.function is not null)
                return (ToDynamicFunction(funcPair.function), funcPair.args, null, null);
            
            // Try to add namespace keyword to path (get VAR);
            var @namespace = _usingsList.FirstOrDefault(@namespace => @namespace.Functions.Any(function => function.Name == node.Name));

            if (@namespace is not null)
            {
                var function = @namespace.Functions.First(function => function.Name == node.Name);

                if (function.IsPrivate)
                    throw new QlangRuntimeException(
                        $"Cannot get access to function '{function.Name}' from namespace '{@namespace.Name}'", node, GetStackTrace());

                return (ToDynamicFunction(function), args, null, @namespace);
            }
            
            throw new QlangRuntimeException($"Function '{node.Name}' is not found in current context ('{lastObject}')",
                node, GetStackTrace());
        }
        
        // Try to get function from class function list;
        if (lastObject is DynamicClass dClass)
        {
            pair = TryGetFunctionFromClass(dClass, node.Name, args);
            // Find class in namespaces
            var @namespace = _dynamicNamespaces.FirstOrDefault(n => n.Value.Classes.Contains(dClass)).Value;
            
            if (pair.function is not null)
            {
                if (pair.function.IsPrivate)
                    throw new QlangRuntimeException($"Cannot get access to private function '{pair.function.Name}' from class '{dClass.ClassName}'", node, GetStackTrace());
                
                return (pair.function, pair.args, dClass, @namespace);
            }
        }
        
        // Try to get function from namespace function list;
        if (lastObject is DynamicNamespace dNamespace)
        {
            pair = TryGetFunctionFromNamespace(dNamespace, node.Name, args);
            if (pair.function is not null)
            {
                if (pair.function.IsPrivate)
                    throw new QlangRuntimeException($"Cannot get access to private function '{pair.function.Name}' from namespace '{dNamespace.Name}'", node, GetStackTrace());
                
                return (pair.function, pair.args, null, dNamespace);
            }
        }
        
        throw new QlangRuntimeException($"Function '{node.Name}' is not found in current context ('{lastObject}')",
            node, GetStackTrace());
    }

    private (object? @object, DynamicNamespace? @namespace) FindObject(ObjectPointerNode node, object? lastObject, bool isPathStart)
    {
        if (node.Name is null)
            throw new QlangRuntimeException("Part of path is null.", node, GetStackTrace());
            
        if (isPathStart)
        {
            // Get 'this'
            if (node.Name == Keywords.ThisKeyword && HasContext)
                return (CurrentContext.Class, CurrentContext.Namespace);
            
            // Try to get VAR from current (class or namespace or function or blocks) context;
            if (HasContext)
            {
                var block = CurrentContext.Blocks
                    .FirstOrDefault(b => b.Variables.ContainsKey(node.Name));
                
                var classIsNull = CurrentContext.Class is null;
                var functionIsNull = CurrentContext.Function is null;
                var namespaceIsNull = CurrentContext.Namespace is null;

                Variable? var;
                
                // Get from function
                if (!functionIsNull)
                {
                    var = CurrentContext.Function?.Variables.FirstOrDefault(funcVar => funcVar.Key == node.Name).Value;
                    
                    if (var is not null)
                        return (var.Value, CurrentContext.Namespace);
                }

                // Get from class
                if (!classIsNull)
                {
                    var = CurrentContext.Class?.Variables.FirstOrDefault(classVar => classVar.Key == node.Name).Value;
                    
                    if (var is not null)
                        return (var.Value, CurrentContext.Namespace);
                }

                // Get from namespace
                if (!namespaceIsNull)
                {
                    var = CurrentContext.Namespace?.Variables.FirstOrDefault(namespaceVar =>
                        namespaceVar.Key == node.Name).Value;
                    
                    if (var is not null)
                        return (var.Value, CurrentContext.Namespace);
                }
                
                // Get from blocks
                if (block != null && block.Variables.TryGetValue(node.Name, out var v))
                    return (v.Value, CurrentContext.Namespace);
            }

            // Try to get VAR from global variable list;
            var variable = _globalVariables.FirstOrDefault(var => var.Name == node.Name);
            if (variable is not null)
                return (variable.Value, null);
            
            // Try to get CLASS from context namespace
            if (HasContext)
            {
                var @class = CurrentContext.Namespace?.Classes.FirstOrDefault(@class => @class.ClassName == node.Name);
                if (@class is not null)
                    return (@class, CurrentContext.Namespace);
            }
            
            // Try to get CLASS from global class list;
            if (_dynamicClasses.TryGetValue(node.Name, out var dynamicClass))
                return (dynamicClass, null);

            // Try to add namespace keyword to path (get VAR);
            var namespaceVar = _usingsList.FirstOrDefault(@namespace => @namespace.Variables.ContainsKey(node.Name));
            if (namespaceVar is not null && namespaceVar.Variables.TryGetValue(node.Name, out var value))
            {
                if (value.IsPrivate)
                    throw new QlangRuntimeException(
                        $"Cannot get access to variable '{node.Name}' from namespace '{namespaceVar.Name}'", node,
                        GetStackTrace());
                
                return (value.Value, namespaceVar);
            }
            
            // Try to add namespace keyword to path (get CLASS);
            var namespaceClass = _usingsList.FirstOrDefault(@namespace => @namespace.Classes.Any(@class => @class.ClassName == node.Name));
            if (namespaceClass is not null)
                return (namespaceClass.Classes.FirstOrDefault(@class => @class.ClassName == node.Name), namespaceClass);
                
            throw new QlangRuntimeException($"Object '{node.Name}' is not found in current context" + (HasContext ? $"\nCurrent function: '{CurrentContext.Function?.Name}'; Current class: '{CurrentContext.Class?.ClassName}'; Current namespace: '{CurrentContext.Namespace?.Name}'" : "\nNo context found."),
                node, GetStackTrace());
        }

        if (lastObject is null)
        {
            throw new QlangRuntimeException($"Last object is null",
                node, GetStackTrace());
        }
        
        switch (lastObject)
        {
            case DynamicClass dynamicClass:
                // Try to get VAR from class
                if (dynamicClass.Variables.TryGetValue(node.Name, out var var))
                {
                    if (var.IsPrivate)
                        throw new QlangRuntimeException($"Cannot get access to private variable '{var.Name}' from class '{dynamicClass.ClassName}'.",
                            node, GetStackTrace());
                        
                    return (var.Value, null);
                }
                break;
            case DynamicNamespace dynamicNamespace:
                // Try to get VAR from namespace
                if (dynamicNamespace.Variables.TryGetValue(node.Name, out var))
                {
                    if (var.IsPrivate)
                        throw new QlangRuntimeException($"Cannot get access to private variable '{var.Name}' from namespace '{dynamicNamespace.Name}'.",
                            node, GetStackTrace());
                    
                    return (var.Value, null);
                }
            
                // Try to get CLASS from namespace
                var @class = dynamicNamespace.Classes.FirstOrDefault(@class => @class.ClassName == node.Name);
                if (@class is not null)
                    return (@class, null);
                break;
        }
        throw new QlangRuntimeException($"Object '{node.Name}' is not found in current context" + (HasContext ? $"\nCurrent function: '{CurrentContext.Function?.Name}'; Current class: '{CurrentContext.Class?.ClassName}'; Current namespace: '{CurrentContext.Namespace?.Name}'" : "\nNo context found."),
            node, GetStackTrace());
    }

    private DynamicNamespace? FindNamespace(NamespacePointerNode node, object? lastObject, bool isPathStart)
    {
        if (_dynamicNamespaces.TryGetValue(node.Name, out var @namespace))
            return @namespace;

        if (lastObject is DynamicNamespace dynamicNamespace)
        {
            var ns = dynamicNamespace.Namespaces.FirstOrDefault(ns => ns.Name == node.Name);

            if (ns is not null)
                return ns.IsPrivate ? throw new QlangRuntimeException($"Cannot get access to private namespace from namespace '{dynamicNamespace.Name}'", node, GetStackTrace()) : ns;
        }

        // TODO: Add 'namespace' to 'namespace'
        // if (lastObject is DynamicNamespace dynamicNamespace)
        throw new QlangRuntimeException($"Namespace '{node.Name}' is not found in current context", node, GetStackTrace());
        
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromNamespace(DynamicNamespace source, string functionName, List<object?>? args)
    {
        var func = GetFunctionFromNamespace(source, functionName, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function), finalArgs: func.Args);
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromClass(DynamicClass? source, string functionName, List<object?>? args)
    {
        if (source is null)
            throw new QlangRuntimeException("Source is null", null, GetStackTrace());
        
        var func = GetFunctionFromClass(source, functionName, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function), finalArgs: func.Args);
    }
}