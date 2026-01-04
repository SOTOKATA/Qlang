namespace Core.AST;

public class SwitchNode : ASTBlock
{
    public ASTNode Condition { get; set; }
    public Dictionary<ASTNode, List<ASTNode>> CaseBlocks { get; set; } = [];
    
    public List<ASTNode>? DefaultBlock { get; set; }

    public override ASTNode Clone()
    {
        return new SwitchNode
        {
            Condition = Condition.Clone(), 
            CaseBlocks = CaseBlocks.ToDictionary(caseBlock => caseBlock.Key.Clone(), caseBlock => caseBlock.Value.Select(node => node.Clone()).ToList()),
            DefaultBlock =  DefaultBlock?.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(SwitchNode), [Condition, CaseBlocks], indent);
    }
}