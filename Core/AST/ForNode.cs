using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ForNode(int line) : ASTBlock(line)
{
    public ForNode() : this(-1) {}
    
    [Key(1)]
    
    public required AssignmentNode Assignment { get; set; }
    [Key(2)]
    
    public required ASTNode Condition { get; set; }
    [Key(3)]
    
    public required ASTNode Statement { get; set; }

    [Key(4)]
    
    public List<ASTNode> Body { get; set; } = [];

    public override ASTNode Clone()
    {
        return new ForNode(DebugIndex) { 
            Assignment = (Assignment.Clone() as AssignmentNode)!,
            Statement = Statement.Clone(),
            Condition = Condition, 
            Body = Body.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ForNode:
                                DebugIndex: {DebugIndex}
                                Assignment: {Assignment.GetTree("\t\t")}
                                Condition: {Condition.GetTree("\t\t")}
                                Statement: {Condition.GetTree("\t\t")}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}