using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class MetaClassClass : IQlangClass
{
    public string Name { get; init; } = "class";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("has_variable", (Func<DynamicClass, string, bool>)((@class, varName) => @class.Variables.ContainsKey(varName))),
            ("is_private", (Func<DynamicClass, bool>)((@class) => @class.IsPrivate)),
            ("get_name", (Func<DynamicClass, string>)((@class) => @class.Name)),
            ("getVariableValue", (Func<DynamicClass, string, object>)((@class, name) => @class.Variables[name])),
            ("get_class_name", (Func<DynamicClass, string>)((@class) => @class.ClassName)),
        ];
    }
}