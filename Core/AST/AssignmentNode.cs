using Core.Tables;
using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class AssignmentNode(bool isPrivate, bool isConst, bool isNew) : ASTNode
{
    public AssignmentNode() : this(false, false, false) {}
    
    [Key(0)]
    
    public bool IsNew { get; set; } = isNew;

    [Key(1)]
    
    public required List<ASTNode> Path { get; init; }

    [Key(2)]
    
    public ASTNode? Value { get; set; }

    [Key(3)]
    public List<CallNode> Types { get; set; } = [];

    [Key(4)]
    
    public bool IsPrivate { get; set; } = isPrivate;

    [Key(5)]
    
    public bool IsConst { get; set; } = isConst;

    [IgnoreMember] 
    public int FileId = -1;

    public int GetLastNameId() => (Path[^1] as ObjectPointerNode)!.NameId;

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => string.Join(".", Path.Select(x => x.ToTokenString(stringPoolTable)));
    
    public override ASTNode Clone()
    {
        return new AssignmentNode(IsPrivate, IsConst, IsNew)
        {
            Path = Path.Select(node => node.Clone()).ToList(),
            Value = Value?.Clone(),
            Types = Types.Select(x => (CallNode)x.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            VariableNode:
                                Path: [{string.Join(",\n", Path.Select(x => x.GetTree("\t")))}]
                                Value: {Value?.GetTree("\t") ?? "<undefined>"}
                                Types: {Types.Select(x => x.GetTree("\t"))}
                                IsPrivate: {IsPrivate}
                                IsConst: {IsConst}
                                IsNew: {IsNew}
                            """, indent);
    }
}
