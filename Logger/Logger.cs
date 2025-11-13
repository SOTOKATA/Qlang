namespace Qlang.Logger;

public static class Logger
{
    public static void Log(string message, ConsoleColor? color = null)
    {
        if (!QLang.Settings.Debug)
            return;
        
        Console.ForegroundColor = color ?? ConsoleColor.DarkGray;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void Warn(string message)
    {
        Log("WARN: " + message, ConsoleColor.DarkYellow);
    }
    
    public static void Succ(string message)
    {
        Log("SUCC: " + message, ConsoleColor.DarkGreen);
    }
    
    public static void Error(string message)
    {
        Log("ERROR: " + message, ConsoleColor.Red);
    }
}