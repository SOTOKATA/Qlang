namespace Core.AST;

public class SwitchCaseNode : ASTNode
{
    public ASTNode? Condition { get; set; }
    public List<ASTNode> CaseBlock { get; set; } = [];
    
    public override string GetTree(string indent = "")
    {
        return "";
    }

    public override ASTNode Clone()
    {
        return new SwitchCaseNode
        {
            Condition = Condition?.Clone(),
            CaseBlock = CaseBlock.Select(node => node.Clone()).ToList()
        };
    }
}