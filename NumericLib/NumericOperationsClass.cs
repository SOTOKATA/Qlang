using Core.NativeLib;

namespace NumericLib;

public class NumericOperationsClass : IQlangClass
{
    public string Name { get; init; } = "operations";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return [
            ("sum", (Func<double, double, double>)((a, b) => a + b))
        ];
    }
}