namespace Qlang.AST;

public class IfNode : ASTBlock
{
    public ASTNode Condition { get; set; }
    public List<ASTNode> ThenBlock { get; set; } = [];
    public List<ASTNode> ElseBlock { get; set; } = [];

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(IfNode), [Condition, ThenBlock, ElseBlock], indent);
    }
}