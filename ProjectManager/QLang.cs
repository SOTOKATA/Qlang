using Core;
using Core.AST;
using Core.Native;

namespace ProjectManager;

public class QLang
{
    private ProgramNode? _programNode;
    private Dictionary<string, string> _stringDictionary = [];
    private Dictionary<string, object> _numberDictionary = [];

    public bool Compile(string path, string? filename = null)
    {
        var code = File.ReadAllText(path);
        Compiler.Compiler c = new();

        try
        {
            _programNode = c.Compile(path, code);
        }
        catch (Exception ex)
        {
            ExceptionManager.Throw(ex);
            
            return false;
        }

        _stringDictionary = c.StringDictionary;
        _numberDictionary = c.NumberDictionary;
        
        var dirName = Path.GetDirectoryName(path);
        if (filename != null)
            path = dirName is null ? filename : Path.Combine(dirName, filename);
        
        SaveProgram(new QLIProgram
        {
            ProgramNode = _programNode,
            StringDictionary = _stringDictionary,
            NumberDictionary = _numberDictionary,
            ExternalLibraries = c.DllDependencies,
        }, path);

        return true;
    }

    private static void SaveProgram(QLIProgram qliProgram, string filePath)
    {
        var json = Json.Serialize(qliProgram);

        var pathToFile = Path.GetDirectoryName(filePath);
        
        if (!Directory.Exists(Path.Combine(pathToFile ?? "", "build")))
            Directory.CreateDirectory(Path.Combine(pathToFile ?? "", "build"));

        var newExternal = qliProgram.ExternalLibraries;
        for (int index = 0; index < qliProgram.ExternalLibraries.Count; index++)
        {
            QLIProgramLib? toCopy = qliProgram.ExternalLibraries[index];
            var newPath = Path.Combine(pathToFile ?? "", "build", "external");
            
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            for (var mainIndex = 0; mainIndex < toCopy.MainFilePaths.Count; mainIndex++)
            {
                var mainFile = toCopy.MainFilePaths[mainIndex];
                var mainPath = Path.Combine(newPath, Path.GetFileName(mainFile));

                File.Copy(mainFile, mainPath, true);

                newExternal[index].MainFilePaths[mainIndex] = mainPath;
            }

            for (var depIndex = 0; depIndex < toCopy.DependenciesFilePaths.Count; depIndex++)
            {
                var depFile = toCopy.DependenciesFilePaths[depIndex];
                var depPath = Path.Combine(newPath, "dependents", Path.GetFileName(depFile));

                if (!Directory.Exists(Path.Combine(newPath, "dependents")))
                    Directory.CreateDirectory(Path.Combine(newPath, "dependents"));

                File.Copy(depFile, depPath, true);

                newExternal[index].DependenciesFilePaths[depIndex] = depPath;
            }
        }

        Console.WriteLine("ExternalLib::Main\n" + string.Join("\n", qliProgram.ExternalLibraries));
        qliProgram.ExternalLibraries = newExternal;
        
        var path = Path.Combine(pathToFile ?? "", "build", Path.GetFileNameWithoutExtension(filePath) + ".resource.qli");
        
        if (!File.Exists(path))
            File.Create(path).Close();
        
        File.Copy(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "qli.exe"), Path.Combine(pathToFile ?? "", "build", Path.GetFileNameWithoutExtension(filePath) + ".exe"), true);
        
        File.WriteAllText(path, json);
    }

    public void Run(List<string?>? args)
    {
        // TODO: Runtime execution (by build/program.exe)
        if (_programNode == null)
            throw new Exception("Program Node is null (program is not compiled)");
        
        global::Interpreter.Interpreter interpreter = new(_stringDictionary, _numberDictionary, new NativeFunctionRegistry());
        
        interpreter.Execute(_programNode, args);
    }
}