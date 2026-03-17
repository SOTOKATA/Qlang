using System.Text;
using Core;
using Core.AST;
using Core.Dynamic;
using Core.Exceptions;
using Core.Native;
using Core.Tables;
using Math = System.Math;

namespace Interpreter;

public partial class Interpreter
{
    public Interpreter(List<double> numberList, NativeFunctionRegistry nativeFunctions, SourceFileTable? sourceFileTable, DebugTable? debugTable, StringPoolTable stringPoolTable)
    {
        _numberList = numberList;
        _nativeFunctions = nativeFunctions;
        _sourceFileTable = sourceFileTable;
        _sourceFileTable?.RebuildCache();
        _debugTable = debugTable;
        _stringPoolTable = stringPoolTable;
    }

    private readonly SourceFileTable? _sourceFileTable;
    private readonly DebugTable? _debugTable;
    private readonly StringPoolTable _stringPoolTable;

    private readonly NativeFunctionRegistry _nativeFunctions;

    private readonly List<double> _numberList;

    private readonly Dictionary<string, DynamicNamespace> _namespaces = new();

    private static bool HasContext(Stack<ASTContext> stack) => stack.Count > 0;
    private static ASTContext? CurrentContext(Stack<ASTContext> stack) => HasContext(stack) ? stack.Peek() : null;

    private const string GlobalNamespaceName = "~global";

    /// <summary>
    /// Function to execute program
    /// </summary>
    /// <param name="program">Program tu execute</param>
    /// <param name="args">Args for main function</param>
    /// <exception cref="QlangRuntimeException">Any exception during program execute</exception>
    public void Execute(ProgramNode program, List<string?>? args = null)
    {
        // Main thread
        var stack = new  Stack<ASTContext>();
        
        // First is load global namespace with core classes
        var globalNamespace = program.Statements.OfType<NamespaceNode>().FirstOrDefault(x => _stringPoolTable[x.NameId] == GlobalNamespaceName);

        if (globalNamespace is not null)
        {
            
            var dynamicNamespace = new DynamicNamespace(GlobalNamespaceName);
            _namespaces[GlobalNamespaceName] = dynamicNamespace;
            
            _namespaces[GlobalNamespaceName] = 
                ToDynamicNamespace(globalNamespace);

            program.Statements.Remove(globalNamespace);
        }
        
       
            // Load namespaces
        foreach (var namespaceNode in program.Statements.OfType<NamespaceNode>().Where(x => _stringPoolTable[x.NameId] != GlobalNamespaceName))
            _namespaces[_stringPoolTable[namespaceNode.NameId]] = ToDynamicNamespace(namespaceNode);

        // Convert variable values
        foreach (var key in _namespaces.Keys.ToList())
            _namespaces[key] = ToDynamicNamespaceVariables(_namespaces[key], stack);

        // Search main function
        var function = _namespaces[GlobalNamespaceName].Functions.FirstOrDefault(f => _stringPoolTable[f.NameId] == "main");
        if (function is null)
        {
            throw new QlangRuntimeException(
                "No 'main' function found in program.",
                0, "",
                []);
        }
        
        // Run main function (and send arguments if exists)
        if (function.Parameters.Count == 0)
            ExecuteFunction(ToDynamicFunction(function, stack), [], null, _namespaces[GlobalNamespaceName], new Stack<ASTContext>());
        else 
            ExecuteFunction(ToDynamicFunction(function, stack), [args?.Cast<object?>().ToList()], null, _namespaces[GlobalNamespaceName], new Stack<ASTContext>());
    }
    
    private static void AddContext(Stack<ASTContext> stack, ASTContext context)
    {
        stack.Push(context);
    }

    /// <summary>
    /// Runs function
    /// </summary>
    /// <param name="function">dynamic function to execute</param>
    /// <param name="arguments">argument (similar to other arguments in other languages)</param>
    /// <param name="ownerClass">owner class if exists (required for context)</param>
    /// <param name="ownerNamespace">owner namespace if exists (required for context)</param>
    /// <param name="stack">Current context stack</param>
    /// <returns></returns>
    /// <exception cref="QlangRuntimeException">will call exception if arguments is not valid or during execution function exception will occur</exception>
    private object? ExecuteFunction(DynamicFunction? function, List<object?> arguments, DynamicClass? ownerClass, DynamicNamespace? ownerNamespace, Stack<ASTContext> stack)
    {
        if (function is null)
            return null;
        
        // Set new context
        var contextClass = function.Context?.Class ?? ownerClass ?? (HasContext(stack) ? CurrentContext(stack)?.Class : null);
        var contextNamespace = function.Context?.Namespace ?? ownerNamespace ?? (HasContext(stack) ? CurrentContext(stack)?.Namespace : null);
        ASTContext newContext = new() { Function = function, ParentFunction = function.Context?.ParentFunction, Class = contextClass, Namespace = contextNamespace};

        AddContext(stack, newContext);

