using Newtonsoft.Json;

namespace Core.AST;

public class BinaryOperationNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public ASTNode? Left { get; set; }

    [JsonProperty("b")]
    public string? Operator { get; set; } // "==", "+", "-", etc.
    [JsonProperty("c")]
    public ASTNode? Right { get; set; }

    public override ASTNode Clone()
    {
        return new BinaryOperationNode(Line, SourceFileId)
        {
            Left = Left?.Clone(),
            Operator = Operator,
            Right = Right?.Clone()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            BinaryOperationNode:
                                Operator: {Operator ?? "<undefined>"}
                                Left: {Left?.GetTree("\t\t") ??  "<undefined>"}
                                Left: {Right?.GetTree("\t\t") ??  "<undefined>"}
                            """, indent);
    }
}