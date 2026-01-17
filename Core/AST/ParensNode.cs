namespace Core.AST;

public class ParensNode : ASTNode
{
    public ASTNode? Statement { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ParensNode:
                                Statement: {(Statement is null ? "<null>" : Statement.GetTree("\t\t"))}
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new ParensNode
        {
            Statement = Statement?.Clone(),
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }
}