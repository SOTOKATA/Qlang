using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class NumberNode : ASTNode
{
    [Key(0)]
    
    public double Value { get; set; }

    public override ASTNode Clone() => new NumberNode
    {
        Value = Value
    };

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => Value.ToString("0.00");

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NumberNode:
                                Value: {Value}
                            """, indent);
    }
}