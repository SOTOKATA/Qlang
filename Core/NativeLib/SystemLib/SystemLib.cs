namespace Core.NativeLib.SystemLib;

public class SystemLib : IQlangLib
{
    public string Name { get; } = "SystemLib";
    public string Version { get; } = "0.0.1";
    public string Author { get; } = "SOTOKATA";
    public List<IQlangNamespace> Namespaces { get; set; }  = [ new SystemNamespace() ];
}