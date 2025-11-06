namespace Qlang.AST;

public abstract class ASTNode
{
    public abstract string GetTree(string indent = "");
    
    public int Line { get; set; }
    public int Column { get; set; }
    public string SourceFile { get; set; }
}