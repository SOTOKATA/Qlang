namespace ProjectManager;

public static class ExceptionManager
{
    public static void Throw(Exception ex, bool @throw = false)
    {
        ConsoleLogger.Error(ex.ToString());

        if (@throw)
            throw ex;
    }
    
    public static void ThrowMessage(string message, bool @throw = false)
    {
        ConsoleLogger.Error(message);

        if (@throw)
            throw new Exception(message);
    }
}