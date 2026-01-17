namespace Core.AST;

public class BooleanNode : ASTNode
{
    public bool Value { get; set; }

    public override ASTNode Clone() => new BooleanNode { 
            Value = Value, 
            SourceFile =  SourceFile, 
            Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            BreakNode:
                                Value: {Value}
                            """, indent);
    }
}