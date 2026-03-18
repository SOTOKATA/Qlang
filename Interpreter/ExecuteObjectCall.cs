using Core;
using Core.AST;
using Core.Dynamic;
using Core.Exceptions;

namespace Interpreter;

public partial class Interpreter
{
    private object? PrimitiveToDynamicClass(object? primitive, Stack<ASTContext> stack)
    {
        switch (primitive)
        {
            case null:
                return null;
            case not (string or List<object?> or List<object>):
                return primitive;
        }

        DynamicClass? @class = null;

        switch (primitive)
        {
            case string str:
            {
                var id = _stringPoolTable.Add(QlSystemClasses.StringClassName);
                @class = ToDynamicClass(_namespaces[GlobalNamespaceName].Classes.First(x => x.NameId == id), stack);
                @class.Variables["_value"].Value = str;
                break;
            }
            case List<object?> arrNullable:
            {
                var id = _stringPoolTable.Add(QlSystemClasses.ArrayClassName);
                @class = ToDynamicClass(_namespaces[GlobalNamespaceName].Classes.First(x => x.NameId == id), stack);
                @class.Variables["_value"].Value = arrNullable;
                break;
            }
        }

        return @class;
    }
    
    private string Typeof(object? arg, Stack<ASTContext> stack)
    {
        if (arg is null)
            return "null";
        
        switch (arg)
        {
            case DynamicClass @class:
                if (@class.ClassName == "~object" && @class.Variables.ContainsKey("_objectName"))
                {
                    object? className;

                    if (@class.Variables["_objectName"].Value is string or int or long or double or float or bool)
                        className = @class.Variables["_objectName"].Value;
                    else className = EvaluateExpression((ASTNode)@class.Variables["_objectName"].Value!, stack);

                    return @class.ToString() + ":" + (className?.ToString() ?? "");
                }
                
                return @class.ToString();
            case FunctionNode fn:
                return _stringPoolTable[fn.NameId];
            case List<object?>:
                return "Collection";
            case int or long or double or float:
                return "Number";
            case bool:
                return "Boolean";
            default:
            {
                if (arg is CallNode call)
                {
                    var first = call.Objects.FirstOrDefault();
                    if (first is ObjectPointerNode pointer)
                    {
                        switch (_stringPoolTable[pointer.NameId])
                        {
                            case "Nullable":
                                return Keywords.NullKeyword;
                            case "Collection" or "Number" or "Boolean":
                                return _stringPoolTable[pointer.NameId];
                        }
                    }

                    return ExecutePathToClass(call, stack).ClassName;
                }

                var type = arg.GetType().Name;
        
                return type;
            }
        }
    }
    
    private object? 
        ExecuteObjectCalls(CallNode call, Stack<ASTContext> stack)
    {
        // overriding system calls
        if (call.Objects.Count > 0 && call.Objects[0] is FunctionPointerNode fn)
        {
            var args = fn.Arguments.ConvertAll(x => EvaluateExpression(x, stack)).ToArray();
            
            switch (_stringPoolTable[fn.NameId])
            {
                case "_str" when args.Length == 1:
                    args[0] = PrimitiveToDynamicClass(args[0], stack);
                    args[0] = args[0] is DynamicClass { ClassName: "String" } dc
                        ? dc.Variables["_value"]
                        : args[0];
                    
                    return ParseString(args[0]!.ToString());
                case "_native":
                    if (args.Length < 3)
                        throw new QlangRuntimeException(
                            """
                            Undefined call structure of _native system command.
                            Structure is _native(\"<namespace>\", \"<class>\", \"<function>)\", <args>
                            """, GetCurrentDebug(stack), GetStackTrace(stack));
                    
                    var @namespace = args[0]?.ToString();
                    var @class = args[1]?.ToString();
                    var function = args[2]?.ToString();
                
                    args = args.Skip(3).ToArray();
                
                    object? returnValue;
                    try
                    {
                        returnValue = _nativeFunctions.Call($"{@namespace}.{@class}.{function}", args);
                    }
                    catch (Exception ex)
                    {
                        switch (ex)
                        {
                            case QlangRuntimeException:
                                throw new QlangRuntimeException(ex.Message, GetCurrentDebug(stack), GetStackTrace(stack, 1));
                            case QlangProgramException exp:
                            {
                                var debug = exp.WriteStackTrace ? GetCurrentDebug(stack) : (-1, "undefined");
                                var stackTrace = exp.WriteStackTrace ? GetStackTrace(stack, 1) : [];
                                throw new QlangRuntimeException(ex.Message, debug, stackTrace);
                            }
                            default:
                                throw new QlangRuntimeException(ex.Message, GetCurrentDebug(stack), GetStackTrace(stack));
                        }
                    }

                    return returnValue;
                case "typeof":
                    return Typeof(args[0], stack);
                case "nameof":
                    return args[0]?.ToString();
            }
        }

        object? returnVal = null;
        for (var i = 0; i < call.Objects.Count; i++)
            returnVal = ExecuteObjectCall(call.Objects[i], returnVal, stack, i == 0);
        return returnVal;
    }

