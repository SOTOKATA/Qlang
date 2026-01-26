using Newtonsoft.Json;

namespace Core.AST;

public class ProgramNode(int line) : ASTNode(line)
{
    [JsonProperty("a")]
    public List<ASTNode> Statements { get; set; } = [];

    public override ASTNode Clone() => new ProgramNode(DebugIndex)
    {
        Statements = Statements.Select(node => node.Clone()).ToList()
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($""" 
                            ProgramNode:
                                Statements: [{string.Join(",\n", Statements.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}