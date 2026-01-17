using Newtonsoft.Json;

namespace Core.AST;

[JsonObject(MemberSerialization.OptOut)]
// ReSharper disable once InconsistentNaming
public abstract class ASTNode
{
    public abstract string GetTree(string indent = "");
    
    public abstract ASTNode Clone();
    
    public int Line { get; set; }
    public string? SourceFile { get; set; }
    
    protected static string DebugIndent(string text, string indent) => "\n" + string.Join("\n", text.Split('\n').Select(line => indent + line));
}