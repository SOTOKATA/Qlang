using Qlang.Core.Lang.AST;

namespace Qlang.Core.Lang.Dynamic;

public class DynamicClass(string name)
{
    public string Name = name;

    public readonly string ClassName = name;
    
    public Dictionary<string, Variable> Variables = [];

    public List<ASTNode> Body = [];

    public override string ToString()
    {
        return $"{ClassName}";
    }

    public DynamicClass Clone()
    {
        var clone = new DynamicClass(Name);
        
        var clonedBody = Body
            .Select(node => node.Clone())
            .ToList();
        
        clone.Body = clonedBody;
        
        var clonedVariables = Variables
            .ToDictionary(
                kv => kv.Key,
                kv => new Variable(kv.Value.Name, kv.Value.Value, kv.Value.IsStatic, kv.Value.IsPrivate, kv.Value.IsConst)
            );
        
        clone.Variables = clonedVariables;
        
        return clone;
    }
}