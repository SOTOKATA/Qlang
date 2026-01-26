using Newtonsoft.Json;

namespace Core.AST;

public class KeywordNode(string keyword, int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public string Value { get; set; } = keyword;
    
    public override string GetTree(string indent = "")
    {
        return "";
    }

    public override ASTNode Clone()
    {
        return new KeywordNode(Value,  DebugIndex);
    }
}