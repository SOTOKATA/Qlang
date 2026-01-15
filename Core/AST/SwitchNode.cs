namespace Core.AST;

public class SwitchNode : ASTBlock
{
    public ASTNode Condition { get; set; }
    public List<SwitchCaseNode> CaseBlocks { get; set; } = [];
    
    public List<ASTNode>? DefaultBlock { get; set; }

    public override ASTNode Clone()
    {
        return new SwitchNode
        {
            Condition = Condition.Clone(), 
            CaseBlocks = CaseBlocks.Select(node => node.Clone()).Cast<SwitchCaseNode>().ToList(),
            DefaultBlock =  DefaultBlock?.Select(node => node.Clone()).ToList(),
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(SwitchNode), [Condition, CaseBlocks], indent);
    }
}