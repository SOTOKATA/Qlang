namespace Core;

public class QLIProgramLib
{
    public string Name { get; } = Guid.NewGuid().ToString();
    public List<string> MainFilePaths { get; set; } = [];
    public List<string> DependenciesFilePaths { get; set; } = [];
}