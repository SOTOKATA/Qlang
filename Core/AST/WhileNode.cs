using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class WhileNode : ASTBlock
{
    [Key(0)]
    
    public required ASTNode Condition { get; set; }
    [Key(1)]
    
    public List<ASTNode> Body { get; set; } = [];
    [Key(2)]
    
    public bool IsDoWhile { get; set; }

    public override ASTNode Clone()
    {
        return new WhileNode
        {
            Condition = Condition.Clone(), 
            Body = Body.Select(node => node.Clone()).ToList(),
            IsDoWhile = IsDoWhile
        };
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => "While";

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            WhileNode:
                                Condition: {Condition.GetTree("\t")}
                                IsDoWhile: {IsDoWhile}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t")))}]
                            """, indent);
    }
}