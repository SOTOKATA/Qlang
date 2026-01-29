using MessagePack;
using Newtonsoft.Json;

namespace Core.AST;
[MessagePackObject]
public class ObjectPointerNode(int line) : ASTNode(line)
{
    [Key(1)]
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