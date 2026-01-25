using Newtonsoft.Json;

namespace Core.AST;

public class ForNode(int line, int sfId) : ASTBlock(line, sfId)
{
    [JsonProperty("a")]
    public AssignmentNode Assignment { get; set; }
    [JsonProperty("b")]
    public ASTNode Condition { get; set; }
    [JsonProperty("c")]
    public ASTNode Statement { get; set; }

    [JsonProperty("d")]
    public List<ASTNode> Body { get; set; } = [];

    public override ASTNode Clone()
    {
        return new ForNode(Line, SourceFileId) { 
            Assignment = (Assignment.Clone() as AssignmentNode)!,
            Statement = Statement.Clone(),
            Condition = Condition, 
            Body = Body.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ForNode:
                                Assignment: {Assignment.GetTree("\t\t")}
                                Condition: {Condition.GetTree("\t\t")}
                                Statement: {Condition.GetTree("\t\t")}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}