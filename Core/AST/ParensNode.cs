using Newtonsoft.Json;

namespace Core.AST;

public class ParensNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public ASTNode? Statement { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ParensNode:
                                Statement: {(Statement is null ? "<null>" : Statement.GetTree("\t\t"))}
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new ParensNode(Line, SourceFileId)
        {
            Statement = Statement?.Clone()
        };
    }
}