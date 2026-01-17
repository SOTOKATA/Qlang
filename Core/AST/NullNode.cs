namespace Core.AST;

public class NullNode : ASTNode
{
    public override string GetTree(string indent = "")
    {
        return DebugIndent("NullNode", indent);
    }

    public override ASTNode Clone() => new NullNode
    {
        SourceFile =  SourceFile, 
        Line =  Line 
    };
}