namespace Core.NativeLib;

public interface IQlangLib
{
    public string Name { get; }
    public string Version { get; }
    public string Author { get; }

    public List<IQlangNamespace> Namespaces { get; set; }
}