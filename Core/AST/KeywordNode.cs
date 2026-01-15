namespace Core.AST;

public class KeywordNode(string keyword) : ASTNode
{
    public string Value { get; set; } = keyword;
    
    public override string GetTree(string indent = "")
    {
        return "";
    }

    public override ASTNode Clone()
    {
        return new KeywordNode(Value)
        {
            Line = Line,
            SourceFile = SourceFile
        };
    }
}