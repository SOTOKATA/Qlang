namespace Qlang.AST;

public class ClassNode : ASTNode
{
    public string Name { get; set; }
    public List<ASTNode> Body { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ClassNode), [Name, Body], indent);
    }
}