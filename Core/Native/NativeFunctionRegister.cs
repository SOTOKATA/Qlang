namespace Core.Native;

public class NativeFunctionRegister(string @namespace, string @class, string name, Delegate function)
{
    public string Class { get; } = @class.Trim();
    public string Namespace { get; } = @namespace.Trim();
    public string Name { get; } = name.Trim();
    public Delegate Function { get; } = function;
    
    public string GetName() => Namespace + "." +  Class + "." + Name;
    
    public override string ToString() => GetName();
}