namespace Core.Exceptions;

public class QlangProgramException(string msg, bool writeStackTrace) : Exception(msg)
{
    public readonly bool WriteStackTrace = writeStackTrace;
}