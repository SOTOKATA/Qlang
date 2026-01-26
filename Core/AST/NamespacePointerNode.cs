using Newtonsoft.Json;

namespace Core.AST;

public class NamespacePointerNode(string name, int line) : ASTNode(line)
{
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