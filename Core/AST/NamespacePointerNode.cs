using MessagePack;
using Newtonsoft.Json;

namespace Core.AST;
[MessagePackObject]
public class NamespacePointerNode(string name, int line) : ASTNode(line)
{
    public NamespacePointerNode() : this("", -1)
    {}
    
    [Key(1)]
    [JsonProperty("a")]
    public string Name { get; set; } = name;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NamespacePointerNode:
                                Name: {Name}
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new NamespacePointerNode(Name, DebugIndex);
    }
}