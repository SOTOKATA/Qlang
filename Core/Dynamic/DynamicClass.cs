using Core.AST;

namespace Core.Dynamic;

public class DynamicClass(string name)
{
    public readonly string Name = name;

    public readonly string ClassName = name;
    
    public readonly Dictionary<string, Variable> Variables = [];

    public List<ASTNode> Body = [];

    public bool IsPrivate = false;
    
    public readonly List<string> Extends = [];

    public override string ToString() => $"{ClassName}";
}