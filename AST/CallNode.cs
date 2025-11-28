namespace Qlang.AST;

public class CallNode : ASTNode
{
    // its: 'Term', 'read()', 'trim()'
    public List<ASTNode> Objects = [];
    public string ObjectName { get; set; }  // "Term"
    public string MethodName { get; set; }  // "print"
    public List<ASTNode> Arguments { get; set; } = [];

    public override ASTNode Clone()
    {
        return new CallNode
        {
            ObjectName = ObjectName,
            MethodName = MethodName,
            Arguments = Arguments.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(CallNode), [ObjectName, MethodName, Arguments], indent);
    }
}