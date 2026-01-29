using MessagePack;

namespace Core.AST;
[MessagePackObject]
public class UsingNode(int line) : ASTNode(line)
{
    [Key(1)]
    public required CallNode CallPath { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            UsingNode:
                                CallPath: {CallPath.GetTree("\t\t")}
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new UsingNode(DebugIndex)
        {
            CallPath = CallPath
        };
    }
}