using Core.Exceptions;

namespace Core.NativeLib.SystemLib.Classes;

public class ExceptionClass : IQlangClass
{
    public string Name { get; init; } = "exception";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("throw", (Action<string>)(msg => throw new QlangRuntimeException(msg, null)))
        ];
    }
}