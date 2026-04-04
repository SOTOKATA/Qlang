namespace Core.NativeLib.SystemLib.Classes;

public class ArrayClass : IQlangClass
{
    public string Name { get; init; } = "Array";
    
    public List<(string name, Delegate body)> GetFunctions()
    {
        return
        [
            ("Create", (Func<List<object?>>)(() => [])),
            ("Add", (Action<List<object?>, object>)((list, item) => list.Add(item))),
            ("AddRange", (Action<List<object?>, List<object?>>)((list, items) => list.AddRange(items))),
            ("GetIndexes", (Func<List<object?>, List<object?>>)((list) => Enumerable.Range(0, list.Count).Cast<object?>().ToList())),
            ("Get", (Func<List<object?>, int, object?>)((list, idx) => list[idx])),
            ("IsCollection", (Func<object?, bool>)(obj => obj is List<object?>)),
            ("Insert", (Action<List<object?>, int, object?>)((list, pos, item) => list.Insert(pos, item))),
            ("Set", (Action<List<object?>, int, object>)((list, idx, val) => list[idx] = val)),
            ("Count", (Func<List<object?>, int>)(list => list.Count)),
            ("Clear", (Action<List<object?>>)(list => list.Clear())),
            ("Contains", (Func<List<object>, object, bool>)((list, item) => list.Contains(item))),
            ("RemoveAt", (Action<List<object?>, int>)((list, idx) => list.RemoveAt(idx))),
            ("IndexOf", (Func<List<object?>, object, int>)((list, item) => list.IndexOf(item))),
        ];
    }
}