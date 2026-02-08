using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class FunctionNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public required int NameId { get; set; }

    [Key(2)]
    
    public bool IsStatic { get; set; } = true;
    
    [Key(3)]
    
    public bool IsPrivate { get; set; }
    [Key(4)]
    
    public List<AssignmentNode> Parameters { get; set; } = [];
    [Key(5)]
    
    public List<ASTNode> Body { get; set; } = [];

    [IgnoreMember] 
    public ASTContext? Context;

    public override ASTNode Clone()
    {
        return new FunctionNode(DebugIndex)
        {
            IsStatic = IsStatic,
            IsPrivate = IsPrivate,
            Parameters = Parameters.Select(node => node.Clone()).Cast<AssignmentNode>().ToList(),
            Context = Context,
            NameId = NameId, 
            Body = Body.Select(node => node.Clone()).ToList() 
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            FunctionNode:
                                DebugIndex: {DebugIndex}
                                Name: {NameId}
                                IsStatic: {IsStatic}
                                IsPrivate: {IsPrivate}
                                Parameters: [{string.Join(",\n", Parameters.Select(x => x.GetTree("\t\t")))}]
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}