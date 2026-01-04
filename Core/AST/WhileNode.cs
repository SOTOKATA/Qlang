namespace Core.AST;

public class WhileNode : ASTBlock
{
    public ASTNode? Condition { get; set; }

    public List<ASTNode> Body { get; set; } = [];
    
    public bool IsDoWhile { get; set; } = false;

    public override ASTNode Clone()
    {
        return new WhileNode
        {
            Condition = Condition?.Clone(), 
            Body = Body.Select(node => node.Clone()).ToList(),
            IsDoWhile = IsDoWhile
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(WhileNode), [Condition, Body, IsDoWhile], indent);
    }
}