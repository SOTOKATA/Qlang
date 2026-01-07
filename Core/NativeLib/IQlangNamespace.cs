namespace Core.NativeLib;

public interface IQlangNamespace
{
    public string Name { get; set; }

    public List<IQlangClass> Classes { get; set; }
}