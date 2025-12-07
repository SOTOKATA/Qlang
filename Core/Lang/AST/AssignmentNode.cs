namespace Qlang.Core.Lang.AST;

public class AssignmentNode(bool isStatic, bool isPrivate, bool isConst, bool isNew) : ASTNode
{
    public bool IsNew { get; set; } = isNew;

    // For simple variable assignments (backward compatibility)
    public string? VariableName { get; set; }

    // For path-based assignments (e.g., object.property.subproperty = value)
    public List<ASTNode>? Path { get; set; }

    public ASTNode? Value { get; set; }

    public bool IsStatic { get; set; } = isStatic;

    public bool IsPrivate { get; set; } = isPrivate;

    public bool IsConst { get; set; } = isConst;

    // Helper property to check if this is a simple variable assignment or a path assignment
    public bool IsPathAssignment => Path != null && Path.Count > 0;

    // Helper method to get the assignment target as a string (for debugging/logging)
    public string GetAssignmentTarget()
    {
        if (IsPathAssignment)
        {
            return string.Join(".", Path!.Select(p => p switch
            {
                ObjectPointerNode obj => obj.Name,
                FunctionPointerNode func => $"{func.Name}()",
                _ => p.GetType().Name
            }));
        }
        return VariableName ?? "unknown";
    }

    public override ASTNode Clone()
    {
        return new AssignmentNode(IsStatic, IsPrivate, IsConst, IsNew)
        {
            VariableName = VariableName,
            Path = Path?.Select(node => node.Clone()).ToList(),
            Value = Value?.Clone(),
        };
    }

    public override string GetTree(string indent = "")
    {
        if (IsPathAssignment)
        {
            return ASTGetTreeBuilder.Build(nameof(AssignmentNode),
                [GetAssignmentTarget(), Path, Value, IsStatic, IsPrivate], indent);
        }
        else
        {
            return ASTGetTreeBuilder.Build(nameof(AssignmentNode),
                [VariableName, Value, IsStatic, IsPrivate], indent);
        }
    }
}
