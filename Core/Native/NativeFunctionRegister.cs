namespace Core.Native;

public class NativeFunctionRegister(string @namespace, string @class, string name, Delegate function)
{
    private string Class { get; } = @class.Trim();
    private string Namespace { get; } = @namespace.Trim();
    private string Name { get; } = name.Trim();
    public Delegate Function { get; } = function;
    
    public string GetName() => Namespace + "." +  Class + "." + Name;
    
    public override string ToString() => GetName();
}