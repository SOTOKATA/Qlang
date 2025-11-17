namespace Qlang.AST;

public class AssignmentNode(bool isStatic, bool isPrivate, bool isConst) : ASTNode
{
    public string VariableName { get; set; }
    
    public ASTNode Value { get; set; }

    public bool IsStatic { get; set; } = isStatic;

    public bool IsPrivate { get; set; } = isPrivate;

    public bool IsConst { get; set; } = isConst;

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(AssignmentNode), [VariableName, Value, IsStatic, IsPrivate], indent); 
    }
}