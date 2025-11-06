namespace Qlang.AST;

public class VariableNode : ASTNode
{
    public string Name { get; set; }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(VariableNode), [Name], indent);
    }
}