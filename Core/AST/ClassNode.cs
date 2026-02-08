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

    public override ASTNode Clone()
    {
        return new ClassNode
        {
            NameId = NameId, 
            ExtendsPath = ExtendsPath,
            Body = Body.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {//Extends: {string.Join(", ", Extends?.Objects.Select(x => x.GetTree("\t\t")) ?? ["<not_exists>"])}
        return DebugIndent($"""
                ClassNode:
                    Name: {NameId}
                    Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                """, indent);
    }
}