using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class NumberRefNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public bool IsNegative { get; set; }
    [Key(2)]
    
    public int Index { get; set; }

    public override ASTNode Clone() => new NumberRefNode(DebugIndex)
    {
        Index = Index, 
        IsNegative = IsNegative
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NumberRefNode:
                                DebugIndex: {DebugIndex}
                                Ref: {Index}
                                IsNegative: {IsNegative}
                            """, indent);
    }
}