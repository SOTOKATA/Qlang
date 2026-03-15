using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ObjectPointerNode : ASTNode
{
    [Key(0)]
    
    public int NameId;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ObjectPointerNode:
                                Name: {NameId}
                            """,
            indent);
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => stringPoolTable[NameId];

    public override ASTNode Clone()
    {
        return new ObjectPointerNode
        {
            NameId = NameId
        };
    }
}