using Newtonsoft.Json;

namespace Core.AST;

public class NumberNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public double Value { get; set; }

    public override ASTNode Clone() => new NumberNode(line,  SourceFileId)
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