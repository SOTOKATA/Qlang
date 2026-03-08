using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class FunctionNode : ASTNode
{
    [Key(0)]
    
    public required int NameId { get; set; }

    [Key(1)]
    
    public bool IsPrivate { get; set; }
    [Key(2)]
    
    public List<AssignmentNode> Parameters { get; set; } = [];
    [Key(3)]
    
    public List<ASTNode> Body { get; set; } = [];

    [Key(4)] 
    public CallNode? ReturnType { get; set; }

    [Key(5)] 
    public bool IsAsync { get; set; }

    [IgnoreMember] 
    public ASTContext? Context;

    public override ASTNode Clone()
    {
        return new FunctionNode
        {
            IsPrivate = IsPrivate,
            Parameters = Parameters.Select(node => node.Clone()).Cast<AssignmentNode>().ToList(),
            Context = Context,
            NameId = NameId, 
            Body = Body.Select(node => node.Clone()).ToList() 
        };
    }

    public override string ToString()
    {
        return "Function";
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            FunctionNode:
                                Name: {NameId}
                                IsPrivate: {IsPrivate}
                                Parameters: [{string.Join(",\n", Parameters.Select(x => x.GetTree("\t")))}]
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t")))}]
                            """, indent);
    }
}