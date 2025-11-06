namespace Qlang.AST;

public class ASTContext
{
    public ClassNode? Class { get; set; }
    public FunctionNode? Function { get; set; }
    public ASTNode CurrentNode { get; set; }
}