using System.Diagnostics;

namespace Core.Debug;

public static class Logger
{
    public static bool Debug = false;
    
    private static readonly FileLogger FileLogger = new("Logs\\debug.log");

    public static void SetLoggerPath(string path)
    {
        FileLogger.SetPath(path);
    }

    private static void _Log(string message, ConsoleColor? color = null, bool isInternal = false, string msg = "", string type = "")
    {
        if (!Debug)
            return;
        
        message = GetStackPath(msg, type, isInternal ? 4 : 3) + message;
        FileLogger.Log(message);
    }
    
    public static void Log(string message, string msg = "", ConsoleColor? color = null)
    {
        _Log(message, color, false, msg, "LOG");
    }

    public static void Warn(string message, string msg = "")
    {
        _Log(message, ConsoleColor.DarkYellow, true, msg, "WARN");
    }
    
    public static void Succ(string message, string msg = "")
    {
        _Log(message, ConsoleColor.DarkGreen,  true, msg, "SUCC");
    }
    
    public static void Error(string message, string msg = "")
    {
        _Log(message, ConsoleColor.Red, true, msg, "ERROR");
    }

    private static string GetStackPath(string msg = "", string type = "", int depth = 3)
    {
        var frame = new StackTrace(true).GetFrame(depth);

        if (frame == null)
            return $"{type} [null/null{(msg != "" ? $" {msg}" : "")}]: ";
        
        var method = frame.GetMethod();
        
        if (method == null)
            return $"{type} [null/null{(msg != "" ? $" {msg}" : "")}]: ";

        return $"{type} [{method.DeclaringType?.Name}/{method.Name}{(msg != "" ? $" {msg}" : "")}]: ";
    }
}