namespace Qlang.Interpreter;

public class NativeConsole
{
    public static void SetForegroundColor(string line)
    {
        if (Enum.TryParse(line, true, out ConsoleColor color))
            Console.ForegroundColor = color;
    }
    
    public static void SetBackgroundColor(string line)
    {
        if (Enum.TryParse(line, true, out ConsoleColor color))
            Console.ForegroundColor = color;
    }
}