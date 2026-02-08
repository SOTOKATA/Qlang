using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class SwitchCaseNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public required ASTNode Condition { get; set; }
    [Key(2)]
    
    public List<ASTNode> CaseBlock { get; set; } = [];
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            SwitchCaseNode:
                                DebugIndex: {DebugIndex}
                                Condition: {Condition.GetTree("\t\t")}
                                CaseBlock: [{string.Join(",\n", CaseBlock.Select(x => x.GetTree("\t\t")))}]
                            """,
            indent
            );
    }

    public override ASTNode Clone()
    {
        return new SwitchCaseNode(DebugIndex)
        {
            Condition = Condition.Clone(),
            CaseBlock = CaseBlock.Select(node => node.Clone()).ToList()
        };
    }
}