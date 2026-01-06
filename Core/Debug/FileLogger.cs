namespace Core.Debug;

public class FileLogger
{
    private string _filePath;

    public static bool Debug = false;
    
    // Disabled because of using function SetPath what set's path
    #pragma warning restore CS8618 
    #pragma warning disable CS8618
    public FileLogger(string filePath, bool append = false)
    {
        if (Debug)
            SetPath(filePath, append);
    }

    public void SetPath(string path, bool append = false)
    {
        if (!Debug)
            return;
        
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
        if (!Debug)
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