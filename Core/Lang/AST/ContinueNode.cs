namespace Qlang.Core.Lang.AST;

public class ContinueNode : ASTNode
{
    public override ASTNode Clone() => new ContinueNode();

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ContinueNode), ["is continue"], indent);
    }
}