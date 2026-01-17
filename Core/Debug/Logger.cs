using System.Diagnostics;

namespace Core.Debug;

public static class Logger
{
    public static bool Debug;

    private static FileLogger FileLogger;

    public static void Initialize(bool debug)
    {
        Debug = debug;
        FileLogger.Debug = debug;
    }
    
    public static void SetLoggerPath(string path)
    {
        FileLogger = new FileLogger(path);
    }

    private static void _Log(string message, bool isInternal = false, string prefix = "", string type = "")
    {
        if (!Debug)
            return;
        
        message = GetStackPath(prefix, type, isInternal ? 4 : 3) + message;
        FileLogger.Log(message);
    }
    
    public static void Log(string message, string prefix = "")
    {
        _Log(message, false, prefix, "LOG");
    }

    public static void Error(string message, string msg = "")
    {
        _Log(message, true, msg, "ERR");
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