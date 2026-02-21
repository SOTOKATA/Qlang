using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class ReturnNode : ASTNode
{
    [Key(0)]
    
    public ASTNode? ReturnValue { get; set; }

    public override ASTNode Clone() => new ReturnNode
    {
        ReturnValue = ReturnValue?.Clone() 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ReturnNode:
                                ReturnValue: {(ReturnValue is null ? "<nothing>" : ReturnValue.GetTree("\t"))}
                            """, indent);
    }
}