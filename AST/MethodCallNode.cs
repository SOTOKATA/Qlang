namespace Qlang.AST;

public class MethodCallNode : ASTNode
{
    public string ObjectName { get; set; }  // "Term"
    public string MethodName { get; set; }  // "print"
    public List<ASTNode> Arguments { get; set; } = [];
    public override string GetTree(string indent = "")
    {
        return $"{indent}{ObjectName}.{MethodName}: {Arguments.Select(a => a.GetTree(indent))}";
    }
}