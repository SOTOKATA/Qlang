using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class BinaryOperationNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public ASTNode? Left { get; set; }

    [Key(2)]
    
    public string? Operator { get; set; }
    [Key(3)]
    
    public ASTNode? Right { get; set; }

    public override ASTNode Clone()
    {
        return new BinaryOperationNode(DebugIndex)
        {
            Left = Left?.Clone(),
            Operator = Operator,
            Right = Right?.Clone()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            BinaryOperationNode:
                                Operator: {Operator ?? "<undefined>"}
                                Left: {Left?.GetTree("\t\t") ??  "<undefined>"}
                                Left: {Right?.GetTree("\t\t") ??  "<undefined>"}
                            """, indent);
    }
}