namespace Qlang.AST;

public class NumberNode : ASTNode
{
    public double Value { get; set; }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(NumberNode), [Value], indent);
    }
}