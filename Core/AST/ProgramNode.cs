namespace Core.AST;

public class ProgramNode : ASTNode
{
    public List<ASTNode> Statements { get; set; } = [];

    public override ASTNode Clone() => new ProgramNode
    {
        Statements = Statements.Select(node => node.Clone()).ToList(),
        SourceFile =  SourceFile, 
        Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($""" 
                            ProgramNode:
                                Statements: [{string.Join(",\n", Statements.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}