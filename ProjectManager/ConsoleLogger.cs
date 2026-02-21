namespace ProjectManager;

public static class ConsoleLogger
{
    private static void _Log(string message, string action = "", ConsoleColor color = default)
    {
        action = action.ToLower();
        var lines = message.Split('\n');

        foreach (var line in lines)
        {
            if (action != "" && !string.IsNullOrWhiteSpace(line))
            {
                Console.ForegroundColor = color;
                Console.Write(action + ": ");
                Console.ResetColor();
            }

            Console.WriteLine(line);
        }
    }

    // Write with unhandled action
    public static void Log(string action, string message, ConsoleColor color = default)
    {
        _Log(action, message, color);
    }

    public static void Info(string message)
    {
        _Log(message, "INFO",  ConsoleColor.Blue);
    }
    
    public static void Error(string message)
    {
        _Log(message, "ERROR",  ConsoleColor.Red);
    }
    
    public static void Get(string message)
    {
        _Log(message, "GET",  ConsoleColor.Green);
    }
    
    public static void Set(string message)
    {
        _Log(message, "SET",  ConsoleColor.Blue);
    }
}