namespace Core.AST;

public class NumberNode : ASTNode
{
    public double Value { get; set; }

    public override ASTNode Clone() => new NumberNode
    {
        Value = Value,
        SourceFile =  SourceFile, 
        Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(NumberNode), [Value], indent);
    }
}