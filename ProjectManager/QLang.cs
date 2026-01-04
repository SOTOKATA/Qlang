using Core;
using Core.AST;
using Core.Native;

namespace ProjectManager;

public class QLang
{
    private ProgramNode? _programNode;
    private Dictionary<string, string> _stringDictionary = [];
    private Dictionary<string, object> _numberDictionary = [];
    private NativeFunctionRegistry _nativeFunctions;

    public bool Compile(string path, string? filename = null)
    {
        string code = File.ReadAllText(path);
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
        _nativeFunctions = c.NativeFunctions;
        
        var dirName = Path.GetDirectoryName(path);
        if (filename != null)
            path = dirName is null ? filename : Path.Combine(dirName, filename);
        
        SaveProgram(new QLIProgram()
        {
            ProgramNode = _programNode,
            StringDictionary = _stringDictionary,
            NumberDictionary = _numberDictionary,
            NativeFunctions = _nativeFunctions,
        }, path);

        return true;
    }

    private static void SaveProgram(QLIProgram qliProgram, string filePath)
    {
        var json = Json.Serialize(qliProgram);

        var pathToFile = Path.GetDirectoryName(filePath);
        
        if (!Directory.Exists(Path.Combine(pathToFile ?? "", "build")))
            Directory.CreateDirectory(Path.Combine(pathToFile ?? "", "build"));
        
        var path = Path.Combine(pathToFile ?? "", "build", Path.GetFileNameWithoutExtension(filePath) + ".resource.qli");
        
        if (!File.Exists(path))
            File.Create(path).Close();
        
        File.Copy(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "qli.exe"), Path.Combine(pathToFile ?? "", "build", Path.GetFileNameWithoutExtension(filePath) + ".exe"), true);
        
        File.WriteAllText(path, json);
    }

    public void Run(List<string?>? args)
    {
        if (_programNode == null)
            throw new Exception("Program Node is null (program is not compiled)");
        
        global::Interpreter.Interpreter.Interpreter interpreter = new(_stringDictionary, _numberDictionary, _nativeFunctions);
        
        interpreter.Execute(_programNode, args);
    }
}