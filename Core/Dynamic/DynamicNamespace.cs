using Core.AST;

namespace Core.Dynamic;

public class DynamicNamespace(string name)
{
    public readonly string Name = name;
    
    public Dictionary<string, Variable> Variables = [];

    public List<DynamicClass> Classes = [];
    
    public List<DynamicNamespace> Namespaces = [];

    public List<FunctionNode> Functions = [];

    public bool IsPrivate = false;

    public override string ToString()
    {
        return $"{Name}";
    }
}