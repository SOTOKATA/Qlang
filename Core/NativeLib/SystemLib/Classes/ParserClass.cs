using System.Runtime.CompilerServices;

namespace Core.NativeLib.SystemLib.Classes;

public class ParserClass : IQlangClass
{
    public string Name { get; init; } = "parser";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("int", (Func<object?, int>)(obj => (int)Parse(obj, "int")!)),
            ("float", (Func<object, double>)(obj => (double)Parse(obj, "float")!)),
            ("number", (Func<object, double>)(obj => (double)Parse(obj, "double")!)),
            ("string", (Func<object, string>)(obj => (string)Parse(obj, "string")!)),
        ];
    }
    
    private static object? Parse(object? obj, string toParse)
    {
        if (obj is null)
        {
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
        catch
        {
            return null;
        }
    }
}