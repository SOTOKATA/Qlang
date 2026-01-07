using Core.AST;
using Core.Dynamic;

namespace Core.NativeLib.SystemLib.Classes;

public class MetaClass : IQlangClass
{
    public string Name { get; init; } = "meta";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("get_method_list_of", (Func<DynamicClass, List<object>>)((@class) =>
            {
                var functions = @class.Body.OfType<FunctionNode>().Select(f => f.Name).ToList<object>();

                return functions;
            })),
            
            ("get_variable_list_of", (Func<DynamicClass, List<object>>)((@class) =>
            {
                var variables = @class.Variables.Keys.ToList<object>();

                return variables;
            })),
            
            ("get_info_of", (Func<DynamicClass, List<object>>)((@class) =>
            {
                List<object> output = [@class.ClassName];
                
                var functions = @class.Body.OfType<FunctionNode>().Select(f => f.Name).ToList<object>();
                var variables = @class.Variables.Keys.ToList<object>();
                
                output.AddRange(functions);
                output.AddRange(variables);

                return output;
            })),
            
            ("is_dynamic_class", (Func<object?, bool>)(@class => @class is DynamicClass))
        ];
    }
}