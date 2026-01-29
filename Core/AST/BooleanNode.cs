using MessagePack;
using Newtonsoft.Json;

namespace Core.AST;

[MessagePackObject]
public class BooleanNode(int line) : ASTNode(line)
{
    [Key(1)]
    [JsonProperty("a")]
    public bool Value { get; set; }

    public override ASTNode Clone() => new BooleanNode(DebugIndex) { 
            Value = Value
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            BreakNode:
                                Value: {Value}
                            """, indent);
    }
}