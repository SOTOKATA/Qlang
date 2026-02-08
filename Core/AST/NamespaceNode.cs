using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class NamespaceNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public required int NameId { get; set; }

    [Key(2)]
    
    public List<ASTNode> Body { get; set; } = [];

    [Key(3)] 
    public bool IsPrivate { get; set; }

    public override ASTNode Clone()
    {
        return new NamespaceNode(DebugIndex) { NameId = NameId, Body = Body.Select(node => node.Clone()).ToList() };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NamespaceNode:
                                DebugIndex: {DebugIndex}
                                Name: {NameId}
                                IsPrivate: {IsPrivate}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}