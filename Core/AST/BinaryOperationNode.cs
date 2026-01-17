namespace Core.AST;

public class BinaryOperationNode : ASTNode
{
    public ASTNode? Left { get; set; }
    public string? Operator { get; set; }  // "==", "+", "-", etc.
    public ASTNode? Right { get; set; }

    public override ASTNode Clone()
    {
        return new BinaryOperationNode
        {
            Left = Left?.Clone(),
            Operator = Operator,
            Right = Right?.Clone(), 
            SourceFile =  SourceFile, 
            Line =  Line 
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