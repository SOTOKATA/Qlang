namespace Core.AST;

public class UsingNode : ASTNode
{
    public required string NamespaceName { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return NamespaceName;
    }

    public override ASTNode Clone()
    {
        return new UsingNode()
        {
            NamespaceName = NamespaceName,
            SourceFile = SourceFile,
            Line = Line,
        };
    }
}