namespace Core.AST;

public class BreakNode : ASTNode
{
    public override ASTNode Clone() => new BreakNode();

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(BreakNode), ["is break"], indent);
    }
}