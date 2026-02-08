using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class NumberRefNode : ASTNode
{
    [Key(0)]
    
    public bool IsNegative { get; set; }
    [Key(1)]
    
    public int Index { get; set; }

    public override ASTNode Clone() => new NumberRefNode
    {
        Index = Index, 
        IsNegative = IsNegative
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