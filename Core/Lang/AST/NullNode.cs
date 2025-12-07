namespace Qlang.Core.Lang.AST;

public class NullNode : ASTNode
{
    public override string GetTree(string indent = "")
    {
        return "NullNode";
    }

    public override ASTNode Clone() => new NullNode();
}