
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class TryCatchNode : ASTBlock
{
    [Key(0)] public List<ASTNode> TryBody { get; set; } = [];
    
    [Key(1)]
    public required LineNode CatchAssignment { get; set; }
    
    [Key(2)] public List<ASTNode> CatchBody { get; set; } = [];
    
    [Key(3)] public List<ASTNode> FinallyBody { get; set; } = [];

    public override ASTNode Clone()
    {
        return new TryCatchNode
        {
            TryBody = TryBody.Select(x => x.Clone()).ToList(),
            CatchBody = CatchBody.Select(x => x.Clone()).ToList(),
            FinallyBody = FinallyBody.Select(x => x.Clone()).ToList(),
            CatchAssignment = (LineNode)CatchAssignment.Clone(),
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            TryCatchNode:
                                TryCatch: {string.Join(", ", TryBody.Select(x => x.GetTree("\t")))}
                                CatchBody: {string.Join(", ", CatchBody.Select(x => x.GetTree("\t")))}
                                FinallyBody: {string.Join(", ", FinallyBody.Select(x => x.GetTree("\t")))}
                            """, indent);
    }
}