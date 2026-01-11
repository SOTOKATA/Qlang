namespace Core.AST;

public class ParensNode : ASTNode
{
    public ASTNode? Statement { get; set; } = null;
    
    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ParensNode), [Statement], indent);
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