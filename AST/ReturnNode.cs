namespace Qlang.AST;

public class ReturnNode : ASTNode
{
    public ASTNode ReturnValue { get; set; }

    public override string GetTree(string indent = "")
    {
        return $"{indent}ReturnValue: {ReturnValue.GetTree(indent)}";
    }
}