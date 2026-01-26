using Newtonsoft.Json;

namespace Core.AST;

public class AssignmentNode(bool isStatic, bool isPrivate, bool isConst, bool isNew, int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public bool IsNew { get; set; } = isNew;

    [JsonProperty("b")]
    public required List<ASTNode> Path { get; set; }

    [JsonProperty("c")]
    public ASTNode? Value { get; set; }
    
    [JsonProperty("d")]
    public CallNode? Type { get; set; }

    [JsonProperty("e")]
    public bool IsStatic { get; set; } = isStatic;

    [JsonProperty("f")]
    public bool IsPrivate { get; set; } = isPrivate;

    [JsonProperty("g")]
    public bool IsConst { get; set; } = isConst;

    public string GetLastName() => (Path[^1] as ObjectPointerNode)!.Name!;

    public override ASTNode Clone()
    {
        return new AssignmentNode(IsStatic, IsPrivate, IsConst, IsNew, DebugIndex)
        {
            Path = Path.Select(node => node.Clone()).ToList(),
            Value = Value?.Clone(),
            Type = Type
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            VariableNode:
                                Path: [{string.Join(",\n", Path.Select(x => x.GetTree("\t\t")))}]
                                Value: {Value?.GetTree("\t\t") ?? "<undefined>"}
                                Type: {Type?.GetTree("\t\t")}
                                IsStatic: {IsStatic}
                                IsPrivate: {IsPrivate}
                                IsConst: {IsConst}
                                IsNew: {IsNew}
                            """, indent);
    }
}
