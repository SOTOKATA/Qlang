using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class NumberNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public double Value { get; set; }

    public override ASTNode Clone() => new NumberNode(DebugIndex)
    {
        Value = Value
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NumberNode:
                                DebugIndex: {DebugIndex}
                                Value: {Value}
                            """, indent);
    }
}