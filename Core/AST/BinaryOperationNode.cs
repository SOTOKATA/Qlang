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
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(BinaryOperationNode), 
            [Left, Operator, Right]
            , indent);
    }
}