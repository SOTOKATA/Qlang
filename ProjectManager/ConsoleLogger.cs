namespace ProjectManager;

public static class ConsoleLogger
{
    private static void _Log(string message, string action = "", ConsoleColor color = default)
    {
        var lines = message.Split('\n');

        foreach (var line in lines)
        {
            if (action != "")
            {
                Console.ForegroundColor = color;
                Console.Write(action + ": ");
                Console.ResetColor();
            }

            Console.WriteLine(line);
        }
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