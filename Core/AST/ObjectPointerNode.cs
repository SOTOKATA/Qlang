using Newtonsoft.Json;

namespace Core.AST;

public class ObjectPointerNode(int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public string? Name;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ObjectPointerNode:
                                Name: {Name ?? "<undefined>"}
                            """,
            indent);
    }

    public override ASTNode Clone()
    {
        return new ObjectPointerNode(DebugIndex)
        {
            Name = Name
        };
    }
}