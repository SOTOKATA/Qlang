using System.Runtime.CompilerServices;

namespace Core.NativeLib.SystemLib.Classes;

public class ParserClass : IQlangClass
{
    public string Name { get; init; } = "Parser";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("Int", (Func<object?, int>)(obj => (int)Parse(obj, "int")!)),
            ("Float", (Func<object, double>)(obj => (double)Parse(obj, "float")!)),
            ("Number", (Func<object, double>)(obj => (double)Parse(obj, "double")!)),
            ("String", (Func<object, string>)(obj => (string)Parse(obj, "string")!)),
        ];
    }
    
    private static object? Parse(object? obj, string toParse)
    {
        if (obj is null)
            return null;

        return toParse switch
        {
            "int" => Convert.ToInt32(obj),
            "float" or "double" => Convert.ToDouble(obj),
            "string" => obj.ToString(),
            _ => throw new SwitchExpressionException($"Undefined parse: '{toParse}'")
        };
    }
}