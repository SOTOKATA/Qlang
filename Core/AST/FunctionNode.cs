using Newtonsoft.Json;

namespace Core.AST;

public class FunctionNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public string Name { get; set; }

    [JsonProperty("b")]
    public bool IsStatic { get; set; } = true;
    
    [JsonProperty("c")]
    public bool IsPrivate { get; set; } = false;
    [JsonProperty("d")]
    public List<AssignmentNode> Parameters { get; set; } = [];
    [JsonProperty("e")]
    public List<ASTNode> Body { get; set; } = [];

    public override ASTNode Clone()
    {
        return new FunctionNode(Line, SourceFileId)
        {
            IsStatic = IsStatic,
            IsPrivate = IsPrivate,
            Parameters = Parameters.Select(node => node.Clone()).Cast<AssignmentNode>().ToList(),
            Name = Name, 
            Body = Body.Select(node => node.Clone()).ToList() 
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            FunctionNode:
                                Name: {Name}
                                IsStatic: {IsStatic}
                                IsPrivate: {IsPrivate}
                                Parameters: [{string.Join(",\n", Parameters.Select(x => x.GetTree("\t\t")))}]
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}