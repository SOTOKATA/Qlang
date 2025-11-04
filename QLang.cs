using Qlang.AST;

namespace Qlang;

public class QLang
{
    private ProgramNode? _programNode;
    private Dictionary<string, string> _stringDictionary = [];
    
    public void Compile(string code)
    {
        Compiler.Compiler c = new();
        
        _programNode = c.Compile(code);

        _stringDictionary = c.StringDictionary;
    }

    public void Run()
    {
        if (_programNode == null)
        {
            throw new Exception("Program Node is null (program is not compiled)");
            return;
        }
        
        Interpreter interpreter = new(_stringDictionary);
        
        interpreter.Execute(_programNode);
    }
}