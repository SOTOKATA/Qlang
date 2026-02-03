using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class NamespaceNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public required string Name { get; set; }

    [Key(2)]
    
    public List<ASTNode> Body { get; set; } = [];

    [Key(3)]
    
    public bool IsPrivate;

    public override ASTNode Clone()
    {
        return new NamespaceNode(DebugIndex) { Name = Name, Body = Body.Select(node => node.Clone()).ToList() };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NamespaceNode:
                                Name: {Name}
                                IsPrivate: {IsPrivate}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}