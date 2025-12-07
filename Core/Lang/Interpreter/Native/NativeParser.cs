using System.Runtime.CompilerServices;
using Qlang.Core.LangDebug;

namespace Qlang.Core.Lang.Interpreter.Native;

public class NativeParser
{
    public static object? Parse(object? obj, string toParse)
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