namespace Qlang.AST;

public class BooleanNode : ASTNode
{
    public bool Value { get; set; }
    public override string GetTree(string indent = "")
    {
        return Value.ToString();
    }
}