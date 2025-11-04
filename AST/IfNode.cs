namespace Qlang.AST;

public class IfNode : ASTNode
{
    public ASTNode Condition { get; set; }
    public List<ASTNode> ThenBlock { get; set; } = [];
    public List<ASTNode> ElseBlock { get; set; } = [];

    public override string GetTree(string indent = "")
    {
        return $"{indent}Condition: {Condition} {ThenBlock.Select(block => block.GetTree(indent))} {ElseBlock.Select(block => block.GetTree(indent))}";
    }
}