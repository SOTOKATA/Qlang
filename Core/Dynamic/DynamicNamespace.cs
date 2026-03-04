using Core.AST;

namespace Core.Dynamic;

public class DynamicNamespace(string name)
{
    public readonly string Name = name;
    
    public Dictionary<string, Variable> Variables = [];

    public readonly List<ClassNode> Classes = [];
    
    public readonly List<DynamicNamespace> Namespaces = [];

    public readonly List<FunctionNode> Functions = [];

    public bool IsPrivate = false;

    public override string ToString()
    {
        return $"{Name}";
    }
}