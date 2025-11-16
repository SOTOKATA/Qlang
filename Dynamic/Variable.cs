namespace Qlang.Dynamic;

public class Variable(string name, object value, bool isStatic, bool isPrivate)
{
    public override string ToString()
    {
        return Value.ToString();       
    }

    public string Name { get; set; } = name;
    public object Value { get; set; } = value;
    
    public bool IsStatic { get; set; } = isStatic;
    
    public bool IsPrivate { get; set; } = isPrivate;
    
    // public bool IsConstant { get; set; }
}