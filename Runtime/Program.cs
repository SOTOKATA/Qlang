using System.Globalization;
using Core;
using Core.Native;

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
            LoadDependencies(qliProgram.ExternalLibraries)).Execute(qliProgram.ProgramNode, args.ToList());
    }

    private static NativeFunctionRegistry LoadDependencies(List<QLIProgramLib> deps)
    {
        var nativeLibRegister = new NativeFunctionRegistry();
        
        foreach (var dep in deps)
        {
            foreach (var depPath in dep.DependenciesFilePaths)
            {
                if (!File.Exists(depPath))
                    throw new FileNotFoundException(depPath);
                
                nativeLibRegister.LoadNativeLib(depPath);
            }
            
            foreach (var depPath in dep.MainFilePaths)
            {
                if (!File.Exists(depPath))
                    throw new FileNotFoundException(depPath);
                
                nativeLibRegister.LoadNativeLib(depPath);
            }
        }
        
        return nativeLibRegister;
    }
}