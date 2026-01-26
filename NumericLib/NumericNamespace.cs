using Core.NativeLib;

namespace NumericLib;

public class NumericNamespace : IQlangNamespace
{
    public string Name { get; set; } = "numeric";
    public List<IQlangClass> Classes { get; set; } = [new NumericOperationsClass()];
}