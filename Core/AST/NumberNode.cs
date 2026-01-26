using Newtonsoft.Json;

namespace Core.AST;

public class NumberNode(int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public double Value { get; set; }

    public override ASTNode Clone() => new NumberNode(DebugIndex)
    {
        Value = Value
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NumberNode:
                                Value: {Value}
                            """, indent);
    }
}