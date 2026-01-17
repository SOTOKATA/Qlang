namespace Core.AST;

public class IfNode : ASTBlock
{
    public ASTNode Condition { get; set; }
    public List<ASTNode> ThenBlock { get; set; } = [];
    public List<ASTNode> ElseBlock { get; set; } = [];

    public override ASTNode Clone()
    {
        return new IfNode
        {
            Condition = Condition.Clone(), 
            ThenBlock = ThenBlock.Select(node => node.Clone()).ToList(), 
            ElseBlock = ElseBlock.Select(node => node.Clone()).ToList(),
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            IfNode:
                                Condition: {Condition.GetTree("\t\t")}
                                ThenBlock: [{string.Join(",\n", ThenBlock.Select(x => x.GetTree("\t\t")))}]
                                ElseBlock: [{string.Join(",\n", ElseBlock.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}