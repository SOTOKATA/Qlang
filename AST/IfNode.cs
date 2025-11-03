namespace Qlang.AST;

public class IfNode : ASTNode
{
    public ASTNode Condition { get; set; }
    public List<ASTNode> ThenBlock { get; set; } = [];
    public List<ASTNode> ElseBlock { get; set; } = [];
}