namespace Core.AST;

public class StringRefNode : ASTNode
{
    public int Index { get; set; }

    public override ASTNode Clone() => new StringRefNode
    {
        Index = Index,
        SourceFile =  SourceFile, 
        Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            StringRefNode:
                                Ref: {Index}
                            """, indent);
    }
}