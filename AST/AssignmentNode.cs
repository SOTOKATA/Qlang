namespace Qlang.AST;

public class AssignmentNode : ASTNode
{
    public string VariableName { get; set; }
    public ASTNode Value { get; set; }

    public override string GetTree(string indent = "")
    {
        return $"{indent}VariableName: {VariableName} & {Value.GetTree(indent)}";
    }
}