    private object? ExecuteObjectCall(ASTNode obj, object? lastReturnValue, Stack<ASTContext> stack, bool isPathStart = false)
    {
        switch (obj)
        {
            case NamespacePointerNode namespacePointer:
            {
                var @namespace = FindNamespace(namespacePointer, lastReturnValue, isPathStart, stack);

                return @namespace;
            }
            case FunctionPointerNode fn:
            {
                if (!isPathStart)
                    lastReturnValue = PrimitiveToDynamicClass(lastReturnValue, stack);
                
                var function = FindFunction(fn, lastReturnValue, isPathStart, stack);
                
                CurrentContext(stack)!.AllowPrivateCall = false;

                if (function.function is null && _stringPoolTable[fn.NameId] == "toString")
                    return lastReturnValue?.ToString() ?? null;
                
                return ExecuteFunction(function.function, function.args ?? [], function.@class, function.@namespace, stack);
            }
            case ObjectPointerNode objCall:
                if (!isPathStart)
                    lastReturnValue = PrimitiveToDynamicClass(lastReturnValue, stack);
                
                var objectAndNamespace = FindObject(objCall, lastReturnValue, isPathStart, stack);

                if (objectAndNamespace.@object is Variable var)
                    objectAndNamespace.@object = var.Value;
                
                return objectAndNamespace.@object;
            default:
                return EvaluateExpression(obj, stack);
        }
    }

    private (DynamicFunction? function, List<object?>? args, DynamicClass? @class, DynamicNamespace? @namespace) FindFunction(FunctionPointerNode node, object? lastObject, bool isPathStart, Stack<ASTContext> stack)
    {
        var args = node.Arguments.ConvertAll(x => EvaluateExpression(x, stack));
        (DynamicFunction? function, List<object?>? args) pair;
        
        // if is path start like: call().., other..
        if (isPathStart)
        {
            if (HasContext(stack))
            {
                var currentContext = CurrentContext(stack);
                // Try to get function from function variables
                if (currentContext?.Function is not null)
                {
                    pair = TryGetFunctionFromFunctionVariables(currentContext.Function, node.NameId, args, stack);
                    if (pair.function is not null)
                        return (pair.function, pair.args, currentContext.Class, currentContext.Namespace);
                }
                
                // Try to get function from class context;
                if (currentContext?.Class is not null)
                {
                    pair = TryGetFunctionFromClass(currentContext.Class, node.NameId, args, stack);
                    if (pair.function is not null)
                        return (pair.function, pair.args, currentContext.Class, currentContext.Namespace);
                }
                
                // Try to get function from namespace context;
                if (currentContext?.Namespace is not null)
                {
                    pair = TryGetFunctionFromNamespace(currentContext.Namespace, node.NameId, args, stack);
                    if (pair.function is not null)
                        return (pair.function, pair.args, null, currentContext.Namespace);
                }
            }
            

            // Try to get function from global list
            var funcPair = GetFunctionFromGlobal(node.NameId, stack, args);
            if (funcPair.function is not null)
                return (ToDynamicFunction(funcPair.function, stack), funcPair.args, null, null);
            
            throw new QlangRuntimeException($"Function '{_stringPoolTable[node.NameId]}' is not found in current context {(lastObject is null ? "" : $"('{lastObject}')")}",
                GetCurrentDebug(stack), GetStackTrace(stack));
        }

            // Try to get function from function variables
        if (lastObject is DynamicFunction function)
        {
            pair = TryGetFunctionFromFunctionVariables(function, node.NameId, args, stack);
            if (pair.function is not null)
                return (pair.function, pair.args, CurrentContext(stack)?.Class, CurrentContext(stack)?.Namespace);
        }
        
        // Try to get function from namespace function list;
        if (lastObject is DynamicNamespace dNamespace)
        {
            pair = TryGetFunctionFromNamespace(dNamespace, node.NameId, args, stack);
            if (pair.function is not null)
            {
                if (pair.function.IsPrivate)
                    throw new QlangRuntimeException($"Cannot get access to private function '{pair.function.Name}' from namespace '{dNamespace.Name}'", GetCurrentDebug(stack), GetStackTrace(stack));
                
                return (pair.function, pair.args, null, dNamespace);
            }
        }

        if (lastObject is DynamicClass dClass)
        {
            pair = TryGetFunctionFromClass(dClass, node.NameId, args, stack);
            if (pair.function is not null)
            {
                if (pair.function.IsPrivate)
                    throw new QlangRuntimeException($"Cannot get access to private function '{pair.function.Name}' from class '{dClass.Name}'", GetCurrentDebug(stack), GetStackTrace(stack));

                var id = _stringPoolTable.Add(dClass.Name);
                var @namespace =
                    _namespaces.FirstOrDefault(x => x.Value.Classes.Any(y => y.NameId == id));

                return (pair.function, pair.args, dClass, @namespace.Value);
            }
        }

        if (_stringPoolTable[node.NameId] == "toString" && lastObject is not null)
            return (null, null, null, null);
        
        // Console.WriteLine($"Context: Class: {CurrentContext(stack)?.Class?.Name}; Function: {CurrentContext(stack)?.Function?.Name}; Namespace: {CurrentContext(stack)?.Namespace?.Name}");
        
        throw new QlangRuntimeException($"Function '{_stringPoolTable[node.NameId]}' is not found in current context {(lastObject is null ? "" : $"('{lastObject}')")}",
            GetCurrentDebug(stack), GetStackTrace(stack));
    }

