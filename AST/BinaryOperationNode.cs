namespace Qlang.AST;

public class BinaryOperationNode : ASTNode
{
    public ASTNode Left { get; set; }
    public string Operator { get; set; }  // "==", "+", "-", etc.
    public ASTNode Right { get; set; }
}