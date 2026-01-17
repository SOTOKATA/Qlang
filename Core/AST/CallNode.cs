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
        return DebugIndent($"""
                            CallNode:
                                Objects: [{string.Join(",\n", Objects.Select(x => x.GetTree("\t\t")))}]
                                Arguments: [{string.Join(",\n", Arguments.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}