using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class ParallelNode : ASTNode
{
    [Key(0)]
    public ASTNode Object; 
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                           ParallelNode:
                                Objects: {string.Join("\n", Object.GetTree("\t"))}
                           """, indent);
    }

    public override ASTNode Clone()
    {
        return new ParallelNode
        {
            Object = Object.Clone()
        };
    }
}