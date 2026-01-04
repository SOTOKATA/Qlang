using Core.AST;
using Core.Exceptions;

namespace Compiler;

public class PostParser
{
    public static ProgramNode IncludeExtends(ProgramNode program)
    {
        var classNodes = program.Statements
            .OfType<ClassNode>()
            .ToList();

        foreach (var cls in classNodes)
        {
            ResolveClass(
                cls,
                classNodes,
                []
            );
        }

        return program;
    }
    
    private static string GetNodeKey(ASTNode node)
    {
        return node switch
        {
            FunctionNode fn => $"fn:{fn.Name}",
            AssignmentNode an when an.VariableName != "" => $"var:{an.VariableName}",
            AssignmentNode an => $"var:{an.Path}",
            CallNode call =>$"call:{string.Join(".", call.Objects)}",
            _ => throw new Exception($"Unsupported AST node: {node.GetType().Name}")
        };
    }
    
    private static void ResolveClass(
        ClassNode cls,
        List<ClassNode> allClasses,
        HashSet<string> resolving)
    {
        if (cls.Extends == "")
            return;

        if (!resolving.Add(cls.Name))
            throw new QlangCompileException(
                $"Cyclic inheritance detected: {cls.Name}",
                cls.Line,
                "PostParser",
                cls.SourceFile);

        var parent = allClasses.FirstOrDefault(c => c.Name == cls.Extends);

        if (parent == null)
            throw new QlangCompileException(
                $"Extended class '{cls.Extends}' is not found",
                cls.Line,
                "PostParser",
                cls.SourceFile);

        ResolveClass(parent, allClasses, resolving);

        cls.Body = MergeBodies(parent.Body, cls.Body);

        cls.Extends = "";

        resolving.Remove(cls.Name);
    }

    
    private static List<ASTNode> MergeBodies(
        List<ASTNode> parentBody,
        List<ASTNode> childBody)
    {
        var map = new Dictionary<string, ASTNode>();

        foreach (var node in parentBody)
            map[GetNodeKey(node)] = node;

        foreach (var node in childBody)
            map[GetNodeKey(node)] = node;

        return map.Values.ToList();
    }
}