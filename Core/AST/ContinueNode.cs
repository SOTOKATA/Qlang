namespace Core.AST;

public class ContinueNode : ASTNode
{
    public override ASTNode Clone() => new ContinueNode()
    {
        SourceFile =  SourceFile, 
        Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ContinueNode
                            """, indent);
    }
}