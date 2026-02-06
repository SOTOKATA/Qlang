using Core.AST;
using Core.Dynamic;

namespace Interpreter;

public partial class Interpreter
{
    /// <summary>
    /// Convert static namespace to dynamic
    /// </summary>
    /// <param name="namespaceNode">namespace to convert</param>
    /// <returns>DynamicNamespace</returns>
    private DynamicNamespace ToDynamicNamespace(NamespaceNode namespaceNode, DynamicNamespace? dNamespace = null)
    {
        // Create dynamic instance
        var dynamicNamespace = new DynamicNamespace(namespaceNode.Name)
        {
            IsPrivate = namespaceNode.IsPrivate
        };
        
        dNamespace?.Namespaces.Add(dynamicNamespace);
        
        // Add and convert all classes
        dynamicNamespace.Classes.AddRange(
            namespaceNode.Body
                .OfType<ClassNode>()
                .Select(x => ToDynamicClass(x, dNamespace))
        );
        
        // Add and convert all namespaces
        dynamicNamespace.Namespaces.AddRange(
            namespaceNode.Body.OfType<NamespaceNode>()
                .Select(x => ToDynamicNamespace(x, dNamespace))
            );
        

        // Add all functions
        dynamicNamespace.Functions.AddRange(namespaceNode.Body.OfType<FunctionNode>());
        
        // Add and convert all assignments
        foreach (var assignmentNode in namespaceNode.Body.OfType<AssignmentNode>())
                dynamicNamespace.Variables[assignmentNode.GetLastName()] = new Variable(assignmentNode.GetLastName(),
                    EvaluateExpression(assignmentNode.Value), assignmentNode.IsStatic, assignmentNode
                        .IsPrivate, assignmentNode.IsConst);
        
        return dynamicNamespace;
    }

    /// <summary>
    /// Convert static class to dynamic
    /// </summary>
    /// <param name="classNode">class to convert</param>
    /// <param name="dynamicNamespace"></param>
    /// <returns>DynamicClass</returns>
    private DynamicClass ToDynamicClass(ClassNode classNode, DynamicNamespace? dynamicNamespace = null)
    {
        // Create dynamic instance
        DynamicClass dynamicClass = new(classNode.Name);
        dynamicNamespace?.Classes.Add(dynamicClass);

        // Add and convert all assignments
        foreach (var assignmentNode in classNode.Body.OfType<AssignmentNode>())
                dynamicClass.Variables[assignmentNode.GetLastName()] = new Variable(assignmentNode.GetLastName(),
                    EvaluateExpression(assignmentNode.Value), assignmentNode.IsStatic, assignmentNode
                    .IsPrivate, assignmentNode.IsConst);
        
        // Remove all AssignmentNodes from body
        classNode.Body.RemoveAll(node => node is AssignmentNode);

        dynamicClass.Body = classNode.Body;

        return dynamicClass;
    }

    /// <summary>
    /// Convert static function to dynamic
    /// </summary>
    /// <param name="functionNode">function to convert</param>
    /// <returns>DynamicFunction</returns>
    private DynamicFunction ToDynamicFunction(FunctionNode functionNode)
    {
        // Create dynamic instance
        DynamicFunction dynamicFunction = new(functionNode.Name)
        {
            Context = functionNode.Context
        };

        // Add and convert all parameters
        foreach (var node in functionNode.Parameters)
        {
            dynamicFunction.Variables[node.GetLastName()] = new Variable(
                node.GetLastName(),
                EvaluateExpression(node.Value),
                node.IsStatic,
                node.IsPrivate,
                node.IsConst,
                node.Type);

            dynamicFunction.Parameters.Add(node.GetLastName());
        }

        // Add body and modificators
        dynamicFunction.Body = functionNode.Body;
        dynamicFunction.IsStatic = functionNode.IsStatic;
        dynamicFunction.IsPrivate = functionNode.IsPrivate;

        return dynamicFunction;
    }
}