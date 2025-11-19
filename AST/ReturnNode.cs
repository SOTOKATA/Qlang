namespace Qlang.AST;

public class ReturnNode : ASTNode
{
    public ASTNode ReturnValue { get; set; }

    public override ASTNode Clone() => new ReturnNode { ReturnValue = ReturnValue.Clone() };

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ReturnNode), [ReturnValue], indent);
    }
}