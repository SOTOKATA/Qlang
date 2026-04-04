using Core.AST;
using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class MetaClass : IQlangClass
{
    public string Name { get; init; } = "Meta";

    public List<(string name, Delegate body)> GetFunctions() => [
        ("FnList", (Func<DynamicClass, List<object?>>)((@class) =>
        {
            return @class.Body.OfType<FunctionNode>().Select(x => x.NameId).Cast<object?>().ToList();
        })),
        
        ("VarNameList", (Func<DynamicClass, List<object?>>)((@class) =>
        {
            return @class.Variables.Select(x => x.Value.Name).Cast<object?>().ToList();
        })),
        
        ("VarValueList", (Func<DynamicClass, List<object?>>)((@class) =>
        {
            return @class.Variables.Select(x => x.Value.Value).ToList();
        })),
        
        ("VarTypeList", (Func<DynamicClass, List<object?>>)((@class) =>
        {
            return @class.Variables.Select(x => x.Value.IsConst ? Keywords.ConstVariableDeclaration : Keywords.VariableDeclaration).Cast<object?>().ToList();
        })),
    ];
}