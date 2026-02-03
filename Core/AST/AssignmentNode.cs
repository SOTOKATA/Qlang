using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class AssignmentNode(bool isStatic, bool isPrivate, bool isConst, bool isNew, int line) : ASTNode(line)
{
    public AssignmentNode() : this(false, false, false, false, -1) {}
    
    [Key(1)]
    
    public bool IsNew { get; set; } = isNew;

    [Key(2)]
    
    public required List<ASTNode> Path { get; set; }

    [Key(3)]
    
    public ASTNode? Value { get; set; }
    
    [Key(4)]
    
    public CallNode? Type { get; set; }

    [Key(5)]
    
    public bool IsStatic { get; set; } = isStatic;

    [Key(6)]
    
    public bool IsPrivate { get; set; } = isPrivate;

    [Key(7)]
    
    public bool IsConst { get; set; } = isConst;

    public string GetLastName() => (Path[^1] as ObjectPointerNode)!.Name!;

    public override ASTNode Clone()
    {
        return new AssignmentNode(IsStatic, IsPrivate, IsConst, IsNew, DebugIndex)
        {
            Path = Path.Select(node => node.Clone()).ToList(),
            Value = Value?.Clone(),
            Type = Type
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            VariableNode:
                                Path: [{string.Join(",\n", Path.Select(x => x.GetTree("\t\t")))}]
                                Value: {Value?.GetTree("\t\t") ?? "<undefined>"}
                                Type: {Type?.GetTree("\t\t")}
                                IsStatic: {IsStatic}
                                IsPrivate: {IsPrivate}
                                IsConst: {IsConst}
                                IsNew: {IsNew}
                            """, indent);
    }
}
