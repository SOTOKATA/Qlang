using Newtonsoft.Json;

namespace Core.AST;

public class CallNode(int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public List<ASTNode> Objects = [];
    [JsonProperty("b")]
    public List<ASTNode> Arguments { get; set; } = [];

    public override ASTNode Clone()
    {
        return new CallNode(DebugIndex)
        {
            Arguments = Arguments.Select(node => node.Clone()).ToList(),
            Objects = Objects.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            CallNode:
                                Objects: [{string.Join(",\n", Objects.Select(x => x.GetTree("\t\t")))}]
                                Arguments: [{string.Join(",\n", Arguments.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}