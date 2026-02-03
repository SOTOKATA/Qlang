using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ProgramNode(int line) : ASTNode(line)
{
    [Key(1)]
    
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