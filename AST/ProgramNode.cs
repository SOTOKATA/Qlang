namespace Qlang.AST;

public class ProgramNode : ASTNode
{
    public List<ASTNode> Statements { get; set; } = [];
}