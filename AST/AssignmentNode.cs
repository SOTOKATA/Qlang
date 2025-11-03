namespace Qlang.AST;

public class AssignmentNode : ASTNode
{
    public string VariableName { get; set; }
    public ASTNode Value { get; set; }
}