using MessagePack;
using Newtonsoft.Json;

namespace Core.AST;
[MessagePackObject]
public class NamespaceNode(int line) : ASTNode(line)
{
    [Key(1)]
    [JsonProperty("a")]
    public required string Name { get; set; }

    [Key(2)]
    [JsonProperty("b")]
    public List<ASTNode> Body { get; set; } = [];

    [Key(3)]
    [JsonProperty("c")]
    public bool IsPrivate;

    public override ASTNode Clone()
    {
        return new NamespaceNode(DebugIndex) { Name = Name, Body = Body.Select(node => node.Clone()).ToList() };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NamespaceNode:
                                Name: {Name}
                                IsPrivate: {IsPrivate}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}