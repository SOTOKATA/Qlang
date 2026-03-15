using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ProgramNode : ASTNode
{
    [Key(0)]
    
    public List<ASTNode> Statements { get; set; } = [];

    public override ASTNode Clone() => new ProgramNode
    {
        Statements = Statements.Select(node => node.Clone()).ToList()
    };

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => "Program";

    public override string GetTree(string indent = "")
    {
        return DebugIndent($""" 
                            ProgramNode:
                                Statements: [{string.Join(",\n", Statements.Select(x => x.GetTree("\t")))}]
                            """, indent);
    }
}