        // Try to parse params
        try
        {
            if (arguments.Count == function.Parameters.Count)
                for (var i = 0; i < function.Parameters.Count; i++)
                {
                    var var = function.Variables[function.Parameters[i]];

                    var typeofArgument = Typeof(arguments[i], stack);

                    if (var.Types.Count > 0 &&
                        var.Types.All(x => Typeof(x, stack) != typeofArgument) &&
                        (PrimitiveToDynamicClass(arguments[i], stack) is not DynamicClass d1 ||
                         !d1.Extends.Exists(x => var.Types.Any(y => Typeof(y, stack) == x))))
                    {
                        throw new QlangRuntimeException($"The type of param is '{(Typeof(arguments[i], stack))}' but must be '{string.Join("|", var.Types.Select(x => Typeof(x, stack)))}' for function '{function.Name}'", GetStackTrace(stack));
                    }
                    
                    function.Variables[function.Parameters[i]] = new Variable(
                        function.Parameters[i],
                        arguments[i],
                        function.IsStatic,
                        false);
                }
            else
                throw new QlangRuntimeException("The number of arguments must be equal to the number of params", GetStackTrace(stack));

            // Execute function body
            var current = CurrentContext(stack);
            current!.IsReturn = false;
            current.IsBreak = false;
            current.IsContinue = false;
            current.ReturnValue = null;
            foreach (var statement in function.Body.TakeWhile(_ => !current.IsReturn))
            {
                if (statement is ReturnNode returnNode)
                {
                    if (returnNode.ReturnValue is not null)
                        current.ReturnValue = EvaluateExpression(returnNode.ReturnValue, stack);

                    break;
                }

                ExecuteStatement(statement, stack);
            }

            current.IsReturn = false;
            current.IsBreak = false;
            current.IsContinue = false;

            var typeofReturnValue = Typeof(current.ReturnValue, stack);
            var functionTypes = function.ReturnTypes.Select(x => Typeof(x, stack)).ToList();
            
            if (functionTypes.Count > 0 && functionTypes.All(x => x != typeofReturnValue))
                throw new QlangRuntimeException($"Function return type '{string.Join("|", functionTypes)}' is not equal to returned value type '{typeofReturnValue}'", GetCurrentDebug(stack),
                    GetStackTrace(stack));
            
            if (function.Name == "~function")
                Console.WriteLine($"ReturnTypes: '{string.Join("|", function.ReturnTypes)}' for function ~function");
            return current.ReturnValue;
        }
        finally
        {
            RestoreContextStack(stack);
        }
    }

