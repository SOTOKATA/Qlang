namespace Core.AST;

public class BreakNode : ASTNode
{
    public override ASTNode Clone() => new BreakNode
    { 
        SourceFile =  SourceFile, 
        Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            BreakNode
                            """, indent);
    }
}