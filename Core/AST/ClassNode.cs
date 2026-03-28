using Core.Tables;
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

    [Key(4)] 
    public List<int> Extends { get; set; } = [];

    [Key(5)] 
    public Guid Id { get; set; }

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => stringPoolTable[NameId];

    public override ASTNode Clone()
    {
        return new ClassNode
        {
            NameId = NameId, 
            ExtendsPath = (CallNode?)ExtendsPath?.Clone(),
            Body = Body.Select(node => node.Clone()).ToList(),
            IsPrivate = IsPrivate,
            Extends = [..Extends],
            Id = Id
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                ClassNode:
                    Id: {Id}
                    Name: {NameId}
                    IsPrivate: {IsPrivate}
                    Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t")))}]
                """, indent);
    }
}