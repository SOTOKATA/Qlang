using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class CollectionNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public required List<ASTNode> Collection { get; set; }
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            CollectionNode:
                                Collection: [{string.Join(",\n", Collection.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }

    public override ASTNode Clone()
    {
        var node = new CollectionNode(DebugIndex)
        {
            Collection = Collection.Select(node => node.Clone()).ToList()
        };

        return node;
    }
}