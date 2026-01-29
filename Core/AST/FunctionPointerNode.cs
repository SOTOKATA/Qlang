using MessagePack;
using Newtonsoft.Json;

namespace Core.AST;
[MessagePackObject]
public class FunctionPointerNode(int line) : ASTNode(line)
{
    [Key(1)]
    [JsonProperty("a")]
    public required string Name;
    [Key(2)]
    [JsonProperty("b")]
    public required List<ASTNode> Arguments;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            FunctionPointerNode:
                                Name: {Name}
                                Arguments: [{string.Join(",\n", Arguments.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new FunctionPointerNode(DebugIndex)
        {
            Name = Name,
            Arguments = [..Arguments.ConvertAll(arg => arg.Clone())]
        };
    }
}