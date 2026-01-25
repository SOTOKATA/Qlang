using Newtonsoft.Json;

namespace Core.AST;

public class BooleanNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public bool Value { get; set; }

    public override ASTNode Clone() => new BooleanNode(Line, SourceFileId) { 
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