using System.Globalization;
using System.Reflection;
using Core;
using Core.Exceptions;
using Core.Native;
using Core.NativeLib;

namespace Runtime;

public static class Program
{
    public static void Main(string[] args)
    {
        
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        var programPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath) ?? "", $"{Path.GetFileNameWithoutExtension(Environment.ProcessPath)}.resource.qli");
        
        if (!File.Exists(programPath))
        {
            Console.WriteLine($"File '{programPath}' does not exist.");
            return;
        }

        var qliProgram = Core.MessagePack.Deserialize<QLIProgram>(Brotli.Decompress(File.ReadAllBytes(programPath)));

        if (qliProgram is null)
        {
            Console.WriteLine("Program is corrupted or not valid");
            return;
        }

        try
        {
            new Interpreter.Interpreter(qliProgram.StringList,
                qliProgram.NumberList,
                LoadDependencies(), qliProgram.SourceFileTable, qliProgram.DebugTable).Execute(qliProgram.ProgramNode, args.ToList()!);
        }
        catch (QlangRuntimeException runtime)
        {
            Console.WriteLine("Runtime handled exception:");
            Console.WriteLine(runtime);
        }
        catch (Exception e)
        {
            Console.WriteLine("Runtime unhandled exception:");
            Console.WriteLine(e);
        }
    }

    private static NativeFunctionRegistry LoadDependencies()
    {
        var dirPath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath) ?? "", $"{Path.GetFileNameWithoutExtension(Environment.ProcessPath)}.external.qli");
        
        // Console.WriteLine("Path to load dependencies: " + dirPath);
        
        // Case if native libs is not exists
        if (!Directory.Exists(dirPath))
            return new  NativeFunctionRegistry();

        var nativeLibRegister = new NativeFunctionRegistry();
        
        foreach (var dir in Directory.GetDirectories(dirPath))
        {
            var dirDepsPath = Path.Combine(dir, "deps");
        
            var dirDepsExists = Directory.Exists(dirDepsPath);

            var filesToAdd = new List<string>();
        
            // Add dependencies
            if (dirDepsExists)
                filesToAdd.AddRange(Directory.GetFiles(dirDepsPath, "*.dll", SearchOption.AllDirectories));
        
            // Add lib (main) files
            filesToAdd.AddRange(Directory.GetFiles(dir, "*.dll", SearchOption.TopDirectoryOnly));
            
            foreach (var file in filesToAdd)
            {
                var ctx = new LibLoadContext(file);
                var asm = ctx.LoadFromAssemblyPath(file);
                
                foreach (var type in asm.GetTypes()
                             .Where(t => typeof(IQlangLib).IsAssignableFrom(t) && !t.IsAbstract))
                    if (Activator.CreateInstance(type) is IQlangLib lib)
                        nativeLibRegister.RegisterLib(lib);
            }
        }
        
        return nativeLibRegister;
    }
}