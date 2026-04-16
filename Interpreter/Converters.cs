using Core;
using Core.AST;
using Core.Dynamic;
using Core.Exceptions;

namespace Interpreter;

public partial class Interpreter
{
    /// <summary>
    /// Convert variables with ASTNode values to normalized
    /// </summary>
    /// <param name="variables">Variables to convert</param>
    /// <param name="stack">Local context stack</param>
    /// <returns>Converted variables</returns>
    private Dictionary<string, Variable> ToDynamicVariables(Dictionary<string, Variable> variables, Stack<ASTContext> stack)
    {
        foreach (var pair in variables)
            if (pair.Value.Value is ASTNode node)
                pair.Value.Value = node is FieldNode fn 
                    ? ToDynamicField(fn, stack) 
                    : EvaluateExpression(node, stack);

        return variables;
    }

    private DynamicNamespace ToDynamicNamespaceVariables(DynamicNamespace dynamicNamespace, Stack<ASTContext> stack)
    {
        AddContext(stack, new ASTContext { Namespace = dynamicNamespace });
        
        for (var index = 0; index < dynamicNamespace.Namespaces.Count; index++)
            dynamicNamespace.Namespaces[index] = ToDynamicNamespaceVariables(dynamicNamespace.Namespaces[index], stack);
        
        dynamicNamespace.Variables = ToDynamicVariables(dynamicNamespace.Variables, stack);

        RestoreContextStack(stack);
        
        return dynamicNamespace;
    }
    
    /// <summary>
    /// Convert static namespace to dynamic
    /// </summary>
    /// <param name="namespaceNode">namespace to convert</param>
    /// <returns>DynamicNamespace</returns>
    private DynamicNamespace ToDynamicNamespace(NamespaceNode namespaceNode)
    {
        // Create dynamic instance
        var dynamicNamespace = new DynamicNamespace(_stringPoolTable[namespaceNode.NameId])
        {
            IsPrivate = namespaceNode.IsPrivate
        };
        
        // Add and convert all classes
        dynamicNamespace.Classes.AddRange(
            namespaceNode.Body
                .OfType<ClassNode>()
                .ToList()
        );
        
        // Add and convert all namespaces
        dynamicNamespace.Namespaces.AddRange(
            namespaceNode.Body.OfType<NamespaceNode>()
                .Select(ToDynamicNamespace)
            );
        

        // Add all functions
        dynamicNamespace.Functions.AddRange(namespaceNode.Body.OfType<FunctionNode>());
        
        // Add and convert all assignments
        foreach (var assignmentNode in namespaceNode.Body.OfType<LineNode>().Select(x => (AssignmentNode)x.Content!))
                dynamicNamespace.Variables[_stringPoolTable[assignmentNode.GetLastNameId()]] = new Variable(_stringPoolTable[assignmentNode.GetLastNameId()],
                    assignmentNode.Value,  assignmentNode
                        .IsPrivate, assignmentNode.IsConst, assignmentNode.Types);
        
        return dynamicNamespace;
    }

