namespace Core.NativeLib.SystemLib.Classes;

public class ObjectClass : IQlangClass
{
    public string Name { get; init; } = "object";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("is_null",  (Func<object?, bool>)(obj => obj is null)),
            ("is_simplify",  (Func<object?, bool>)(obj => obj is int or long or double or float or string or bool or List<object?>))
        ];
    }
}