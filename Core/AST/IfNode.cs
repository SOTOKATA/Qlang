using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class IfNode : ASTBlock
{
    [Key(0)]
    
    public required ASTNode Condition { get; set; }
    [Key(1)]
    
    public List<ASTNode> ThenBlock { get; set; } = [];
    [Key(2)]
    
    public List<ASTNode> ElseBlock { get; set; } = [];

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => "if";

    public override ASTNode Clone()
    {
        return new IfNode
        {
            Condition = Condition.Clone(), 
            ThenBlock = ThenBlock.Select(node => node.Clone()).ToList(), 
            ElseBlock = ElseBlock.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            IfNode:
                                Condition: {Condition.GetTree("\t")}
                                ThenBlock: [{string.Join(",\n", ThenBlock.Select(x => x.GetTree("\t")))}]
                                ElseBlock: [{string.Join(",\n", ElseBlock.Select(x => x.GetTree("\t")))}]
                            """, indent);
    }
}