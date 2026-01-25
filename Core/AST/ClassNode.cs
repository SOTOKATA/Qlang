using Newtonsoft.Json;

namespace Core.AST;

public class ClassNode(int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public string Name { get; set; }
    [JsonProperty("b")]
    public string Extends { get; set; }
    [JsonProperty("c")]
    public List<ASTNode> Body { get; set; }

    public override ASTNode Clone()
    {
        return new ClassNode(Line, SourceFileId)
        {
            Name = Name, 
            Extends = Extends,
            Body = Body.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                ClassNode:
                    Name: {Name}
                    Extends: {(Extends == "" ? "<not_exists>" : Extends)}
                    Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                """, indent);
    }
}