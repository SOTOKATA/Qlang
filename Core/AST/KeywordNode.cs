using MessagePack;
using Newtonsoft.Json;

namespace Core.AST;
[MessagePackObject]
public class KeywordNode(string keyword, int line) : ASTNode(line)
{
    public KeywordNode() : this("", -1)
    {}
    
    [Key(1)]
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