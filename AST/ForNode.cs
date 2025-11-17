namespace Qlang.AST;

public class ForNode : ASTBlock
{
    public AssignmentNode Assignment { get; set; }
    public ASTNode Condition { get; set; }
    public ASTNode Statement { get; set; }

    public List<ASTNode> Body { get; set; } = [];
    
    public override string GetTree(string indent = "")
    {
        return "";
    }
}