namespace Qlang.AST;

public class FunctionNode : ASTNode
{
    public string Name { get; set; }
    public List<string> Parameters { get; set; } = [];
    public List<ASTNode> Body { get; set; } = [];
}