namespace Qlang.AST;

public class NumberRefNode : ASTNode
{
    public int Index { get; set; }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(NumberNode), [Index], indent);
    }
}