namespace Qlang;

public static class ExceptionManager
{
    public static void Throw(Exception ex, bool @throw = false)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex);
        Console.ResetColor();

        if (@throw)
            throw ex;
    }
    
    public static void ThrowMessage(string message, bool @throw = false)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();

        if (@throw)
            throw new Exception(message);
    }
}