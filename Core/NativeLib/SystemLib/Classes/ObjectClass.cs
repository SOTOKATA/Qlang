using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class ObjectClass : IQlangClass
{
    public string Name { get; init; } = "Object";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("IsSimplify",  (Func<object?, bool>)(obj => obj is int or null or long or double or float or string or bool or List<object?>)),
            ("Equals", (Func<object?, object?, bool>)(ObjectsEqual))
        ];
    }
    
    private static bool ObjectsEqual(object? obj1, object? obj2)
    {
        return obj1 switch
        {
            null when obj2 == null => true,
            DynamicClass d1 when obj2 is DynamicClass d2 => d1.Id.Equals(d2.Id),
            string s1 when obj2 is string s2 => s1 == s2,
            int i1 when obj2 is int i2 => i1 == i2,
            _ => obj1?.Equals(obj2) ?? false
        };
    }
}