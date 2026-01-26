using Newtonsoft.Json;

namespace Core.AST;

[JsonObject(MemberSerialization.OptOut)]
// ReSharper disable once InconsistentNaming
public abstract class ASTNode(int debugIndex)
{
    [JsonProperty("z")] 
    public int DebugIndex { get; set; } = debugIndex;
    
    public abstract string GetTree(string indent = "");
    
    public abstract ASTNode Clone();
    
    protected static string DebugIndent(string text, string indent) => "\n" + string.Join("\n", text.Split('\n').Select(line => indent + line));
}