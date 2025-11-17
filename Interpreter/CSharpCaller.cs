using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections.Concurrent;

namespace Qlang.Interpreter;

public class CSharpCaller
{
    private static readonly ConcurrentDictionary<string, Script<object>> ScriptCache = new();
    private static readonly ScriptOptions Options;
    
    static CSharpCaller()
    {
        // Настройка один раз при старте
        Options = ScriptOptions.Default
            .WithReferences(
                typeof(object).Assembly,
                typeof(Console).Assembly,
                typeof(System.Linq.Enumerable).Assembly
            )
            .WithImports(
                "System",
                "System.Linq",
                "System.Collections.Generic",
                "System.Text"
            )
            .WithOptimizationLevel(Microsoft.CodeAnalysis.OptimizationLevel.Release);
        
        // Прогрев JIT - первый запуск всегда медленный
        try
        {
            CSharpScript.EvaluateAsync("1+1", Options).Wait();
        }
        catch
        {
            // ignored
        }
    }
    
    public static object Call(string code)
    {
        var script = ScriptCache.GetOrAdd(code, 
            c => CSharpScript.Create<object>(c, Options));
        
        return script.RunAsync().Result.ReturnValue;
    }
    
    public object CallOnce(string code)
    {
        return CSharpScript.EvaluateAsync(code, Options).Result;
    }
}