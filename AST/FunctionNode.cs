namespace Qlang.AST;

public class FunctionNode : ASTNode
{
    public string Name { get; set; }
    public List<string> Parameters { get; set; } = [];
    public List<ASTNode> Body { get; set; } = [];

    public override string GetTree(string indent = "")
    {
        string result = $"{indent}Function: {Name}\n";
        if (Parameters.Count > 0)
            result += $"{indent}  Parameters: {string.Join(", ", Parameters)}";

        if (Body.Count <= 0) 
            return result;
        
        result += $"{indent}  Body:\n";
        
        foreach (var node in Body)
            result += node.GetTree(indent + "    ") + "\n";

        return result;
    }
}