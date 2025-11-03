using Qlang.AST;

namespace Qlang;

public class QLang
{
    private Compiler.Compiler _compiler = new();
    private Interpreter _interpreter;
    
    private ProgramNode? _programNode;
    private Dictionary<string, string> _stringDictionary = [];
    
    public void Compile(string code)
    {
        _programNode = _compiler.Compile(code);

        _stringDictionary = _compiler.StringDictionary;
    }

    public void Run()
    {
        if (_programNode == null)
            return;
        
        _interpreter = new Interpreter(_stringDictionary);
        
        _interpreter.Execute(_programNode);
    }
}