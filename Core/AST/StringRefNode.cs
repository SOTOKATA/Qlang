using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class StringRefNode : ASTNode
{
    [Key(0)]
    
    public int Index { get; set; }

    public override ASTNode Clone() => new StringRefNode
    {
        Index = Index
    };

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => $"{stringPoolTable[Index]}";

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            StringRefNode:
                                Ref: {Index}
                            """, indent);
    }
}