using Newtonsoft.Json;

namespace Core.AST;

public class BinaryOperationNode(int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public ASTNode? Left { get; set; }

    [JsonProperty("b")]
    public string? Operator { get; set; } // "==", "+", "-", etc.
    [JsonProperty("c")]
    public ASTNode? Right { get; set; }

    public override ASTNode Clone()
    {
        return new BinaryOperationNode(DebugIndex)
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