using Newtonsoft.Json;

namespace Core.AST;

public class NumberRefNode(int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public bool IsNegative { get; set; }
    [JsonProperty("b")]
    public int Index { get; set; }

    public override ASTNode Clone() => new NumberRefNode(DebugIndex)
    {
        Index = Index, 
        IsNegative = IsNegative
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NumberRefNode:
                                Ref: {Index}
                                IsNegative: {IsNegative}
                            """, indent);
    }
}