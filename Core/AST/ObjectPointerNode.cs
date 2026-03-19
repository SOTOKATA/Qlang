using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ObjectPointerNode : CallPartNode
{
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ObjectPointerNode:
                                Name: {NameId}
                                IsNullable: {IsNullable}
                            """,
            indent);
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => stringPoolTable[NameId] + (IsNullable ? "?" : "");

    public override ASTNode Clone()
    {
        return new ObjectPointerNode
        {
            NameId = NameId,
            IsNullable = IsNullable,
        };
    }
}