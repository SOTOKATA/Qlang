using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ClassNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public required string Name { get; set; }
    
    [Key(2)]
    
    public required string Extends { get; set; }
    [Key(3)]
    
    public required List<ASTNode> Body { get; set; }

    public override ASTNode Clone()
    {
        return new ClassNode(DebugIndex)
        {
            Name = Name, 
            Extends = Extends,
            Body = Body.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {//Extends: {string.Join(", ", Extends?.Objects.Select(x => x.GetTree("\t\t")) ?? ["<not_exists>"])}
        return DebugIndent($"""
                ClassNode:
                    Name: {Name}
                    Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                """, indent);
    }
}