using Newtonsoft.Json;

namespace Core.AST;

public class SwitchNode(int line, int sfId) : ASTBlock(line, sfId)
{
    [JsonProperty("a")]
    public ASTNode Condition { get; set; }
    [JsonProperty("b")]
    public List<SwitchCaseNode> CaseBlocks { get; set; } = [];
    
    [JsonProperty("c")]
    public List<ASTNode>? DefaultBlock { get; set; }

    public override ASTNode Clone()
    {
        return new SwitchNode(Line, SourceFileId)
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