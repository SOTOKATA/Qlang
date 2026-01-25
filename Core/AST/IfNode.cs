using Newtonsoft.Json;

namespace Core.AST;

public class IfNode(int line, int sfId) : ASTBlock(line, sfId)
{
    [JsonProperty("a")]
    public ASTNode Condition { get; set; }
    [JsonProperty("b")]
    public List<ASTNode> ThenBlock { get; set; } = [];
    [JsonProperty("c")]
    public List<ASTNode> ElseBlock { get; set; } = [];

    public override ASTNode Clone()
    {
        return new IfNode(Line,  SourceFileId)
        {
            Condition = Condition.Clone(), 
            ThenBlock = ThenBlock.Select(node => node.Clone()).ToList(), 
            ElseBlock = ElseBlock.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            IfNode:
                                Condition: {Condition.GetTree("\t\t")}
                                ThenBlock: [{string.Join(",\n", ThenBlock.Select(x => x.GetTree("\t\t")))}]
                                ElseBlock: [{string.Join(",\n", ElseBlock.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}