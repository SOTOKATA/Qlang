using Qlang.AST;

namespace Qlang.Project;

public partial class Project
{
    public static void SetCompileSetting(string param, object value)
    {
        _compileSettings.Set(param, value);
        Console.WriteLine($"SET {param}: {value}");
        _compileSettings.Save();
    }

    public static object? GetCompileSetting(string param)
    {
        return _compileSettings.Get(param);
    }

    public void Compile()
    {
        _qlang.Compile(_settings.GetString("main_file_path"));
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
        if (_qlang.Compile(_settings.GetString("main_file_path")))
            _qlang.Run();
    }
}