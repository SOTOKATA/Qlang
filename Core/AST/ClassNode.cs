using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ClassNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public required int NameId { get; set; }
    
    [Key(2)]
    
    public required int ExtendsId { get; set; }
    [Key(3)]
    
    public required List<ASTNode> Body { get; set; }

    public override ASTNode Clone()
    {
        return new ClassNode(DebugIndex)
        {
            NameId = NameId, 
            ExtendsId = ExtendsId,
            Body = Body.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {//Extends: {string.Join(", ", Extends?.Objects.Select(x => x.GetTree("\t\t")) ?? ["<not_exists>"])}
        return DebugIndent($"""
                ClassNode:
                    DebugIndex: {DebugIndex}
                    Name: {NameId}
                    Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                """, indent);
    }
}