    private (object? @object, DynamicNamespace? @namespace) FindObject(ObjectPointerNode node, object? lastObject, bool isPathStart, Stack<ASTContext> stack)
    {
        if (node.NameId == -1)
            throw new QlangRuntimeException("Part of path is null.", GetCurrentDebug(stack), GetStackTrace(stack));
        
        var nodeName = _stringPoolTable[node.NameId];
            
        if (isPathStart)
        {
            // Get 'this'
            if (node.NameId == _stringPoolTable.Add(Keywords.ThisKeyword) && HasContext(stack))
            {
                CurrentContext(stack)!.AllowPrivateCall = true;
                return (CurrentContext(stack)!.Class, CurrentContext(stack)!.Namespace);
            }

            CurrentContext(stack)!.AllowPrivateCall = false;


            // Try to get VAR from current (class or namespace or function or blocks) context;
            if (HasContext(stack))
            {
                var block = CurrentContext(stack)!.Blocks
                    .FirstOrDefault(b => b.Variables.ContainsKey(nodeName));

                var currentContext = CurrentContext(stack);
                var classIsNull = currentContext!.Class is null;
                var functionIsNull = currentContext.Function is null;
                var parentFunctionIsNull = currentContext.ParentFunction is null;
                var namespaceIsNull = currentContext.Namespace is null;

                Variable? var;
                
                // Get from function
                if (!functionIsNull)
                {
                    var = currentContext.Function?.Variables.FirstOrDefault(funcVar => funcVar.Key == nodeName).Value;

                    if (var is not null)
                        return (var, currentContext.Namespace);
                }
                
                // Get from parent function
                if (!parentFunctionIsNull)
                {
                    var = currentContext.ParentFunction!.Variables.FirstOrDefault(funcVar => funcVar.Key == nodeName).Value;

                    if (var is not null)
                        return (var, currentContext.Namespace);
                }


                // Get from class
                if (!classIsNull)
                {
                    var = currentContext.Class?.Variables.FirstOrDefault(classVar => classVar.Key == nodeName).Value;
                    
                    if (var is not null)
                        return (var, currentContext.Namespace);
                }

                // Get from namespace
                if (!namespaceIsNull)
                {
                    var = currentContext.Namespace?.Variables.FirstOrDefault(namespaceVar =>
                        namespaceVar.Key == nodeName).Value;
                    
                    if (var is not null)
                        return (var, currentContext.Namespace);
                }
                
                // Get from blocks
                if (block != null && block.Variables.TryGetValue(nodeName, out var v))
                    return (v, currentContext.Namespace);
            }

            // Try to get VAR from global variable list;
            var variable = _namespaces[GlobalNamespaceName].Variables.FirstOrDefault(var => var.Value.Name == nodeName).Value;
            if (variable is not null)
                return (variable, null);
        }

        if (lastObject is null)
        {
            throw new QlangRuntimeException($"Object '{nodeName}' is not found in current context.  PF:{CurrentContext(stack)?.ParentFunction?.Name} F:{CurrentContext(stack)?.Function?.Name}",
                GetCurrentDebug(stack), GetStackTrace(stack));
        }
        
        switch (lastObject)
        {
            case DynamicClass dynamicClass:
                // Try to get VAR from class
                if (dynamicClass.Variables.TryGetValue(nodeName, out var var))
                {
                    if (var.IsPrivate && !CurrentContext(stack)!.AllowPrivateCall)
                        throw new QlangRuntimeException($"Cannot get access to private variable '{var.Name}' from class '{dynamicClass.ClassName}'.",
                            GetCurrentDebug(stack), GetStackTrace(stack));
                        
                    return (var, null);
                }
                break;
            case DynamicNamespace dynamicNamespace:
                // Try to get VAR from namespace
                if (dynamicNamespace.Variables.TryGetValue(nodeName, out var))
                {
                    if (var.IsPrivate)
                        throw new QlangRuntimeException($"Cannot get access to private variable '{var.Name}' from namespace '{dynamicNamespace.Name}'.",
                            GetCurrentDebug(stack), GetStackTrace(stack));
                    
                    return (var, null);
                }
                break;
        }
        throw new QlangRuntimeException($"Object '{nodeName}' is not found in current context" + (HasContext(stack) ? $"\nCurrent function: '{CurrentContext(stack)?.Function?.Name}'; Current class: '{CurrentContext(stack)?.Class?.ClassName}'; Current namespace: '{CurrentContext(stack)?.Namespace?.Name}'" : "\nNo context found."),
            GetCurrentDebug(stack), GetStackTrace(stack));
    }

