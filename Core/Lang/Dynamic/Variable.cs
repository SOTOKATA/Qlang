using Qlang.Core.Lang.AST;

namespace Qlang.Core.Lang.Dynamic;

public class Variable(string name, object? value, bool isStatic, bool isPrivate, bool isConst)
{
    public override string? ToString()
    {
        return Value?.ToString();       
    }

    public string Name { get; set; } = name;
    public object? Value { get; set; } = value;

    public string Type { get; set; } = "";
    
    public bool IsStatic { get; set; } = isStatic;
    
    public bool IsConst { get; set; } = isConst;
    
    public bool IsPrivate { get; set; } = isPrivate;
    
    // public bool IsConstant { get; set; }
}