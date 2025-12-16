using Qlang.Core.Lang.AST;
using Qlang.Core.Lang.Dynamic;
using Qlang.Core.ProjectManager.Project;

namespace Qlang.NativeLib.SystemLib;

public class MetaLib : IQlangLib
{
    public string Name { get; } = "MetaLib";
    public string Version { get; } = Project.Version;
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "meta";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("get_method_list_of", (Func<DynamicClass, List<object>>)((@class) =>
            {
                List<object> functions = @class.Body.OfType<FunctionNode>().Select(f => f.Name).ToList<object>();

                return functions;
            })),
            
            ("get_variable_list_of", (Func<DynamicClass, List<object>>)((@class) =>
            {
                List<object> variables = @class.Variables.Keys.ToList<object>();

                return variables;
            })),
            
            ("get_info_of", (Func<DynamicClass, List<object>>)((@class) =>
            {
                List<object> output = [@class.ClassName];
                
                List<object> functions = @class.Body.OfType<FunctionNode>().Select(f => f.Name).ToList<object>();
                List<object> variables = @class.Variables.Keys.ToList<object>();
                
                output.AddRange(functions);
                output.AddRange(variables);

                return output;
            })),
            
            ("is_dynamic_class", (Func<object?, bool>)(@class => @class is DynamicClass))
        ];
    }
}