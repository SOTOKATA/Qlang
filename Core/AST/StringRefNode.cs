using Newtonsoft.Json;

namespace Core.AST;

public class StringRefNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public int Index { get; set; }

    public override ASTNode Clone() => new StringRefNode(line, SourceFileId)
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