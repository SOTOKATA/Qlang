namespace Qlang.Core.Lang.AST;

public class AssignmentNode(bool isStatic, bool isPrivate, bool isConst, bool isNew) : ASTNode
{
    public bool IsNew { get; set; } = false;
    public string VariableName { get; set; }
    
    public ASTNode? Value { get; set; }

    public bool IsStatic { get; set; } = isStatic;

    public bool IsPrivate { get; set; } = isPrivate;

    public bool IsConst { get; set; } = isConst;

    public override ASTNode Clone()
    {
        return new AssignmentNode(IsStatic, IsPrivate, IsConst, IsNew)
        {
            VariableName = VariableName,
            Value = Value?.Clone() ?? null,
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(AssignmentNode), [VariableName, Value, IsStatic, IsPrivate], indent); 
    }
}