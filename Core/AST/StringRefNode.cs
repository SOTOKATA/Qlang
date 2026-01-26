using Newtonsoft.Json;

namespace Core.AST;

public class StringRefNode(int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public int Index { get; set; }

    public override ASTNode Clone() => new StringRefNode(DebugIndex)
    {
        Index = Index
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            StringRefNode:
                                Ref: {Index}
                            """, indent);
    }
}