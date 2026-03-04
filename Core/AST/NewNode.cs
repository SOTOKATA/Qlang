using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class NewNode : ASTNode
{
    [Key(0)]
    public required CallNode NodePath { get; init; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent(
            $"""
            NewNode:
                NodePath: {NodePath.GetTree("\t")}
            """,
            indent);
    }

    public override ASTNode Clone() =>  new NewNode
        {
            NodePath = (CallNode)NodePath.Clone()
        };
}