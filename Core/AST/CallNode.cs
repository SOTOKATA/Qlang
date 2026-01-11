namespace Core.AST;

public class CallNode : ASTNode
{
    public List<ASTNode> Objects = [];
    public List<ASTNode> Arguments { get; set; } = [];

    public override ASTNode Clone()
    {
        return new CallNode
        {
            Arguments = Arguments.Select(node => node.Clone()).ToList(),
            Objects = Objects.Select(node => node.Clone()).ToList(),
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(CallNode), [Objects, Arguments], indent);
    }
}