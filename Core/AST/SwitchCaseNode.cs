namespace Core.AST;

public class SwitchCaseNode : ASTNode
{
    public ASTNode Condition { get; set; }
    public List<ASTNode> CaseBlock { get; set; } = [];
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            SwitchCaseNode:
                                Condition: {Condition.GetTree("\t\t")}
                                CaseBlock: [{string.Join(",\n", CaseBlock.Select(x => x.GetTree("\t\t")))}]
                            """,
            indent
            );
    }

    public override ASTNode Clone()
    {
        return new SwitchCaseNode
        {
            Condition = Condition.Clone(),
            CaseBlock = CaseBlock.Select(node => node.Clone()).ToList()
        };
    }
}