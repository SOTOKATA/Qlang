using Core.Exceptions;

namespace Core.NativeLib.SystemLib.Classes;

public class ExceptionClass : IQlangClass
{
    public string Name { get; init; } = "Exception";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("Throw", (Action<string, bool>)((msg, writeStackTrace) => throw new QlangProgramException(msg, writeStackTrace)))
        ];
    }
}