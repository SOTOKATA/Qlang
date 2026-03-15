using Core.Tables;
using MessagePack;

namespace Core.AST;
[MessagePackObject]
public class UsingNode : ASTNode
{
    [Key(0)]
    public required CallNode CallPath { get; set; }

    [IgnoreMember] 
    public int FileId = -1;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            UsingNode:
                                CallPath: {CallPath.GetTree("\t")}
                            """, indent);
    }
    
    public override string ToTokenString(StringPoolTable stringPoolTable)
        => "using " + CallPath.ToTokenString(stringPoolTable);

    public override ASTNode Clone()
    {
        return new UsingNode
        {
            CallPath = CallPath
        };
    }
}