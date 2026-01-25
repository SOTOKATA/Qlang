using Core.AST;

namespace Core.Dynamic;

public class DynamicNamespace(string name)
{
    public readonly string Name = name;
    
    public readonly Dictionary<string, Variable> Variables = [];

    public readonly List<DynamicClass> Classes = [];
    
    public readonly List<DynamicNamespace> Namespaces = [];

    public readonly List<FunctionNode> Functions = [];

    public bool IsPrivate = false;

    public override string ToString()
    {
        return $"{Name}";
    }
}