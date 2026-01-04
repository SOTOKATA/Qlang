using System.Globalization;
using Core;

namespace Runtime;

public class Program
{
    public static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        var programPath = Path.Combine(Directory.GetCurrentDirectory(), $"{Path.GetFileNameWithoutExtension(Environment.ProcessPath)}.resource.qli");
        
        var programArgs = args.Skip(1).ToList();

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
        
        LoadDependencies(qliProgram.Dependencies);

        new Interpreter.Interpreter(qliProgram.StringDictionary, 
            qliProgram.NumberDictionary, 
            qliProgram.NativeFunctions).Execute(qliProgram.ProgramNode, programArgs);
    }

    private static void LoadDependencies(List<string> paths)
    {
        
    }
}