    /// <summary>
    /// Convert static class to dynamic
    /// </summary>
    /// <param name="classNode">class to convert</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>DynamicClass</returns>
    private DynamicClass ToDynamicClass(ClassNode classNode, DynamicNamespace? dynamicNamespace, Stack<ASTContext> stack)
    {
        // Create dynamic instance
        DynamicClass dynamicClass = new(_stringPoolTable[classNode.NameId])
        {
            IsPrivate = classNode.IsPrivate,
            Namespace = dynamicNamespace,
            Id = classNode.Id
        };
        
        // Add and convert all assignments
        foreach (var lineNode in classNode.Body.OfType<LineNode>())
        {
            var assignmentNode = (AssignmentNode)lineNode.Content!;
            
            // Add context before evaluation to allow lookup in the current class/namespace
            AddContext(stack, new ASTContext
            {
                CurrentDebugIndex = lineNode.DebugIndex,
                Class = dynamicClass,
                Namespace = dynamicNamespace
            });

            var value = assignmentNode.Value is FieldNode fn 
                ? ToDynamicField(fn, stack) 
                : EvaluateExpression(assignmentNode.Value, stack);
            
            var typeofValue = Typeof(value, stack);
            var name = _stringPoolTable[assignmentNode.GetLastNameId()];
            
            if (!IsTypeCompatible(assignmentNode.Types, value, false, stack))
                throw new QlangRuntimeException(
                    $"Cannot assign value of type '{typeofValue}' to variable '{name}' in class '{dynamicClass.Name}'. Expected type: '{string.Join("|", assignmentNode.Types.Select(x => x.ToTokenString(_stringPoolTable)))}'",
                    GetCurrentDebug(stack), GetStackTrace(stack));
            
            RestoreContextStack(stack);
            
            dynamicClass.Variables[name] = new Variable(
                name, value, assignmentNode.IsPrivate, assignmentNode.IsConst, assignmentNode.Types
            );
        }

        // Remove all AssignmentNodes from body
        classNode = (classNode.Clone() as ClassNode)!;
        classNode.Body.RemoveAll(node => node is LineNode);

        dynamicClass.Extends.AddRange(classNode.Extends.Select(x => _stringPoolTable[x]));
        
        dynamicClass.Body = classNode.Body;
        
        return dynamicClass;
    }
    
    /// <summary>
    /// Convert static field to dynamic
    /// </summary>
    /// <param name="fieldNode">field to convert</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>DynamicField</returns>
    private DynamicField ToDynamicField(FieldNode fieldNode, Stack<ASTContext> stack)
    {
        var privateFieldVariable = new Variable(
            _stringPoolTable[fieldNode.PrivateVariable.GetLastNameId()],
            EvaluateExpression(fieldNode.PrivateVariable.Value, stack),
            true, false, []
            );

        return new DynamicField(
            privateFieldVariable,
            fieldNode.GetFunction,
            fieldNode.SetFunction
        );
    }
    
    /// <summary>
    /// Convert static function to dynamic
    /// </summary>
    /// <param name="functionNode">function to convert</param>
    /// <param name="stack"></param>
    /// <returns>DynamicFunction</returns>
    private DynamicFunction ToDynamicFunction(FunctionNode functionNode, Stack<ASTContext> stack)
    {
        // Create dynamic instance
        DynamicFunction dynamicFunction = new(_stringPoolTable[functionNode.NameId])
        {
            Context = functionNode.Context,
            ReturnTypes = functionNode.ReturnTypes.Select(x => (CallNode)x.Clone()).ToList()
        };
        
        // Add and convert all parameters
        foreach (var node in functionNode.Parameters)
        {
            var nodeName = _stringPoolTable[node.GetLastNameId()];
            var value = node.Value is FieldNode fn 
            ? ToDynamicField(fn, stack) 
            : EvaluateExpression(node.Value, stack);
            
            var typeofValue = Typeof(value, stack);

            if (!IsTypeCompatible(node.Types, value, true, stack))
                throw new QlangRuntimeException(
                    $"Cannot assign value of type '{typeofValue}' to variable '{nodeName}'. Expected type: '{string.Join("|", node.Types.Select(x => Typeof(x, stack)))}'",
                    GetCurrentDebug(stack), GetStackTrace(stack));

            dynamicFunction.Variables[nodeName] = new Variable(
                nodeName,
                value,
                node.IsPrivate,
                node.IsConst,
                node.Types
            );

            dynamicFunction.Parameters.Add(nodeName);
        }
        
        // Add body and modificators
        dynamicFunction.Body.AddRange(functionNode.Body.Select(x => x.Clone()));
        dynamicFunction.IsPrivate = functionNode.IsPrivate;

        return dynamicFunction;
    }

    private DynamicClass ToQlangException(Exception ex, Stack<ASTContext> stack)
    {
        var id = _stringPoolTable.Add(QlSystemClasses.ExceptionClassName);
        var @class = ToDynamicClass(_namespaces[GlobalNamespaceName].Classes
            .FirstOrDefault(x => x.NameId == id)!, _namespaces[GlobalNamespaceName], stack);
        
        @class.Variables["message"].Value = ex.ToString();
        return @class;
    }
}