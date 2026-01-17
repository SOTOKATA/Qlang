namespace Core.AST;

public class ObjectPointerNode : ASTNode
{
    public string? Name;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            ObjectPointerNode:
                                Name: {Name ?? "<undefined>"}
                            """,
            indent);
    }

    public override ASTNode Clone()
    {
        return new ObjectPointerNode
        {
            Name = Name,
            SourceFile =  SourceFile, 
            Line =  Line 
        };
    }
}