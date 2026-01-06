namespace Core.AST;

public class NamespacePointerNode(string name) : ASTNode
{
    public string Name { get; set; } = name;
    
    
    
    public override string GetTree(string indent = "")
    {
        return $"{indent}{Name}";
    }

    public override ASTNode Clone()
    {
        return new NamespacePointerNode(Name)
        {
            SourceFile =  SourceFile,
            Line =  Line,
        };
    }
}