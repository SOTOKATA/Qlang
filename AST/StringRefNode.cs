namespace Qlang.AST;

public class StringRefNode : ASTNode
{
    public int Index { get; set; }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(StringRefNode), [Index], indent);
    }
}