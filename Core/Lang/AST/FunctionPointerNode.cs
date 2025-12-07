namespace Qlang.Core.Lang.AST;

public class FunctionPointerNode : ASTNode
{
    public string Name;
    public List<ASTNode> Arguments;
    
    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(FunctionPointerNode), [Name, Arguments], indent);
    }

    public override ASTNode Clone()
    {
        return new FunctionPointerNode
        {
            Name = Name,
            Arguments = [..Arguments.ConvertAll(arg => arg.Clone())],
        };
    }
}