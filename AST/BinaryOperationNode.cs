namespace Qlang.AST;

public class BinaryOperationNode : ASTNode
{
    public ASTNode Left { get; set; }
    public string Operator { get; set; }  // "==", "+", "-", etc.
    public ASTNode Right { get; set; }

    public override string GetTree(string indent = "")
    {
        return $"{indent}{Left?.GetTree(indent)}{Operator} {Right?.GetTree(indent)}";
    }
}