    /// <summary>
    /// Function used for execute lines or structures
    /// </summary>
    /// <param name="statement">Statement to execute</param>
    /// <param name="stack">Current context stack</param>
    /// <exception cref="QlangRuntimeException">Will throw exception if statement is not exists</exception>
    private void ExecuteStatement(ASTNode? statement, Stack<ASTContext> stack)
    {
        if (HasContext(stack))
            CurrentContext(stack)!.CurrentNode = statement;
        
        switch (statement)
        {
            case AssignmentNode assign:
                AssignmentNode(assign, stack);
                break;

            case CallNode call:
                ExecuteObjectCalls(call, stack);
                break;

            case IfNode ifNode:
                ExecuteIf(ifNode, stack);
                break;
            
            case TryCatchNode tryCatchNode:
                ExecuteTryCatch(tryCatchNode, stack);
                break;
            
            case SwitchNode switchNode:
                ExecuteSwitch(switchNode, stack);
                break;

            case WhileNode whileNode:
                ExecuteWhile(whileNode, stack);
                break;

            case ForNode forNode:
                ExecuteFor(forNode, stack);
                break;
            
            case LineNode lineNode:
                EvaluateLine(lineNode, stack);
                break;
            
            default:
                throw new QlangRuntimeException($"Unknown statement type: {statement?.GetType().Name ?? "<null>"}", GetCurrentDebug(stack), GetStackTrace(stack));
        }
    }
    
    
    /// <summary>
    /// Used for operate with assignments. Can create or change value
    /// </summary>
    /// <param name="assignmentNode">Assignment structure (like: 'const a = 1')</param>
    /// <param name="stack">Current context stack</param>
    /// <exception cref="QlangRuntimeException">
    /// 1. If assignment path is empty.
    /// 2. If assignment try to re-assign const variable
    /// 3. If assignment try to assign private variable from external source
    /// 4. If assignment has invalid assignment part of path
    /// </exception>
    private void AssignmentNode(AssignmentNode assignmentNode, Stack<ASTContext> stack)
    {
        if (stack.Count == 0)
            return;

        var value = EvaluateExpression(assignmentNode.Value, stack);
        var valueType = Typeof(value, stack);

        var path = assignmentNode.Path;

        if (path.Count == 0)
            throw new QlangRuntimeException("Assignment path cannot be empty", GetCurrentDebug(stack), GetStackTrace(stack));

        var lastNode = (ObjectPointerNode)path[^1];

        object? currentObject = null;

        // Execute path except last part
        if (path.Count > 1)
        {
            var callNode = new CallNode
            {
                Objects = path.SkipLast(1).ToList(),
            };

            currentObject = ExecuteObjectCalls(callNode, stack);
        }

        var assignName = _stringPoolTable[assignmentNode.GetLastNameId()];

        if (!assignmentNode.IsNew)
        {
            var obj = FindObject(lastNode, currentObject, currentObject is null, stack);

            if (obj.@object is Variable var)
            {
                if (var.IsConst)
                    throw new QlangRuntimeException(
                        $"Cannot re-assign const property '{_stringPoolTable[lastNode.NameId]}'",
                        GetCurrentDebug(stack), GetStackTrace(stack));

                if (currentObject is not null && var.IsPrivate && !CurrentContext(stack)!.AllowPrivateCall)
                    throw new QlangRuntimeException(
                        "Cannot access to private variable from external source",
                        GetCurrentDebug(stack), GetStackTrace(stack));

                if (var.Types.Count > 0 && var.Types.Any(x => Typeof(x, stack) != valueType))
                    throw new QlangRuntimeException(
                        $"Cannot assign value of type '{valueType}' to variable '{assignName}'. Expected type: '{string.Join("|", var.Types.Select(x => x.ToTokenString(_stringPoolTable)))}'",
                        GetCurrentDebug(stack), GetStackTrace(stack));

                var.Value = value;
                return;
            }

            throw new QlangRuntimeException(
                $"Invalid assignment target: {obj.@object?.GetType().Name}",
                GetCurrentDebug(stack), GetStackTrace(stack));
        }

        Dictionary<string, Variable> variables;

        if (HasContext(stack))
        {
            var ctx = CurrentContext(stack)!;

            if (ctx.Function is not null)
                variables = ctx.Function.Variables;
            else if (ctx.Class is not null)
                variables = ctx.Class.Variables;
            else if (ctx.Namespace is not null)
                variables = ctx.Namespace.Variables;
            else
                variables = _namespaces[GlobalNamespaceName].Variables;
        }
        else
            variables = _namespaces[GlobalNamespaceName].Variables;
        
        var variable = Variable.FromAssignmentNode(assignmentNode, value, _stringPoolTable);
        
        if (variable.Types.Count > 0 && variable.Types.Any(x => Typeof(x, stack) != valueType))
            throw new QlangRuntimeException(
                $"Cannot assign value of type '{valueType}' to variable '{assignName}'. Expected type: '{string.Join("|", variable.Types.Select(x => x.ToTokenString(_stringPoolTable)))}'",
                GetCurrentDebug(stack), GetStackTrace(stack));

        variables[assignName] = Variable.FromAssignmentNode(assignmentNode, value, _stringPoolTable);
    }

    /// <summary>
    /// Creates new class instance of sent class
    /// </summary>
    /// <param name="classNode">Copy of class to create</param>
    /// <param name="args">Arguments for function 'new'</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>New instance</returns>
    /// <exception cref="QlangRuntimeException">If was sent invalid parameters</exception>
    private DynamicClass GetNewClass(ClassNode classNode, List<object?> args, Stack<ASTContext> stack)
    {
        var dClass = ToDynamicClass((ClassNode)classNode.Clone(), stack);

        var fromClass = GetFunctionFromClass(dClass, _stringPoolTable.Add(Keywords.CreateClassInstanceKeyword), stack, args);

        var indexOfNewClass = _stringPoolTable.Add(Keywords.CreateClassInstanceKeyword);
        if (dClass.Body.OfType<FunctionNode>().Any(f => f.NameId == indexOfNewClass) &&
            fromClass.function == null)
            throw new QlangRuntimeException($"Can't initialize class '{_stringPoolTable[classNode.NameId]}' with this parameters.", GetStackTrace(stack));

        if (fromClass.function != null)
            ExecuteFunction(ToDynamicFunction(fromClass.function, stack), fromClass.Args, dClass, null, stack);

        return dClass;
    }

    private static void RestoreContextStack(Stack<ASTContext> stack)
    {
        if (!HasContext(stack)) 
            return;
        
        stack.Pop();
    }

