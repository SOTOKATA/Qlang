using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class BooleanNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public bool Value { get; set; }

    public override ASTNode Clone() => new BooleanNode(DebugIndex) { 
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