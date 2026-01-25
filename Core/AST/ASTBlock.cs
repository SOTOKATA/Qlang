using Core.Dynamic;
using Newtonsoft.Json;

namespace Core.AST;

public abstract class ASTBlock(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("v")]
    public Dictionary<string, Variable> Variables { get; set; } = [];
}