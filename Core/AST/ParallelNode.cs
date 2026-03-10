using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class ParallelNode : ASTNode
{
    [Key(0)]
    public List<CallNode> Objects = []; 
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                           ParallelNode:
                                Objects: {string.Join("\n", Objects.Select(o => o.GetTree("\t")).ToArray())}
                           """, indent);
    }

    public override ASTNode Clone()
    {
        return new ParallelNode
        {
            Objects = Objects.Select(x => (CallNode)x.Clone()).ToList()
        };
    }
}