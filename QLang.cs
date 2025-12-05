using Qlang.AST;

namespace Qlang;

public class QLang
{
    private ProgramNode? _programNode;
    private Dictionary<string, string> _stringDictionary = [];
    private Dictionary<string, object> _numberDictionary = [];
    
    public bool Compile(string path)
    {
        string code = File.ReadAllText(path);
        Compiler.Compiler c = new();

        try
        {
            _programNode = c.Compile(code);
        }
        catch (Exception ex)
        {
            ExceptionManager.Throw(ex);
            
            return false;
        }

        _stringDictionary = c.StringDictionary;
        _numberDictionary = c.NumberDictionary;
        
        SaveProgram(_programNode, path);

        return true;
    }

    private static void SaveProgram(ProgramNode programNode, string filePath)
    {
        string json = Json.Serialize(programNode);

        string path = filePath + ".json";

        if (!File.Exists(path))
            File.Create(path).Close();

        File.WriteAllText(path, json);
    }

    public void SetProgramNode(ProgramNode programNode)
    {
        _programNode = programNode;
    }

    public void Run()
    {
        if (_programNode == null)
            throw new Exception("Program Node is null (program is not compiled)");
        
        Interpreter.Interpreter interpreter = new(_stringDictionary, _numberDictionary);
        
        interpreter.Execute(_programNode);
    }
}