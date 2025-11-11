using Qlang.AST;

namespace Qlang;

public class QLang
{
    private ProgramNode? _programNode;
    private Dictionary<string, string> _stringDictionary = [];

    public static Settings Settings;

    public QLang()
    {
        var settings = Settings.Load();
        
        Settings = settings ?? new Settings();
        
        Settings.Save();
    }

    public void SetSettings(string name, string? value)
    {
        // TODO: add settings change 
        
        switch (name)
        {
            case "debug":
                if (value is null)
                {
                    Console.WriteLine($"Current value of \"{name}\" is: " + Settings.Debug);
                    return;
                }
                
                var @bool = bool.Parse(value);
                Settings.Debug = @bool;
                Settings.Save();
                
                Console.WriteLine($"Current value of \"{name}\" is: " + Settings.Debug);
                break;
        }
    }
    
    public void Compile(string code)
    {
        Compiler.Compiler c = new();
        
        _programNode = c.Compile(code);

        _stringDictionary = c.StringDictionary;
    }

    public void Run()
    {
        if (_programNode == null)
            throw new Exception("Program Node is null (program is not compiled)");
        
        Interpreter.Interpreter interpreter = new(_stringDictionary);
        
        interpreter.Execute(_programNode);
    }
}