using MessagePack;
using Newtonsoft.Json;

namespace Core.AST;
[MessagePackObject]
public class IfNode(int line) : ASTBlock(line)
{
    public IfNode() : this(-1) {}
    
    [Key(1)]
    [JsonProperty("a")]
    public required ASTNode Condition { get; set; }
    [Key(2)]
    [JsonProperty("b")]
    public List<ASTNode> ThenBlock { get; set; } = [];
    [Key(3)]
    [JsonProperty("c")]
    public List<ASTNode> ElseBlock { get; set; } = [];

    public override ASTNode Clone()
    {
        return new IfNode(DebugIndex)
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