    /// <summary>
    /// Parse text to c# string. Like 'Hello, World!\t' to 'Hello, World!    '
    /// </summary>
    /// <param name="input">Input string to execute</param>
    /// <returns></returns>
    private static string ParseString(ReadOnlySpan<char> input)
    {
        var sb = new StringBuilder(input.Length);

        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == '\\' && i + 1 < input.Length)
            {
                var next = input[i + 1];

                switch (next)
                {
                    case 'n':
                        sb.Append('\n');
                        i++;
                        break;

                    case 't':
                        sb.Append('\t');
                        i++;
                        break;

                    case '\\':
                        sb.Append('\\');
                        i++;
                        break;

                    case '\"':
                        sb.Append('\"');
                        i++;
                        break;

                    case 'u':
                        if (i + 5 < input.Length)
                        {
                            var hexSpan = input.Slice(i + 2, 4);

                            if (int.TryParse(hexSpan, 
                                    System.Globalization.NumberStyles.HexNumber,
                                    null,
                                    out var code))
                            {
                                sb.Append((char)code);
                                i += 5;
                                break;
                            }
                        }

                        sb.Append('\\');
                        break;

                    default:
                        sb.Append('\\');
                        break;
                }
            }
            else
            {
                sb.Append(input[i]);
            }
        }

        return sb.ToString();
    }

    private static FunctionNode PrepareFunctionNodePointer(FunctionNode node, Stack<ASTContext> stack)
    {
        var currentContext = CurrentContext(stack);
        if (currentContext is not null && node.Context is null)
        {
            node.Context = currentContext.Copy();
            node.Context.ParentFunction = node.Context.Function;
        }
        
        return node;
    }

    /// <summary>
    /// Evaluate expressions like casting, keywords, string refs
    /// </summary>
    /// <param name="expr">Expression to evaluate</param>
    /// <param name="stack">Current context stack</param>
    /// <returns></returns>
    /// <exception cref="QlangRuntimeException">Will throw if expression is undefined</exception>
    private object? EvaluateExpression(ASTNode? expr, Stack<ASTContext> stack)
    {
        if (expr is null)
            return null;

        if (HasContext(stack))
            CurrentContext(stack)!.CurrentNode = expr;

        try
        {
            if (expr is ParallelNode parallel)
            {
                ExecuteParallel(parallel, stack);
                return null;
            }
            
            return expr switch
            {
                LineNode ln => EvaluateLine(ln, stack),
                CastNode cast => CastObject(cast, stack),
                StringRefNode strRef => _stringPoolTable[strRef.Index],
                NumberRefNode numberRef => GetNumberRef(numberRef, stack),
                ClassNode classNode => ToDynamicClass(classNode, stack),
                BooleanNode booleanNode => booleanNode.Value,
                NumberNode num => num.Value,
                BinaryOperationNode binOp => EvaluateBinaryOperation(binOp, stack),
                CollectionNode collection => collection.Collection.ConvertAll(x => EvaluateExpression(x, stack)),
                KeywordNode node when _stringPoolTable[node.KeywordId] == Keywords.NullKeyword => null,
                NewNode newNode => EvaluateNewKeyword(newNode, stack),
                CallNode call => ExecuteObjectCalls(call, stack),
                ShortHandIfNode shortIf => ExecuteShortIf(shortIf, stack),
                FunctionNode fn => PrepareFunctionNodePointer(fn, stack),
                _ => throw new QlangRuntimeException(
                    $"Unknown expression type: {expr.GetType().Name}",
                    GetCurrentDebug(stack),
                    GetStackTrace(stack))
            };
        }
        catch (QlangRuntimeException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new QlangRuntimeException(
                $"Internal error: {ex}",
                GetCurrentDebug(stack),
                GetStackTrace(stack));
        }
    }

    private object? EvaluateLine(LineNode ln, Stack<ASTContext> stack)
    {
        if (HasContext(stack))
            CurrentContext(stack)!.CurrentDebugIndex = ln.DebugIndex;
        
        if (ln.Content is AssignmentNode assignment)
        {
            ExecuteStatement(assignment, stack);
            return null;
        }

        return EvaluateExpression(ln.Content, stack);
    }
    
    private void ExecuteParallel(ParallelNode node, Stack<ASTContext> parentStack)
    {
        var currentCtx = CurrentContext(parentStack);

        var expr = EvaluateExpression(node.Object, parentStack);
        
        // Array founded
        if (expr is not DynamicClass { Name: QlSystemClasses.ArrayClassName } array)
            throw new QlangRuntimeException("Undefined array", GetCurrentDebug(parentStack),
                GetStackTrace(parentStack));

        // Array has valid collection
        if (array.Variables["_value"].Value is not List<object?> objects || objects.Any(x => x is not ASTNode))
            throw new QlangRuntimeException("Undefined collection", GetCurrentDebug(parentStack),
                GetStackTrace(parentStack));

        var nodes = objects.Cast<ASTNode>().ToList();

        var tasks =  nodes.Select(astNode =>
        {
            return Task.Run(() =>
            {
                var forkedStack = new Stack<ASTContext>();

                if (currentCtx is not null)
                    forkedStack.Push(currentCtx.Copy());

                // Get FunctionNode
                var ret = EvaluateExpression(astNode, parentStack);

                if (ret is not FunctionNode function)
                    throw new QlangRuntimeException("Undefined function pointer", GetCurrentDebug(parentStack),
                        GetStackTrace(parentStack));

                var context = CurrentContext(forkedStack);
                ExecuteFunction(ToDynamicFunction(function, forkedStack), [], context?.Class, context?.Namespace,
                    forkedStack);
            });
        }).ToArray();

        try
        {
            Task.WaitAll(tasks);
        }
        catch (AggregateException e)
        {
            throw e.InnerException!;
        }
    }

    private DynamicClass EvaluateNewKeyword(NewNode node, Stack<ASTContext> stack)
    {
        if (node.NodePath.Objects.Count < 1)
            throw new QlangRuntimeException("Undefined path to class.", GetCurrentDebug(stack), GetStackTrace(stack));

        var pointer = (FunctionPointerNode)node.NodePath.Objects[^1];
        var classNode = GetClassNodeByPath(node.NodePath, stack);
        

        return GetNewClass(classNode, pointer.Arguments.ConvertAll(x => EvaluateExpression(x, stack)), stack);
    }

    private DynamicClass ExecutePathToClass(CallNode callNode, Stack<ASTContext> stack)
    {
        return ToDynamicClass(GetClassNodeByPath(callNode, stack), stack);
    }

    private ClassNode GetClassNodeByPath(CallNode node, Stack<ASTContext> stack)
    {
        var pointer = node.Objects[^1];

        var nameId = pointer switch
        {
            NamespacePointerNode fn => fn.NameId,
            FunctionPointerNode fn => fn.NameId,
            ObjectPointerNode ns => ns.NameId,
            _ => throw new QlangRuntimeException($"Undefined call part: '{pointer.ToTokenString(_stringPoolTable)}'", GetCurrentDebug(stack), GetStackTrace(stack))
        };

        var isPrivateCall = false;

        DynamicNamespace? @namespace = null;

        if (HasContext(stack))
        {
            @namespace = CurrentContext(stack)?.Namespace;
            isPrivateCall = true;
        }

        if (node.Objects.Count > 1)
        {
            node.Objects = node.Objects.SkipLast(1).ToList();
            var obj = ExecuteObjectCalls(node, stack);

            if (obj is not DynamicNamespace ns)
                throw new QlangRuntimeException($"Undefined namespace: '{obj}'", GetCurrentDebug(stack), GetStackTrace(stack));

            isPrivateCall = false;
            @namespace = ns;
        }

        var classNode = @namespace?.Classes.FirstOrDefault(x => x.NameId == nameId) ?? 
                        _namespaces[GlobalNamespaceName].Classes.FirstOrDefault(x => x.NameId == nameId);

        if (classNode is null)
            throw new QlangRuntimeException($"Class '{_stringPoolTable[nameId]}' is not founded." +
                                            (@namespace is not null ? $"\nIn namespace '{@namespace.Name}'" : ""), GetCurrentDebug(stack), GetStackTrace(stack));
        
        if (!isPrivateCall && classNode.IsPrivate)
            throw new QlangRuntimeException($"Cannot instantiate private class '{_stringPoolTable[classNode.NameId]}'", GetCurrentDebug(stack), GetStackTrace(stack));

        return classNode;
    }

    /// <summary>
    /// Casting objects, cannot cast dynamic classes at now
    /// </summary>
    /// <param name="cast">Cast node</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>Cast object</returns>
    /// <exception cref="QlangRuntimeException">If casting is impossible (like bool to double)</exception>
    private object? CastObject(CastNode cast, Stack<ASTContext> stack)
    {
        var type = ExecutePathToClass(cast.TypeCastPath, stack);
        var @object = ExecuteObjectCalls(cast.ToCastObject, stack);
        
        if (Typeof(type, stack) == "Number" && @object is DynamicClass { ClassName: QlSystemClasses.StringClassName } @class &&
            @class.Variables["_value"].Value!.ToString().TryParseNumber(out var number))
            return number;

        // cast string to number
        if (Typeof(type, stack) == "Number" && @object is string str 
                                            && str.TryParseNumber(out number)) 
            return number;
        
        if (Typeof(type, stack) == "Number" && @object is int or long or double or float)
            return Convert.ToDouble(@object);
        
        // cast any object to string
        if (Typeof(type, stack) == "String" && @object is not null)
            return @object.ToString();
        
        return CreateClassFrom(@object, type, stack);
    }
    
    /// <summary>
    /// Divide two numbers with check if divisor equal to 0
    /// </summary>
    /// <param name="left">To divide</param>
    /// <param name="right">Divisor</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>Divided value</returns>
    /// <exception cref="QlangRuntimeException">If divisor equal to 0</exception>
    private double DivideWithCheck(object left, object right, Stack<ASTContext> stack)
    {
        var divisor = right.ToString().ParseNumber();

        if (Math.Abs(divisor) < double.Epsilon)
        {
            throw new QlangRuntimeException(
                "Cannot divide by zero.",
                GetCurrentDebug(stack),
                GetStackTrace(stack));
        }
        return left.ToString().ParseNumber() / divisor;
    }

    /// <summary>
    /// Get number by reference
    /// </summary>
    /// <param name="numberRef">Reference node</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>Number</returns>
    /// <exception cref="QlangRuntimeException">If is undefined reference</exception>
    private double GetNumberRef(NumberRefNode numberRef, Stack<ASTContext> stack)
    {
        if (numberRef.Index >= _numberList.Count || numberRef.Index < 0)
        {
            throw new QlangRuntimeException(
                $"Undefined number reference: '{numberRef.Index}'",
                GetCurrentDebug(stack),
                GetStackTrace(stack));
        }

        var number = _numberList[numberRef.Index];

        return numberRef.IsNegative ? -number : number;
    }

    /// <summary>
    /// Create class from class copy using '_createFrom' function
    /// </summary>
    /// <param name="obj">Object to cast</param>
    /// <param name="copy">Copy of class with function '_createFrom'</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>new class instance class</returns>
    /// <exception cref="QlangRuntimeException">Will throw if values is incompatible</exception>
    private DynamicClass CreateClassFrom(object? obj, DynamicClass copy, Stack<ASTContext> stack)
    {
        if (obj is DynamicClass dynamic && dynamic.ClassName == copy.ClassName)
            return dynamic;
        
        var createFunction = copy.Body
            .OfType<FunctionNode>()
            .FirstOrDefault(f => _stringPoolTable[f.NameId] == "_createFrom");

        if (createFunction is null)
            throw new QlangRuntimeException(
                "Cannot apply this operation because neither class implements '_createFrom'.",
                GetCurrentDebug(stack),
                GetStackTrace(stack));

        var created = ExecuteFunction(
            ToDynamicFunction(createFunction, stack),
            [obj],
            copy,
            null, stack);

        if (created is not DynamicClass dClass ||
            dClass.ClassName != copy.ClassName)
            throw new QlangRuntimeException(
                "Cannot apply this operation between these classes because the second value is incompatible.",
                GetCurrentDebug(stack),
                GetStackTrace(stack));

        return dClass;
    }

    /// <summary>
    /// Evaluate binary operation with DynamicClasses
    /// </summary>
    /// <param name="left">First object</param>
    /// <param name="right">Second object</param>
    /// <param name="binOp">Binary process (with operator)</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>Output from binary operation</returns>
    /// <exception cref="QlangRuntimeException">Will throw if values is incompatible or undefined</exception>
    private object? EvaluateClassBinaryOperation(object left, object right, BinaryOperationNode binOp, Stack<ASTContext> stack)
    {
        DynamicClass? rightClass = null;
        DynamicClass? leftClass = null;

        if (left is DynamicClass lc)
            leftClass = lc;
        else
            rightClass = (DynamicClass)right;

        leftClass ??= CreateClassFrom(left, rightClass!, stack);
        rightClass ??= CreateClassFrom(right, leftClass, stack);

        var originalOperator = _stringPoolTable[binOp.OperatorId];
        if (originalOperator.Any(c => c is '=' or '>' or '<' or '!' or '*' or '/' or '%' or '+' or '-'))
        {
            var @operator = "";
            for (var i = 0; i < originalOperator.Length; i++)
            {
                @operator += originalOperator[i] switch
                {
                    '=' => "Equal",
                    '>' => "Greater",
                    '<' => "Less",
                    '!' => "Not",
                    '+' => "Addition",
                    '-' => "Subtraction",
                    '/' => "Division",
                    '*' => "Multiplication",
                    '%' => "Percent",
                    _ => throw new QlangRuntimeException("Undefined operator: " + originalOperator[i], GetStackTrace(stack))
                    
                };

                if (i == 0 && originalOperator.Length == 2 && originalOperator[i] == '=' && originalOperator[i + 1] == '=')
                    break;

                if (i == 0 && originalOperator.Length == 2 && originalOperator[i] == '!' && originalOperator[i + 1] == '=')
                {
                    @operator += "Equal";
                    break;
                }
                
                if (i !=  originalOperator.Length - 1)
                    @operator += "Or";
            }

            binOp.OperatorId = _stringPoolTable.Add(@operator);
            originalOperator = @operator;
        }
        
        var opFunction = leftClass.Body
            .OfType<FunctionNode>()
            .FirstOrDefault(f =>
                _stringPoolTable[f.NameId] == $"_operator{originalOperator}" && f.Parameters.Count == 2);

        if (opFunction is null)
            throw new QlangRuntimeException(
                $"Class '{leftClass.ClassName}' is incompatible for operator '{originalOperator}', function prototype '_operator{originalOperator}(const, const)'",
                GetCurrentDebug(stack),
                GetStackTrace(stack));

        var result = ExecuteFunction(
            ToDynamicFunction(opFunction, stack),
            [leftClass, rightClass],
            leftClass, null, stack);

        if ((result is not DynamicClass dynamicClass || dynamicClass.ClassName != leftClass.ClassName) &&
            (originalOperator.Contains("Division") || originalOperator.Contains("Subtraction") || originalOperator.Contains("Multiplication") || originalOperator.Contains("Addition")))
            // Console.WriteLine($"warning: return value of {leftClass.ClassName} is not equal to self type");
            throw new QlangRuntimeException(
                $"Return value of '_operator{originalOperator}' must be equal to type '{leftClass.ClassName}'", GetCurrentDebug(stack),
                GetStackTrace(stack)); 
        
        if (result is not bool &&
            (originalOperator.Contains("Equal") ||  originalOperator.Contains("Greater") || 
             originalOperator.Contains("Less")))
            throw new QlangRuntimeException(
                $"Return value of '_operator{originalOperator}' must be equal to type 'Boolean'", GetCurrentDebug(stack),
                GetStackTrace(stack)); 
        
        return result;
    }

    /// <summary>
    /// Evaluate binary operation like '1 + 1' or 'var % 2 == 0'
    /// </summary>
    /// <param name="binOp">Operation</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>Result of operation</returns>
    /// <exception cref="QlangRuntimeException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private object? EvaluateBinaryOperation(BinaryOperationNode binOp, Stack<ASTContext> stack)
    {
        var left = EvaluateExpression(binOp.Left, stack);
        var right = EvaluateExpression(binOp.Right, stack);

        var @operator = _stringPoolTable[binOp.OperatorId];
        if (left is null || right is null)
        {
            return @operator switch
            {
                "==" => left == right,
                "!=" => left != right,
                _ => null
            };
        }

        if ((left is DynamicClass || right is DynamicClass))
            return EvaluateClassBinaryOperation(left, right, binOp, stack);

        
        bool leftBool;
        bool rightBool;
        switch (@operator)
        {

            case "&&":
                {
                    if (!bool.TryParse(left.ToString(), out leftBool))
                        throw new QlangRuntimeException(
                            $"Left operand of '&&' must be boolean, got '{left}'",
                            GetCurrentDebug(stack), GetStackTrace(stack));

                    if (!leftBool)
                        return false;

                    if (!bool.TryParse(right.ToString(), out rightBool))
                        throw new QlangRuntimeException(
                            $"Right operand of '&&' must be boolean, got '{right}'",
                            GetCurrentDebug(stack), GetStackTrace(stack));

                    return rightBool;
                }
            case "||":
                {
                    if (!bool.TryParse(left.ToString(), out leftBool))
                        throw new QlangRuntimeException(
                            $"Left operand of '||' must be boolean, got '{left}'",
                            GetCurrentDebug(stack), GetStackTrace(stack));

                    if (leftBool)
                        return true;

                    if (!bool.TryParse(right.ToString(), out rightBool))
                        throw new QlangRuntimeException(
                            $"Right operand of '||' must be boolean, got '{right}'",
                            GetCurrentDebug(stack), GetStackTrace(stack));

                    return rightBool;
                }
        }

        // If it's bool condition
        if (bool.TryParse(left.ToString(), out leftBool) && bool.TryParse(right.ToString(), out rightBool))
        {
            return @operator switch
            {
                "==" => Equals(leftBool, rightBool),
                "!=" => !Equals(leftBool, rightBool),
                _ => throw new QlangRuntimeException(
                    $"Unknown operator for boolean: {@operator}",
                    GetCurrentDebug(stack),
                    GetStackTrace(stack))
            };
        }

        if (@operator == "+" && (left is string || right is string))
        {
            if (left is DynamicClass leftClassStr)
            {
                var leftToString = leftClassStr.Body.OfType<FunctionNode>().FirstOrDefault(f => _stringPoolTable[f.NameId] == "toString");

                if (leftToString is not null)
                    left = ExecuteFunction(ToDynamicFunction(leftToString, stack), [left], leftClassStr, null, stack);
            }
            
            if (right is DynamicClass rightClassStr)
            {
                var rightToString = rightClassStr.Body.OfType<FunctionNode>().FirstOrDefault(f => _stringPoolTable[f.NameId] == "toString");

                if (rightToString is not null)
                    right = ExecuteFunction(ToDynamicFunction(rightToString, stack), [right], rightClassStr, null, stack);
            }
            
            return left?.ToString() + right;
        }

        if (!left.ToString().IsNumber() || !right.ToString().IsNumber())
        {
            if (@operator is "==" or "!=")
                return @operator switch
                {
                    "==" => Equals(left, right),
                    "!=" => !Equals(left, right),
                    _ => throw new ArgumentOutOfRangeException()
                };

            throw new QlangRuntimeException(
                $"Cannot apply operator '{@operator}' to " +
                $"'{left.ToString() ?? "null"}' ({left.GetType().Name}) and '{right.ToString() ?? "null"}' ({right.GetType().Name})",
                GetCurrentDebug(stack),
                GetStackTrace(stack));
        }

        // Double operations
        try
        {
            var leftNum = Convert.ToDouble(left);
            var rightNum = Convert.ToDouble(right);

            if (@operator is "==" or "!=" && (Math.Abs(leftNum - Math.Round(leftNum)) < 1e-10 ||
                                              Math.Abs(rightNum - Math.Round(rightNum)) < 1e-10))
                return @operator is "==" ? (int)leftNum == (int)rightNum : (int)leftNum != (int)rightNum;
            
            return @operator switch
            {
                "==" => leftNum == rightNum,
                "!=" => leftNum != rightNum,
                "<" => leftNum < rightNum,
                ">" => leftNum > rightNum,
                ">=" => leftNum >= rightNum,
                "<=" => leftNum <= rightNum,
                "+" => leftNum + rightNum,
                "-" => leftNum - rightNum,
                "*" => leftNum * rightNum,
                "/" => DivideWithCheck(leftNum, rightNum, stack),
                "%" => leftNum % rightNum,
                _ => throw new QlangRuntimeException(
                    $"Unknown operator: {@operator}",
                    GetCurrentDebug(stack),
                    GetStackTrace(stack))
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
                GetCurrentDebug(stack),
                GetStackTrace(stack));
        }
    }
    
    private (FunctionNode? function, List<object?> args) GetFunctionFromGlobal
        (int nameId, Stack<ASTContext> stack, List<object?>? args = null)
    {
        args ??= [];

        foreach (var function in _namespaces[GlobalNamespaceName].Functions.Where(f => f.NameId == nameId))
            if (TryMatchFunction(function, args, out var finalArgs, stack))
                return (function, finalArgs);

        return (null, null)!;
    }
        
    private (FunctionNode? function, List<object?> Args) GetFunctionFromFunctionVariables
        (DynamicFunction? func, int nameId, Stack<ASTContext> stack, List<object?>? args = null)
    {
        if (func is null)
            return (null, null)!;
        
        args ??= [];
        
        var name = _stringPoolTable[nameId];
        var functions = func.Variables.Where(f => f.Key == name).Select(var => var.Value.Value).OfType<FunctionNode>();
        
        foreach (var function in functions)
            if (TryMatchFunction(function, args, out var finalArgs, stack))
                return (function, finalArgs);

        return (null, null)!;
    }
    
    private (FunctionNode? function, List<object?> Args) GetFunctionFromClass
        (DynamicClass? @class, int nameId, Stack<ASTContext> stack, List<object?>? args = null)
    {
        if (@class is null)
            return (null, null)!;
        
        args ??= [];

        var name = _stringPoolTable[nameId];
        var functions = @class.Body.OfType<FunctionNode>().Where(f => f.NameId == nameId).ToList();
        var values = @class.Variables.Where(f => f.Key == name).Select(var => var.Value.Value);
        
        functions.AddRange(values.OfType<FunctionNode>());
        
        foreach (var function in functions)
            if (TryMatchFunction(function, args, out var finalArgs, stack))
                return (function, finalArgs);

        return (null, null)!;
    }
    
    private (FunctionNode? function, List<object?> Args) GetFunctionFromNamespace
        (DynamicNamespace? @namespace, int nameId, Stack<ASTContext> stack, List<object?>? args = null)
    {
        if (@namespace is null)
            return (null, null)!;
        
        args ??= [];

        var name = _stringPoolTable[nameId];
        var functions = @namespace.Functions.Where(f => f.NameId == nameId).ToList();
        var values = @namespace.Variables.Where(f => f.Key == name).Select(var => var.Value.Value);
        
        functions.AddRange(values.OfType<FunctionNode>());
        
        foreach (var function in functions)
            if (TryMatchFunction(function, args, out var finalArgs, stack))
                return (function, finalArgs);

        return (null, null)!;
    }
    private bool TryMatchFunction(FunctionNode function, List<object?> args, out List<object?> finalArgs, Stack<ASTContext> stack)
    {
        finalArgs = [];

        var totalParamsCount = function.Parameters.Count;
    
        var requiredParamsCount = function.Parameters.Count(param => param.Value == null);

        if (args.Count < requiredParamsCount || args.Count > totalParamsCount)
            return false;
    
        for (var i = 0; i < totalParamsCount; i++)
            if (i < args.Count)
                finalArgs.Add(args[i]);
            else
            {
                var param = function.Parameters[i];
                if (param.Value != null)
                {
                    var defaultValue = EvaluateExpression(param.Value, stack);
                    finalArgs.Add(defaultValue);
                }
                else
                    return false;
            }
    
        return true;
    }
    
    private (int, string) GetCurrentDebug(Stack<ASTContext> stack) => GetDebug(CurrentContext(stack)!.CurrentDebugIndex);
    
    private (int, string) GetDebug(int index)
    {
        if (_debugTable is null || _sourceFileTable is null)
            return (-1, "Debug file reference is not found.");

        if (index < 0 || index >= _debugTable.LineIndexes.Count)
            return (-1, "Debug index is " + index);
        
        return (_debugTable.GetLineIndex(index) + 1, _sourceFileTable[_debugTable.GetFileId(index)]);
    }
}
