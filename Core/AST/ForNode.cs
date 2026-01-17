namespace Core.AST;

public class ForNode : ASTBlock
{
    public AssignmentNode Assignment { get; set; }
    public ASTNode Condition { get; set; }
    public ASTNode Statement { get; set; }

    public List<ASTNode> Body { get; set; } = [];

    public override ASTNode Clone()
    {
        return new ForNode { 
            Assignment = (Assignment.Clone() as AssignmentNode)!,
            Statement = Statement.Clone(),
            Condition = Condition, 
            Body = Body.Select(node => node.Clone()).ToList(),
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ForNode:
                                Assignment: {Assignment.GetTree("\t\t")}
                                Condition: {Condition.GetTree("\t\t")}
                                Statement: {Condition.GetTree("\t\t")}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}