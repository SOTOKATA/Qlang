using Newtonsoft.Json;

namespace Core.AST;

[JsonObject(MemberSerialization.OptOut)]
// ReSharper disable once InconsistentNaming
public abstract class ASTNode(int line, int sfId)
{
    public abstract string GetTree(string indent = "");
    
    public abstract ASTNode Clone();

    [JsonProperty("l")]
    public int Line { get; set; } = line;
    [JsonProperty("s")]
    public int SourceFileId { get; set; } = sfId;
    
    protected static string DebugIndent(string text, string indent) => "\n" + string.Join("\n", text.Split('\n').Select(line => indent + line));
}