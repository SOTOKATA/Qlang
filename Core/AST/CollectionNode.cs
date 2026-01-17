namespace Core.AST;

public class CollectionNode : ASTNode
{

    public List<ASTNode> Collection { get; set; }
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            CollectionNode:
                                Collection: [{string.Join(",\n", Collection.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }

    public override ASTNode Clone()
    {
        var node = new CollectionNode
        {
            Collection = Collection.Select(node => node.Clone()).ToList(),
            SourceFile =  SourceFile, 
            Line =  Line 
        };

        return node;
    }
}