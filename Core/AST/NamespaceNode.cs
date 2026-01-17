namespace Core.AST;

public class NamespaceNode : ASTNode
{
    public string Name { get; set; }
    
    public List<ASTNode> Body { get; set; }

    public bool IsPrivate;

    public override ASTNode Clone()
    {
        return new NamespaceNode { Name = Name, Body = Body.Select(node => node.Clone()).ToList(), SourceFile =  SourceFile, Line =  Line };
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            NamespaceNode:
                                Name: {Name}
                                IsPrivate: {IsPrivate}
                                Body: [{string.Join(",\n", Body.Select(x => x.GetTree("\t\t")))}]
                            """, indent);
    }
}