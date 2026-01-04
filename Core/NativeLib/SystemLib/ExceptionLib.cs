using Core.Exceptions;

namespace Core.NativeLib.SystemLib;

public class ExceptionLib : IQlangLib
{
    public string Name { get; } = "ExceptionLib";
    public string Version { get; } = "0.0.0";
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "exception";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("throw", (Action<string>)(msg => throw new QlangRuntimeException(msg, null)))
        ];
    }
}