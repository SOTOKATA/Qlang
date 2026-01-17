namespace Core.AST;

public class NumberRefNode : ASTNode
{
    public bool IsNegative { get; set; }
    public int Index { get; set; }

    public override ASTNode Clone() => new NumberRefNode
    {
        Index = Index, 
        IsNegative = IsNegative,
        SourceFile =  SourceFile, 
        Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NumberRefNode:
                                Ref: {Index}
                                IsNegative: {IsNegative}
                            """, indent);
    }
}