using Newtonsoft.Json;

namespace Qlang.AST;

[JsonObject(MemberSerialization.OptOut)]
public abstract class ASTNode
{
    public abstract string GetTree(string indent = "");
    
    public abstract ASTNode Clone();
    
    public int Line { get; set; }
    public int LineIndex { get; set; }
    public string SourceFile { get; set; }
}