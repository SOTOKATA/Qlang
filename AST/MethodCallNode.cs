namespace Qlang.AST;

public class MethodCallNode : ASTNode
{
    public string ObjectName { get; set; }  // "Term"
    public string MethodName { get; set; }  // "print"
    public List<ASTNode> Arguments { get; set; } = [];
}