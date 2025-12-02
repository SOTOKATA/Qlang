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
}