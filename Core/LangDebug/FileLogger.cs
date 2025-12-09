using Qlang.Core.ProjectManager.Project;

namespace Qlang.Core.LangDebug;

public class FileLogger
{
    private string _filePath;
    
    // Disabled because of using function SetPath what set's path
    #pragma warning restore CS8618 
    #pragma warning disable CS8618
    public FileLogger(string filePath, bool append = false)
    {
        SetPath(filePath, append);
    }

    public void SetPath(string path, bool append = false)
    {
        _filePath = path;
        
        if (!Directory.Exists(Path.GetDirectoryName(path)))
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        if (!File.Exists(path))
            File.Create(_filePath).Close();
        
        if (!append)
            File.WriteAllText(_filePath, "");
    }

    public void Log(string message)
    {
        object? value = Project.GetCompileSetting("debug");
        if (value is null || !(bool)value)
            return;
        
        using StreamWriter writer = new(_filePath, true);
        
        writer.WriteLine(message);
        
        writer.Close();
    }

    public void Clear()
    {
        File.WriteAllText(_filePath, "");
    }
}