    private DynamicNamespace FindNamespace(NamespacePointerNode node, object? lastObject, bool isPathStart, Stack<ASTContext> stack)
    {
        var nodeName = _stringPoolTable[node.NameId];

        if (_namespaces.TryGetValue(nodeName, out var @namespace) && isPathStart)
            return @namespace;

        if (HasContext(stack) && CurrentContext(stack)?.Namespace is not null && isPathStart)
        {
            var ns = CurrentContext(stack)!.Namespace!.Namespaces.FirstOrDefault(ns => ns.Name == nodeName);

            if (ns is not null)
                return ns;
        }

        if (lastObject is DynamicNamespace dynamicNamespace)
        {
            var ns = dynamicNamespace.Namespaces.FirstOrDefault(ns => ns.Name == nodeName);

            if (ns is not null)
                return ns.IsPrivate
                    ? throw new QlangRuntimeException(
                        $"Cannot get access to private namespace from namespace '{dynamicNamespace.Name}'",
                        GetCurrentDebug(stack), GetStackTrace(stack))
                    : ns;
        }

        throw new QlangRuntimeException(
            $"Namespace '{_stringPoolTable[node.NameId]}' is not found in current context", GetCurrentDebug(stack),
            GetStackTrace(stack));
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromNamespace(DynamicNamespace source, int functionName, List<object?>? args, Stack<ASTContext> stack)
    {
        var func = GetFunctionFromNamespace(source, functionName, stack, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function, stack), finalArgs: func.Args);
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromClass(DynamicClass? source, int nameId, List<object?>? args, Stack<ASTContext> stack)
    {
        if (source is null)
            throw new QlangRuntimeException("Source is null", -1, "", GetStackTrace(stack));
        
        var func = GetFunctionFromClass(source, nameId, stack, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function, stack), finalArgs: func.Args);
    }
    
    private (DynamicFunction? function, List<object?>? args) TryGetFunctionFromFunctionVariables(DynamicFunction? source, int functionName, List<object?>? args, Stack<ASTContext> stack)
    {
        if (source is null)
            throw new QlangRuntimeException("Source is null", -1, "", GetStackTrace(stack));
        
        var func = GetFunctionFromFunctionVariables(source, functionName, stack, args);
        
        return func.function is null ? (null, null) : (ToDynamicFunction(func.function, stack), finalArgs: func.Args);
    }
}