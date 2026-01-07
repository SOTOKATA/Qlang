namespace Core.NativeLib.SystemLib.Classes;

public class ObjectClass : IQlangClass
{
    public string Name { get; init; } = "object";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("is_null",  (Func<object?, bool>)(obj => obj is null)),
            ("is_simplify",  (Func<object?, bool>)(obj => obj is float or double or int or long or decimal or string or bool or List<object> or List<object?>))
        ];
    }
}