using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ForNode : ASTBlock
{
    [Key(0)]
    
    public required AssignmentNode Assignment { get; set; }
    [Key(1)]
    
    public required ASTNode Condition { get; set; }
    [Key(2)]
    
    public required ASTNode Statement { get; set; }

    [Key(3)]
    
    public List<ASTNode> Body { get; set; } = [];

    public override ASTNode Clone()
    {
        return new ForNode { 
            Assignment = (Assignment.Clone() as AssignmentNode)!,
            Statement = Statement.Clone(),
            Condition = Condition, 
            Body = Body.Select(node => node.Clone()).ToList()
        };
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
        =>
            $"for {Assignment.ToTokenString(stringPoolTable)}; {Condition.ToTokenString(stringPoolTable)}; {Statement.ToTokenString(stringPoolTable)}";

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ForNode:
                                Assignment: {Assignment.GetTree("\t")}
                                Condition: {Condition.GetTree("\t")}
                                Statement: {Condition.GetTree("\t")}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t")))}]
                            """, indent);
    }
}