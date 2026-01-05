using System.Globalization;
using System.Reflection;
using Core;
using Core.Native;
using Core.NativeLib;

namespace Runtime;

public class Program
{
    public static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        var programPath = Path.Combine(Directory.GetCurrentDirectory(), $"{Path.GetFileNameWithoutExtension(Environment.ProcessPath)}.resource.qli");
        
        if (!File.Exists(programPath))
        {
            Console.WriteLine($"File '{programPath}' does not exist.");
            return;
        }

        var content = File.ReadAllText(programPath);
        
        var qliProgram = Json.Deserialize<QLIProgram>(content);

        if (qliProgram is null)
        {
            Console.WriteLine("Program is corrupted or not valid");
            return;
        }

        new Interpreter.Interpreter(qliProgram.StringDictionary, 
            qliProgram.NumberDictionary, 
            LoadDependencies()).Execute(qliProgram.ProgramNode, args.ToList());
    }

    private static NativeFunctionRegistry LoadDependencies()
    {
        string dirPath = $"{Path.GetFileNameWithoutExtension(Environment.ProcessPath)}.external.qli";
        
        // Console.WriteLine("Path to load dependencies: " + dirPath);
        
        // Case if native libs is not exists
        if (!Directory.Exists(dirPath))
            return new  NativeFunctionRegistry();
        
        string dirDepsPath = Path.Combine(dirPath, "dependents");
        
        bool dirDepsExists = Directory.Exists(dirDepsPath);

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