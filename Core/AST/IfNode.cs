namespace Core.AST;

public class IfNode : ASTBlock
{
    public ASTNode Condition { get; set; }
    public List<ASTNode> ThenBlock { get; set; } = [];
    public List<ASTNode> ElseBlock { get; set; } = [];

    public override ASTNode Clone()
    {
        return new IfNode
        {
            Condition = Condition.Clone(), 
            ThenBlock = ThenBlock.Select(node => node.Clone()).ToList(), 
            ElseBlock = ElseBlock.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(IfNode), [Condition, ThenBlock, ElseBlock], indent);
    }
}