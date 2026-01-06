namespace Core.AST;

public class NamespaceNode : ASTNode
{
    public string Name { get; set; }
    
    public List<ASTNode> Body { get; set; }

    public override ASTNode Clone()
    {
        return new NamespaceNode { Name = Name, Body = Body.Select(node => node.Clone()).ToList(), SourceFile =  SourceFile, Line =  Line };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(NamespaceNode), [Name, Body], indent);
    }
}