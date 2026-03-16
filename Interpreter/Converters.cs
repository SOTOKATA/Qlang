using Core;
using Core.AST;
using Core.Dynamic;

namespace Interpreter;

public partial class Interpreter
{
    /// <summary>
    /// Convert variables with ASTNode values to normalized
    /// </summary>
    /// <param name="variables">Variables to convert</param>
    /// <returns>Converted variables</returns>
    private Dictionary<string, Variable> ToDynamicVariables(Dictionary<string, Variable> variables, Stack<ASTContext> stack)
    {
        foreach (var pair in variables)
            if (pair.Value.Value is ASTNode node)
                pair.Value.Value = EvaluateExpression(node, stack);

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
    /// <param name="globalNamespaceName">name of subnamespace</param>
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
                        .IsPrivate, assignmentNode.IsConst);
        
        return dynamicNamespace;
    }

    /// <summary>
    /// Convert static class to dynamic
    /// </summary>
    /// <param name="classNode">class to convert</param>
    /// <param name="stack">Current context stack</param>
    /// <returns>DynamicClass</returns>
    private DynamicClass ToDynamicClass(ClassNode classNode, Stack<ASTContext> stack)
    {
        // Create dynamic instance
        DynamicClass dynamicClass = new(_stringPoolTable[classNode.NameId])
        {
            IsPrivate = classNode.IsPrivate
        };
        
        // Add and convert all assignments
        foreach (var assignmentNode in classNode.Body.OfType<LineNode>().Select(x => (AssignmentNode)x.Content!))
                dynamicClass.Variables[_stringPoolTable[assignmentNode.GetLastNameId()]] = new Variable(_stringPoolTable[assignmentNode.GetLastNameId()],
                    EvaluateExpression(assignmentNode.Value, stack),  assignmentNode
                    .IsPrivate, assignmentNode.IsConst);

        // Remove all AssignmentNodes from body
        classNode = (classNode.Clone() as ClassNode)!;
        classNode.Body.RemoveAll(node => node is LineNode);

        dynamicClass.Extends.AddRange(classNode.Extends.Select(x => _stringPoolTable[x]));
        
        dynamicClass.Body = classNode.Body;
        
        return dynamicClass;
    }

    /// <summary>
    /// Convert static function to dynamic
    /// </summary>
    /// <param name="functionNode">function to convert</param>
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
            dynamicFunction.Variables[nodeName] = new Variable(
                nodeName,
                EvaluateExpression(node.Value, stack),
                node.IsPrivate,
                node.IsConst,
                node.Types);

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
            .FirstOrDefault(x => x.NameId == id)!, stack);
        
        @class.Variables["message"].Value = ex.ToString();
        return @class;
    }
}