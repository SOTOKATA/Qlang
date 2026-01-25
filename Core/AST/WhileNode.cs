using Newtonsoft.Json;

namespace Core.AST;

public class WhileNode(int line, int sfId) : ASTBlock(line, sfId)
{
    [JsonProperty("a")]
    public ASTNode Condition { get; set; }

    [JsonProperty("b")]
    public List<ASTNode> Body { get; set; } = [];
    
    [JsonProperty("c")]
    public bool IsDoWhile { get; set; }

    public override ASTNode Clone()
    {
        return new WhileNode(Line,  SourceFileId)
        {
            Condition = Condition.Clone(), 
            Body = Body.Select(node => node.Clone()).ToList(),
            IsDoWhile = IsDoWhile
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            WhileNode:
                                Condition: {Condition.GetTree("\t\t")}
                                IsDoWhile: {IsDoWhile}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}