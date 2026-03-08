using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Core;
using Core.AST;
using Core.Exceptions;
using Core.Native;
using Core.NativeLib;

namespace Runtime;

public static class Program
{
    public static void Main(string[] args)
    {
        
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        var processDir = Path.GetDirectoryName(Environment.ProcessPath);
        var programPath = Path.Combine(processDir ?? "", $"{Path.GetFileNameWithoutExtension(Environment.ProcessPath)}.resource.qli");
        var debugPath = Path.Combine(processDir ?? "", $"{Path.GetFileNameWithoutExtension(Environment.ProcessPath)}.debug.qli");
        
        if (!File.Exists(programPath))
        {
            Console.WriteLine($"File '{programPath}' does not exist.");
            return;
        }

        var qliProgram = Core.MessagePack.Deserialize<QLIProgram>(Brotli.Decompress(File.ReadAllBytes(programPath)));
        
        var qliDebug = new QLIDebug
        {
            SourceFileTable = null,
            DebugTable = null
        };

        if (File.Exists(debugPath))
            qliDebug = Core.MessagePack.Deserialize<QLIDebug>(Brotli.Decompress(File.ReadAllBytes(debugPath)));
        
        try
        {
            new Interpreter.Interpreter(
                qliProgram.NumberList,
                LoadDependencies(), 
                qliDebug.SourceFileTable, 
                qliDebug.DebugTable, 
                qliProgram.StringPoolTable)
                    .Execute(qliProgram.ProgramNode, args.ToList()!);
        }
        catch (QlangRuntimeException runtime)
        {
            Console.WriteLine(runtime);
        }
        catch (QlangProgramException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private static NativeFunctionRegistry LoadDependencies()
    {
        var dirPath = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath) ?? "",
            $"{Path.GetFileNameWithoutExtension(Environment.ProcessPath)}.external.qli"
        );

        if (!Directory.Exists(dirPath))
            return new NativeFunctionRegistry();

        var nativeLibRegister = new NativeFunctionRegistry();

        foreach (var dir in Directory.GetDirectories(dirPath))
        {
            var mainFiles = Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly);

            if (mainFiles.Length == 0)
                continue;

            var entryDll = mainFiles[0];
            var ctx = new LibLoadContext(entryDll);

            var depsPath = Path.Combine(dir, "deps");
            if (Directory.Exists(depsPath))
            {
                foreach (var dep in Directory.GetFiles(depsPath, "*.dll", SearchOption.AllDirectories))
                {
                    try { ctx.LoadFromAssemblyPath(dep); }
                    catch { /* dep уже загружен или не нужен */ }
                }
            }

            foreach (var file in mainFiles)
            {
                var asm = ctx.LoadFromAssemblyPath(file);

                var types = asm.GetTypes()
                    .Where(t => typeof(IQlangLib).IsAssignableFrom(t) && !t.IsAbstract).ToList();

                if (types.Count == 0)
                    throw new Exception($"""
                                        Cannot load native C# library. IQlangLib override was not found.
                                        Make sure that Core.dll (QlangCore) does not exist in the dependencies or in the main files.
                                        Main .dll path: {entryDll}
                                        """);
                
                foreach (var type in types)
                {
                    try
                    {
                        var instance = Activator.CreateInstance(type);
                        if (instance is IQlangLib lib)
                            nativeLibRegister.RegisterLib(lib);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[QlangLoader] Failed to instantiate {type.FullName}: {ex.Message}");
                    }
                }
            }
        }

        return nativeLibRegister;
    }
}