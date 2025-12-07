namespace Qlang.Core.Lang.AST;

public class ProgramNode : ASTNode
{
    public List<ASTNode> Statements { get; set; } = [];

    public override ASTNode Clone() => new ProgramNode { Statements = Statements.Select(node => node.Clone()).ToList() };

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ProgramNode), [Statements], indent);
    }
}