namespace Core.AST;

public class AssignmentNode(bool isStatic, bool isPrivate, bool isConst, bool isNew) : ASTNode
{
    public bool IsNew { get; set; } = isNew;

    public string? VariableName { get; set; }

    public List<ASTNode>? Path { get; set; }

    public ASTNode? Value { get; set; }
    
    public CallNode? Type { get; set; }

    public bool IsStatic { get; set; } = isStatic;

    public bool IsPrivate { get; set; } = isPrivate;

    public bool IsConst { get; set; } = isConst;

    public bool IsPathAssignment => Path is { Count: > 0 };

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
            Type = Type,
            IsStatic = IsStatic,
            IsPrivate = IsPrivate,
            IsConst = IsConst,
            IsNew = IsNew, 
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            VariableName (Legacy): {VariableName}
                            Path: [{string.Join(",\n", Path?.Select(x => x.GetTree("\t\t")) ?? ["<not_exists>"])}]
                            Value: {Value?.GetTree("\t\t") ?? "<undefined>"}
                            Type: {Type?.GetTree("\t\t")}
                            IsStatic: {IsStatic}
                            IsPrivate: {IsPrivate}
                            IsConst: {IsConst}
                            IsNew: {IsNew}
                            """, indent);
    }
}
