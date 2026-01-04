namespace Core.AST;

public class FunctionNode : ASTNode
{
    public string Name { get; set; }

    public bool IsStatic { get; set; } = true;
    
    public bool IsPrivate { get; set; } = false;
    public List<AssignmentNode> Parameters { get; set; } = [];
    public List<ASTNode> Body { get; set; } = [];

    public override ASTNode Clone()
    {
        return new FunctionNode
        {
            IsStatic = IsStatic,
            IsPrivate = IsPrivate,
            Parameters = Parameters.Select(node => node.Clone()).Cast<AssignmentNode>().ToList(),
            Name = Name, 
            Body = Body.Select(node => node.Clone()).ToList()
        };
    }

    public override string GetTree(string indent = "")
    {
        return ASTGetTreeBuilder.Build(nameof(FunctionNode), [Name, IsStatic, Parameters, Body], indent);
    }
}