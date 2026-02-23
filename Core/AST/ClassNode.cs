using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ClassNode : ASTNode
{
    [Key(0)]
    
    public required int NameId { get; init; }
    
    [Key(1)]
    
    public CallNode? ExtendsPath { get; set; }
    [Key(2)]
    
    public required List<ASTNode> Body { get; set; }

    [Key(3)] 
    public bool IsPrivate { get; set; }

    public override ASTNode Clone()
    {
        return new ClassNode
        {
            NameId = NameId, 
            ExtendsPath = ExtendsPath,
            Body = Body.Select(node => node.Clone()).ToList(),
            IsPrivate = IsPrivate
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                ClassNode:
                    Name: {NameId}
                    IsPrivate: {IsPrivate}
                    Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t")))}]
                """, indent);
    }
}