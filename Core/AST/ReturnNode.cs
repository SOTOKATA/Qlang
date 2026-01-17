namespace Core.AST;

public class ReturnNode : ASTNode
{
    public ASTNode? ReturnValue { get; set; }

    public override ASTNode Clone() => new ReturnNode
    {
        ReturnValue = ReturnValue?.Clone(),
        SourceFile =  SourceFile, 
        Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ReturnNode:
                                ReturnValue: {(ReturnValue is null ? "<nothing>" : ReturnValue.GetTree("\t\t"))}
                            """, indent);
    }
}