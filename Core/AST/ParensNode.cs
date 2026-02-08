using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ParensNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public ASTNode? Statement { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ParensNode:
                                DebugIndex: {DebugIndex}
                                Statement: {(Statement is null ? "<null>" : Statement.GetTree("\t\t"))}
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new ParensNode(DebugIndex)
        {
            Statement = Statement?.Clone()
        };
    }
}