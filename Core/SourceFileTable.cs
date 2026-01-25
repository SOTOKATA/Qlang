using Newtonsoft.Json;

namespace Core;

public class SourceFileTable
{
    [JsonProperty("a")]
    public List<string> Index { get; set; } = [];
    
    [JsonIgnore]
    private Dictionary<string, int> _cache = new();

    public int GetOrAdd(string path)
    {
        if (_cache.TryGetValue(path, out var index))
            return index;

        index = Index.Count;
        
        Index.Add(path);
        
        _cache[path] = index;
        
        return index;
    }

    public string this[int index] => Index[index];
    
    public void RebuildCache()
    {
        _cache = new Dictionary<string, int>();
        for (var i = 0; i < Index.Count; i++)
            _cache[Index[i]] = i;
    }
}
