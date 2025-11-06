namespace Qlang.AST;

public class ProgramNode : ASTNode
{
    public List<ASTNode> Statements { get; set; } = [];

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ProgramNode), [Statements], indent);
    }
}