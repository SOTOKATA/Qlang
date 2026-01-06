namespace Core.AST;

public class ClassNode : ASTNode
{
    public string Name { get; set; }
    public string Extends { get; set; }
    public List<ASTNode> Body { get; set; }

    public override ASTNode Clone()
    {
        return new ClassNode { Name = Name, Extends = Extends, Body = Body.Select(node => node.Clone()).ToList() };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ClassNode), [Name, Body], indent);
    }
}