using Newtonsoft.Json;

namespace Core.AST;

public class CollectionNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
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
        var node = new CollectionNode(Line, SourceFileId)
        {
            Collection = Collection.Select(node => node.Clone()).ToList()
        };

        return node;
    }
}