namespace Core.AST;

public class UsingNode : ASTNode
{
    public required CallNode CallPath { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return CallPath.GetTree(indent);
    }

    public override ASTNode Clone()
    {
        return new UsingNode
        {
            CallPath = CallPath,
            SourceFile = SourceFile,
            Line = Line,
        };
    }
}