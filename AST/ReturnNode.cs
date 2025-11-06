namespace Qlang.AST;

public class ReturnNode : ASTNode
{
    public ASTNode ReturnValue { get; set; }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ReturnNode), [ReturnValue], indent);
    }
}