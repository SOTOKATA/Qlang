using Core.Exceptions;

namespace Core.NativeLib.SystemLib.Classes;

public class ExceptionClass : IQlangClass
{
    public string Name { get; init; } = "exception";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("throw", (Action<string, bool>)((msg, writeStackTrace) => throw new QlangProgramException(msg, writeStackTrace)))
        ];
    }
}