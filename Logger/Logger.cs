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
        if (!QLang.Settings.Debug)
            return;
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    public static void Error(string message)
    {
        if (!QLang.Settings.Debug)
            return;
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}