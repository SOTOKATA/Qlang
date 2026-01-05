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

    private static void SaveDependencies(QLIProgram qliProgram, string path, string filename)
    {
        string dirPath =  Path.Combine(path ?? "", filename + ".external.qli");
        string dirDepsPath = Path.Combine(dirPath, "dependents");
        
        if (Directory.Exists(dirPath))
            Directory.Delete(dirPath, true);
        
        Directory.CreateDirectory(dirPath);
        Directory.CreateDirectory(dirDepsPath);

        var externalLibs = qliProgram.ExternalLibraries;
        qliProgram.ExternalLibraries = [];

        foreach (var lib in externalLibs)
        {
            foreach (var depsFile in lib.DependenciesFilePaths)
                File.Copy(depsFile, Path.Combine(dirDepsPath, Path.GetFileName(depsFile)), true);
            foreach (var mainFile in lib.MainFilePaths)
                File.Copy(mainFile, Path.Combine(dirPath, Path.GetFileName(mainFile)), true);  
        }
    }

    private static void SaveProgram(QLIProgram qliProgram, string filePath)
    {
        var json = Json.Serialize(qliProgram);

        var pathToFile = Path.GetDirectoryName(filePath);
        
        if (!Directory.Exists(Path.Combine(pathToFile ?? "", "build")))
            Directory.CreateDirectory(Path.Combine(pathToFile ?? "", "build"));

        SaveDependencies(qliProgram, Path.Combine(pathToFile ?? "", "build"), Path.GetFileNameWithoutExtension(filePath));
        
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