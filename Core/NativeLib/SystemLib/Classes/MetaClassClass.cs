using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class MetaClassClass : IQlangClass
{
    public string Name { get; init; } = "MetaClass";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("HasVariable", (Func<DynamicClass, string, bool>)((@class, varName) => @class.Variables.ContainsKey(varName))),
            ("IsPrivate", (Func<DynamicClass, bool>)((@class) => @class.IsPrivate)),
            ("GetName", (Func<DynamicClass, string>)((@class) => @class.Name)),
            ("GetVariableValue", (Func<DynamicClass, string, object>)((@class, name) => @class.Variables[name])),
            ("GetClassName", (Func<DynamicClass, string>)((@class) => @class.ClassName)),
        ];
    }
}