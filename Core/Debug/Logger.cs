using System.Diagnostics;

namespace Core.Debug;

public static class Logger
{
    public static bool Debug;

    private static FileLogger _fileLogger = null!;

    public static void Initialize(bool debug)
    {
        Debug = debug;
        FileLogger.Debug = debug;
    }
    
    public static void SetLoggerPath(string path)
    {
        _fileLogger = new FileLogger(path);
    }

    private static void _Log(string message, bool isInternal = false, string prefix = "", string type = "")
    {
        if (!Debug)
            return;
        
        message = GetStackPath(prefix, type, isInternal ? 4 : 3) + message;
        _fileLogger.Log(message);
    }
    
    public static void Log(string message, string prefix = "")
    {
        _Log(message, false, prefix, "LOG");
    }

    private static string GetStackPath(string msg = "", string type = "", int depth = 3)
    {
        var frame = new StackTrace(true).GetFrame(depth);

        if (frame == null)
            return $"{type} [{(msg != "" ? $" {msg}" : "")}]: ";
        
        var method = frame.GetMethod();
        
        if (method == null)
            return $"{type} [{(msg != "" ? $" {msg}" : "")}]: ";

        return $"{type} [{method.DeclaringType?.Name}/{method.Name}{(msg != "" ? $" {msg}" : "")}]: ";
    }
}