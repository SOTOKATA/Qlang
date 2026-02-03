using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class SwitchNode(int line) : ASTBlock(line)
{
    public SwitchNode() : this(-1) {}
    [Key(1)]
    
    public required ASTNode Condition { get; set; }
    [Key(2)]
    
    public List<SwitchCaseNode> CaseBlocks { get; set; } = [];
    
    [Key(3)]
    
    public List<ASTNode>? DefaultBlock { get; set; }

    public override ASTNode Clone()
    {
        return new SwitchNode(DebugIndex)
        {
            Condition = Condition.Clone(), 
            CaseBlocks = CaseBlocks.Select(node => node.Clone()).Cast<SwitchCaseNode>().ToList(),
            DefaultBlock =  DefaultBlock?.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            SwitchNode:
                                Condition: {Condition.GetTree("\t\t")}
                                DefaultBlock: [{string.Join(",\n", DefaultBlock?.Select(x => x.GetTree("\t\t")) ?? ["<not_exists>"])}]
                                CaseBlocks: [{string.Join(",\n", CaseBlocks.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}