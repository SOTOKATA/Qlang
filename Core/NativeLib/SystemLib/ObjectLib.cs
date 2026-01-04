namespace Core.NativeLib.SystemLib;

public class ObjectLib : IQlangLib
{
    public string Name { get; } = "ObjectLib";
    public string Version { get; } = "0.0.0";
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "object";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("is_null",  (Func<object?, bool>)(obj => obj is null)),
            ("is_simplify",  (Func<object?, bool>)(obj => obj is float or double or int or long or decimal or string or bool or List<object> or List<object?>))
        ];
    }
}