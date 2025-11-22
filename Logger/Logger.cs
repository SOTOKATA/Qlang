using System.Diagnostics;
using Qlang.Dependencies;

namespace Qlang.Logger;

public static class Logger
{
    private static readonly FileLogger FileLogger = new("Logs\\debug.txt");

    public static void SetLoggerPath(string path)
    {
        FileLogger.SetPath(path);
    }
    
    public static void _Log(string message, ConsoleColor? color = null, bool isInternal = false, string msg = "")
    {
        message = GetStackPath(msg, isInternal ? 4 : 3) + ": " + message;
        FileLogger.Log(message);
        
        if (!QLang.Settings.Debug)
            return;
        
        Console.ForegroundColor = color ?? ConsoleColor.DarkGray;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    public static void Log(string message, string msg = "", ConsoleColor? color = null)
    {
        _Log("Log: " + message, color, false, msg);
    }

    public static void Warn(string message, string msg = "")
    {
        _Log("WARN: " + message, ConsoleColor.DarkYellow, true, msg);
    }
    
    public static void Succ(string message, string msg = "")
    {
        _Log("SUCC: " + message, ConsoleColor.DarkGreen,  true, msg);
    }
    
    public static void Error(string message, string msg = "")
    {
        _Log("ERROR: " + message, ConsoleColor.Red, true, msg);
    }

    private static string GetStackPath(string msg = "", int depth = 3)
    {
        var frame = new StackTrace(true).GetFrame(depth);
        var method = frame.GetMethod();

        return $"({frame.GetFileLineNumber()}) {method.DeclaringType?.Name}/{method.Name}{(msg != "" ? $".{msg}" : "")}";
    }
}