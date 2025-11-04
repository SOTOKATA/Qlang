namespace Qlang.AST;

public class ProgramNode : ASTNode
{
    public List<ASTNode> Statements { get; set; } = [];

    public override string GetTree(string indent = "")
    {
        return Statements.Aggregate("", (current, statement) => current + (statement.GetTree(current) + Environment.NewLine));
    }
}