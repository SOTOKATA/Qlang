using Qlang.AST;

namespace Qlang.Project;

public partial class Project
{
    public static void SetCompileSetting(string param, object value)
    {
        try
        {
            CompileSettings.Set(param, value);
        }
        catch (Exception ex)
        {
            ExceptionManager.ThrowMessage(ex.Message);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("SET");
        Console.ResetColor();
        Console.WriteLine($" {param}: {value}");
        CompileSettings.Save();
    }

    public static object? GetCompileSetting(string param)
    {
        return CompileSettings.Get(param);
    }

    public void Compile()
    {
        // _qlang.Compile(_settings.GetString("main_file_path"));
    }

    public void Run()
    {
        // if (File.Exists(_settings.GetString("main_file_path") + ".json"))
        // {
        //     ProgramNode? compiled = Json.Deserialize<ProgramNode>(_settings.GetString("main_file_path") + ".json");
        //     
        //     if (compiled == null)
        //         throw new Exception("Program Node is null (program is not compiled)");
        //     
        //     _qlang.SetProgramNode(compiled);
        // }
        
        Console.WriteLine("IsNln@3: " + _qlang);
        Console.WriteLine("IsNln@4: " + Settings);
        
        if (_qlang.Compile(Path.Combine(Settings.GetString("path"), Settings.GetString("main_file_path"))))
            _qlang.Run();
    }
}