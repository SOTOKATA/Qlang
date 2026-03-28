using Core.Tables;
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class TypeEqualityNode : ASTNode
{
    [Key(0)]
    public ASTNode Left;
    
    [Key(1)]
    public CallNode Class;
    
    [Key(2)]
    public bool IsNotEqual;

    public override string GetTree(string indent = "")
    {
        return "";
    }

    public override ASTNode Clone()
    {
        return new TypeEqualityNode
        {
            Left = Left.Clone(),
            Class = (CallNode)Class.Clone(),
            IsNotEqual = IsNotEqual
        };
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
    {
        return Left.ToTokenString(stringPoolTable) + $" {(IsNotEqual ? "is" : "is not")} " + Class.ToTokenString(stringPoolTable);
    }
}