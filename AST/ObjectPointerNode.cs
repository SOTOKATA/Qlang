namespace Qlang.AST;

public class ObjectPointerNode : ASTNode
{
    public string Name;
    
    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(ObjectPointerNode), [Name], indent);
    }

    public override ASTNode Clone()
    {
        return new ObjectPointerNode
        {
            Name = Name,
        };
    }
}