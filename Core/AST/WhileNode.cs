namespace Core.AST;

public class WhileNode : ASTBlock
{
    public ASTNode Condition { get; set; }

    public List<ASTNode> Body { get; set; } = [];
    
    public bool IsDoWhile { get; set; }

    public override ASTNode Clone()
    {
        return new WhileNode
        {
            Condition = Condition.Clone(), 
            Body = Body.Select(node => node.Clone()).ToList(),
            IsDoWhile = IsDoWhile,
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            WhileNode:
                                Condition: {Condition.GetTree("\t\t")}
                                IsDoWhile: {IsDoWhile}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}