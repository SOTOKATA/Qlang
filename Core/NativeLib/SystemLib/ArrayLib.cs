using Core.Dynamic;

namespace Core.NativeLib.SystemLib;

public class ArrayLib : IQlangLib
{
    public string Name { get; } = "ArrayLib";
    public string Version { get; } = "0.0.0";
    public string Author { get; } = "SOTOKATA";
    public string Class { get; } = "array";
    public string Namespace { get; } = "lib";
    public List<(string name, Delegate body)> GetFunctions()
    {
        return
        [
            ("create", (Func<List<object>>)(() => [])),
            ("add", (Action<List<object>, object>)((list, item) => list.Add(item))),
            ("get", (Func<List<object>, int, object?>)((list, idx) => list[idx])),
            ("is", (Func<object?, bool>)(obj => obj is List<object>)),
            ("is_array", (Func<object?, bool>)(obj => obj is DynamicClass { ClassName: "Array" })),
            ("insert", (Action<List<object>, int, object?>)((list, pos, item) => list.Insert(pos, item))),
            ("set", (Action<List<object>, int, object>)((list, idx, val) => list[idx] = val)),
            ("count", (Func<List<object>, int>)(list => list.Count)),
            ("clear", (Action<List<object>>)(list => list.Clear())),
            ("contains", (Func<List<object>, object, bool>)((list, item) => list.Contains(item))),
            ("remove_at", (Action<List<object>, int>)((list, idx) => list.RemoveAt(idx))),
            ("index_of", (Func<List<object>, object, int>)((list, item) => list.IndexOf(item))),
        ];
    }
}