using MessagePack;

namespace Core.Tables;

[MessagePackObject]
public class StringPoolTable
{
    [Key(0)]
    public List<string> StringPool { get; set; } = [];

    public int Add(string str)
    {
        var index = StringPool.IndexOf(str);
        if (index != -1)
            return index;

        StringPool.Add(str);
        return StringPool.Count - 1;
    }
    
    public string this[int index] => StringPool[index];
}