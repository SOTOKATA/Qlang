using System.Globalization;
using Qlang.AST;
using Qlang.Dependencies.QlangDependencies;
using Qlang.Dependencies.QlangDependencies.Classes;
using Qlang.Dependencies.QlangDependencies.Functions;

namespace Qlang.Dynamic;

public class CSharp
{
    public static object? Execute(object? ln)
    {
        if (ln is not string line)
            throw new QlangRuntimeException($"Line is not a string", null);
        
        var args = GetArguments(line);

        if (args != null)
            line = line[..line.IndexOf('=')];

        line = line.ToLower().Trim();

        var className = line[..line.IndexOf('.')];
        Logger.Logger.Log("Interpreter.CSharp: ClassName = " + className);
        foreach (var @class in Namespace.GetClassList().Where(@class => @class.GetName().ToLower() == className))
        {
            line = line[(@class.GetName().Length + 1)..];

            var fn = @class.GetFunctions().Find(fn => fn.GetName() == line);
            if (fn == null)
                continue;
                    
            Logger.Logger.Log("Interpreter.CSharp: Function = " + line);

            return fn.Execute(args ?? []);
        }

        throw new QlangRuntimeException($"Internal csharp path is not founded. ({line})", null);
    }

    private static object[]? GetArguments(string line)
    {
        if (!line.Contains("="))
            return null;
        
        if (line.IndexOf('=') + 1 > line.Length)
            return null;
        
        line = line[(line.IndexOf('=') + 1)..];
        
        Logger.Logger.Log("Interpreter.CSharp: Arguments = " + line);

        return line.Split(',').Cast<object>().ToArray();
    }
}