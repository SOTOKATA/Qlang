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

        QLIProgram? qliProgram;

        // If trying to read .qli file as JSON will throw exception, will use GZip decompress
        try
        {
            qliProgram = Json.Deserialize<QLIProgram>(File.ReadAllText(programPath));
        }
        catch
        {
            qliProgram = Json.Deserialize<QLIProgram>(GZip.Decompress(File.ReadAllBytes(programPath)));
        }

        if (qliProgram is null)
        {
            Console.WriteLine("Program is corrupted or not valid");
            return;
        }

        try
        {
            new Interpreter.Interpreter(qliProgram.StringList,
                qliProgram.NumberList,
                LoadDependencies(), qliProgram.SourceFileTable).Execute(qliProgram.ProgramNode, args.ToList()!);
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
        
        var dirDepsPath = Path.Combine(dirPath, "dependents");
        
        var dirDepsExists = Directory.Exists(dirDepsPath);

        var filesToAdd = new List<string>();
        
        // Add dependencies
        if (dirDepsExists)
            filesToAdd.AddRange(Directory.GetFiles(dirDepsPath, "*.dll", SearchOption.AllDirectories));
        
        // Add lib (main) files
        filesToAdd.AddRange(Directory.GetFiles(dirPath, "*.dll", SearchOption.TopDirectoryOnly));

        var nativeLibRegister = new NativeFunctionRegistry();
        
        foreach (var file in filesToAdd)
        {
            var assembly = Assembly.LoadFrom(file);

            var types = assembly.GetTypes();

            var libTypes = types
                .Where(t => typeof(IQlangLib).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

            foreach (var type in libTypes)
            {
                var nativeLib = Activator.CreateInstance(type) as IQlangLib;
                // If lib exists and is not corrupted: add
                nativeLibRegister.RegisterLib(nativeLib);
            }
            
            // Console.WriteLine("Loaded dependence: " + file);
        }
        
        return nativeLibRegister;
    }
}