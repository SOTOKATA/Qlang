using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class NamespacePointerNode : ASTNode
{
    [Key(0)]
    
    public int NameId { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NamespacePointerNode:
                                Name: {NameId}.
                            """, indent);
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => $"{stringPoolTable[NameId]}";

    public override ASTNode Clone()
    {
        return new NamespacePointerNode
        {
            NameId = NameId,
        };
    }
}