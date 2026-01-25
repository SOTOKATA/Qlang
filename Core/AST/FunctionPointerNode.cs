using Newtonsoft.Json;

namespace Core.AST;

public class FunctionPointerNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public string Name;
    [JsonProperty("b")]
    public List<ASTNode> Arguments;
    
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
        return new FunctionPointerNode(Line, SourceFileId)
        {
            Name = Name,
            Arguments = [..Arguments.ConvertAll(arg => arg.Clone())]
        };
    }
}