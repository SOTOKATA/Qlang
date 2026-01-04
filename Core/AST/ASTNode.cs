using Newtonsoft.Json;

namespace Core.AST;

[JsonObject(MemberSerialization.OptOut)]
// ReSharper disable once InconsistentNaming
public abstract class ASTNode
{
    public abstract string GetTree(string indent = "");
    
    public abstract ASTNode Clone();
    
    public int Line { get; set; }
    public int LineIndex { get; set; }
    public string? SourceFile { get; set; }
}