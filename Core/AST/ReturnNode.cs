using Newtonsoft.Json;

namespace Core.AST;

public class ReturnNode(int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public ASTNode? ReturnValue { get; set; }

    public override ASTNode Clone() => new ReturnNode(DebugIndex)
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