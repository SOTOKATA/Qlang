using Core.Dynamic;
using MessagePack;
using Newtonsoft.Json;

namespace Core.AST;

[MessagePackObject]
public class ASTBlock(int line) : ASTNode(line)
{
    public ASTBlock() : this(-1) {}
    
    [Key(-1)]
    [JsonProperty("v")]
    public Dictionary<string, Variable> Variables { get; set; } = [];

    public override string GetTree(string indent = "")
    {
        throw new NotImplementedException();
    }

    public override ASTNode Clone()
    {
        throw new NotImplementedException();
    }
}