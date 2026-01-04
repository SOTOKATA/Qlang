namespace Core.NativeLib;

public interface IQlangLib
{
    public string Name { get; }
    public string Version { get; }
    public string Author { get; }
    public string Class { get; }
    public string Namespace { get; }

    public List<(string name, Delegate body)> GetFunctions();
}