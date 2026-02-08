using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class WhileNode(int line) : ASTBlock(line)
{
    public WhileNode() : this(-1) {}
    
    [Key(1)]
    
    public required ASTNode Condition { get; set; }
    [Key(2)]
    
    public List<ASTNode> Body { get; set; } = [];
    [Key(3)]
    
    public bool IsDoWhile { get; set; }

    public override ASTNode Clone()
    {
        return new WhileNode(DebugIndex)
        {
            Condition = Condition.Clone(), 
            Body = Body.Select(node => node.Clone()).ToList(),
            IsDoWhile = IsDoWhile
        };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            WhileNode:
                                DebugIndex: {DebugIndex}
                                Condition: {Condition.GetTree("\t\t")}
                                IsDoWhile: {IsDoWhile}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}