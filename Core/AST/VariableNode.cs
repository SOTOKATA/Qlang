namespace Core.AST;

public class VariableNode : ASTNode
{
    public string ClassName { get; set; } = "";
    public string Name { get; set; } = "";

    public override ASTNode Clone() => new VariableNode
    {
        ClassName = ClassName, 
        Name = Name,
        SourceFile =  SourceFile, 
        Line =  Line 
    };

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            VariableNode:
                                ClassName: {ClassName}
                                Name: {Name}
                            """, indent);
    }
}