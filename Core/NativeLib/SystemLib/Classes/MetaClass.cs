using Core.AST;
using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class MetaClass : IQlangClass
{
    public string Name { get; init; } = "meta";

    public List<(string name, Delegate body)> GetFunctions() => [
        ("fn_list", (Func<DynamicClass, List<object?>>)((@class) =>
        {
            return @class.Body.OfType<FunctionNode>().Select(x => x.NameId).Cast<object?>().ToList();
        })),
        
        ("var_list", (Func<DynamicClass, List<object?>>)((@class) =>
        {
            return @class.Variables.Select(x => x.Value.Name).Cast<object?>().ToList();
        })),
        
        ("var_value_list", (Func<DynamicClass, List<object?>>)((@class) =>
        {
            return @class.Variables.Select(x => x.Value.Value).ToList();
        })),
    ];
}