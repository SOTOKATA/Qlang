using Core.NativeLib;

namespace NumericLib;

public class NumericLib : IQlangLib
{
    public string Name { get; } = "Numeric";
    public string Version { get; } = "1.0";
    public string Author { get; } = "You";
    public List<IQlangNamespace> Namespaces { get; set; } = [new NumericNamespace()];
}