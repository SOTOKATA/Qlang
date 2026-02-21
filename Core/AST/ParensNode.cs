using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ParensNode : ASTNode
{
    [Key(0)]
    
    public ASTNode? Statement { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ParensNode:
                                Statement: {(Statement is null ? "<null>" : Statement.GetTree("\t"))}
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new ParensNode
        {
            Statement = Statement?.Clone()
        };
    }
}