namespace Qlang.AST;

public class FunctionNode : ASTNode
{
    public string Name { get; set; }

    public bool IsStatic { get; set; } = true;
    
    public bool IsPrivate { get; set; } = false;
    public List<AssignmentNode> Parameters { get; set; } = [];
    public List<ASTNode> Body { get; set; } = [];

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(FunctionNode), [Name, IsStatic, Parameters, Body], indent);
    }
}