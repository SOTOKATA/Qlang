using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class SwitchCaseNode : ASTNode
{
    [Key(0)]
    
    public required ASTNode Condition { get; set; }
    [Key(1)]
    
    public List<ASTNode> CaseBlock { get; set; } = [];
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            SwitchCaseNode:
                                Condition: {Condition.GetTree("\t")}
                                CaseBlock: [{string.Join(",\n", CaseBlock.Select(x => x.GetTree("\t")))}]
                            """,
            indent
            );
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => "Switch";

    public override ASTNode Clone()
    {
        return new SwitchCaseNode
        {
            Condition = Condition.Clone(),
            CaseBlock = CaseBlock.Select(node => node.Clone()).ToList()
        };
    }
}