using Core.AST;

namespace Core.Dynamic;

public class Variable(string name, object? value, bool isStatic, bool isPrivate, bool isConst, CallNode? type = null)
{
    public override string? ToString()
    {
        return Value?.ToString();       
    }

    public string Name { get; set; } = name;
    public object? Value { get; set; } = value;

    public CallNode? Type { get; set; } = type;
    
    public bool IsStatic { get; set; } = isStatic;
    
    public bool IsConst { get; set; } = isConst;
    
    public bool IsPrivate { get; set; } = isPrivate;
}