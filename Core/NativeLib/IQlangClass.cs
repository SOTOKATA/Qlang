namespace Core.NativeLib;

public interface IQlangClass
{
    public string Name { get; init; }
    
    public List<(string name, Delegate body)> GetFunctions();
}