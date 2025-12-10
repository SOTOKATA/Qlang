using Qlang.Core.Lang.AST;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.Lang.Interpreter.Native;

namespace Qlang.Core.Lang;

public class QLang
{
    private ProgramNode? _programNode;
    private Dictionary<string, string> _stringDictionary = [];
    private Dictionary<string, object> _numberDictionary = [];
    private NativeFunctionRegistry _nativeFunctions;
    
    public bool Compile(string path)
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

    public void Run(List<string?>? args)
    {
        if (_programNode == null)
            throw new Exception("Program Node is null (program is not compiled)");
        
        Interpreter.Interpreter interpreter = new(_stringDictionary, _numberDictionary, _nativeFunctions);
        
        interpreter.Execute(_programNode, args);
    }
}