namespace Qlang.AST;

public class WhileNode : ASTNode
{
    public ASTNode Condition { get; set; }

    public List<ASTNode> Body { get; set; } = [];
    
    public bool IsDoWhile { get; set; } = false;
    
    public override string GetTree(string indent = "")
    {
        string str = indent + Condition.GetTree(indent + "\t");

        str = Body.Aggregate(str, (current, node) => current + (indent + node.GetTree(indent + "\t") + "\n"));

        return str;
    }
}