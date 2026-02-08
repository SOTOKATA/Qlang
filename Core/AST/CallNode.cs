using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class CallNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public List<ASTNode> Objects = [];

    public override ASTNode Clone()
    {
        return new CallNode(DebugIndex)
        {
            Objects = Objects.Select(node => node.Clone()).ToList(),
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            CallNode:
                                DebugIndex: {DebugIndex}
                                Objects: [{string.Join(" ,\n", Objects.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}