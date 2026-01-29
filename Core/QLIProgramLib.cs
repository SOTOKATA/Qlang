using MessagePack;

namespace Core;

[MessagePackObject]
public class QLIProgramLib
{
    [Key(1)]
    public string Name { get; } = Guid.NewGuid().ToString();
    [Key(2)]
    public List<string> MainFilePaths { get; set; } = [];
    [Key(3)]
    public List<string> DependenciesFilePaths { get; set; } = [];
}