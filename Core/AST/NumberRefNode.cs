namespace Core.AST;

public class NumberRefNode : ASTNode
{
    public bool IsNegative { get; set; } = false;
    public int Index { get; set; }

    public override ASTNode Clone() => new NumberRefNode { Index = Index, IsNegative = IsNegative };

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(NumberNode), [Index], indent);
    }
}