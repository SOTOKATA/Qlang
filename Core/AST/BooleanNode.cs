namespace Core.AST;

public class BooleanNode : ASTNode
{
    public bool Value { get; set; }

    public override ASTNode Clone() => new BooleanNode { Value = Value };

    public override string GetTree(string indent = "")
    {
        return Value.ToString();
    }
}