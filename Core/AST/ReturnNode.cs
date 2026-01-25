using Newtonsoft.Json;

namespace Core.AST;

public class ReturnNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public ASTNode? ReturnValue { get; set; }

    public override ASTNode Clone() => new ReturnNode(line, SourceFileId)
    {
        ReturnValue = ReturnValue?.Clone() 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ReturnNode:
                                ReturnValue: {(ReturnValue is null ? "<nothing>" : ReturnValue.GetTree("\t\t"))}
                            """, indent);
    }
}