namespace Qlang.AST;

public class MethodCallNode : ASTNode
{
    public string ObjectName { get; set; }  // "Term"
    public string MethodName { get; set; }  // "print"
    public List<ASTNode> Arguments { get; set; } = [];

    public override ASTNode Clone()
    {
        return new MethodCallNode
        {
            ObjectName = ObjectName,
            MethodName = MethodName,
            Arguments = Arguments.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(MethodCallNode), [ObjectName, MethodName, Arguments], indent);
    }
}