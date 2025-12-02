using System.Text.Json;
using Newtonsoft.Json;
using Qlang.AST;

namespace Qlang;

public class QLang
{
    private ProgramNode? _programNode;
    private Dictionary<string, string> _stringDictionary = [];
    private Dictionary<string, object> _numberDictionary = [];

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
    
    public bool Compile(string code, string filePath)
    {
        Compiler.Compiler c = new();

        try
        {
            _programNode = c.Compile(code);
        }
        catch (Exception ex)
        {
            ExceptionManager.Throw(ex);
            
            Logger.Logger.Error("Error");

            return false;
        }

        _stringDictionary = c.StringDictionary;
        _numberDictionary = c.NumberDictionary;
        
        SaveProgram(_programNode, filePath);

        return true;
    }

    private static void SaveProgram(ProgramNode programNode, string filePath)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };

        string json = JsonConvert.SerializeObject(programNode, settings);

        string path = Path.Combine(Directory.GetCurrentDirectory(), filePath + ".json");

        if (!File.Exists(path))
            File.Create(path).Close();

        File.WriteAllText(path, json);
    }

    private static ProgramNode? LoadFromFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("File is not found", path);

        string json = File.ReadAllText(path);
        
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };

        return JsonConvert.DeserializeObject<ProgramNode>(json, settings);
    }

    public void Run()
    {
        if (_programNode == null)
            throw new Exception("Program Node is null (program is not compiled)");
        
        Interpreter.Interpreter interpreter = new(_stringDictionary, _numberDictionary);
        
        interpreter.Execute(_programNode);
    }
}