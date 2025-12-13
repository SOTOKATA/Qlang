using System.Runtime.CompilerServices;
using Qlang.Core.Lang.Interpreter.Native;
using Qlang.Core.LangDebug;
using Qlang.Core.ProjectManager.Project;

namespace Qlang.NativeLib.SystemLib;

public class ParserLib : IQlangLib
{
    public string Name { get; } = "ParserLib";
    public string Version { get; } = Project.Version;
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "parser";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("int", (Func<object?, int>)(obj => (int)Parse(obj, "int"))),
            ("float", (Func<object, double>)(obj => (double)Parse(obj, "float"))),
            ("number", (Func<object, double>)(obj => (double)Parse(obj, "double"))),
            ("string", (Func<object, string>)(obj => (string)Parse(obj, "string"))),
        ];
    }
    
    private static object? Parse(object? obj, string toParse)
    {
        if (obj is null)
        {
            Logger.Error("Object is null");
            return null;
        }

        try
        {
            return toParse switch
            {
                "int" => Convert.ToInt32(obj),
                "float" or "double" => Convert.ToDouble(obj),
                "string" => obj.ToString(),
                _ => throw new SwitchExpressionException($"Undefined parse: '{toParse}'")
            };
        }
        catch (Exception e)
        {
            Logger.Error(e.ToString());
            return null;
        }
    }
}