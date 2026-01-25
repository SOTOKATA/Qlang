using Newtonsoft.Json;

namespace Core.AST;

public class NamespaceNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public string Name { get; set; }

    [JsonProperty("b")]
    public List<ASTNode> Body { get; set; } = [];

    [JsonProperty("c")]
    public bool IsPrivate;

    public override ASTNode Clone()
    {
        return new NamespaceNode(Line, SourceFileId) { Name = Name, Body = Body.Select(node => node.Clone()).ToList() };
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