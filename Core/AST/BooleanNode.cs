using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class BooleanNode : ASTNode
{
    [Key(0)] 
    public bool Value { get; set; }

    public override ASTNode Clone() => new BooleanNode { 
            Value = Value
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            BreakNode:
                                Value: {Value}
                            """, indent);
    }
}