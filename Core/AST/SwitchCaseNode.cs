using Newtonsoft.Json;

namespace Core.AST;

public class SwitchCaseNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public ASTNode Condition { get; set; }
    [JsonProperty("b")]
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
        return new SwitchCaseNode(Line,  SourceFileId)
        {
            Condition = Condition.Clone(),
            CaseBlock = CaseBlock.Select(node => node.Clone()).ToList()
        };
    }
}