using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class CallNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public List<ASTNode> Objects = [];
    [Key(2)]
    
    public List<ASTNode> Arguments { get; set; } = [];

    public override ASTNode Clone()
    {
        return new CallNode(DebugIndex)
        {
            Arguments = Arguments.Select(node => node.Clone()).ToList(),
            Objects = Objects.Select(node => node.Clone()).ToList()
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