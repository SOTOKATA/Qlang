using MessagePack;
using Newtonsoft.Json;

namespace Core;

[JsonObject(MemberSerialization.OptOut)]
[MessagePackObject]
public class DebugTable
{
    [Key(1)]
    [JsonProperty("l")]
    public List<int> LineIndexes { get; set; } = [];
    
    [Key(2)]
    [JsonProperty("f")]
    public List<NumberCount> FileIds { get; set; }  = [];

    [JsonIgnore] 
    [IgnoreMember]
    private int _index = 0;
    
    public int Add(int lineIndex, int fileId)
    {
        var lineExists = LineIndexes.Count > 0 && LineIndexes[^1] == lineIndex; //.Number
        var fileExists = FileIds.Count > 0 && FileIds[^1].Number == fileId;
        switch (lineExists)
        {
            case true when fileExists:
                // LineIndexes[^1].Count++;
                // FileIds[^1].Count++;
                // _index++;
                break;
            case true when !fileExists:
                // LineIndexes[^1].Count++;
                FileIds.Add(new NumberCount(fileId, 1));
                _index++;
                break;
            case false when fileExists:
                LineIndexes.Add(lineIndex);
                // LineIndexes.Add(new NumberCount(lineIndex, 1));
                FileIds[^1].Count++;
                _index++;
                break;
            default:
                LineIndexes.Add(lineIndex);
                // LineIndexes.Add(new NumberCount(lineIndex, 1));
                FileIds.Add(new NumberCount(fileId, 1));
                _index++;
                break;
        }

        return _index;
    }

    public int GetLineIndex(int index)
    {
        // var virtualIndex = GetListIndex(LineIndexes, index).listIndex;
        
        // return LineIndexes[virtualIndex].Number;
        return LineIndexes[index];
    }
    
    public int GetFileId(int index)
    {
        // Получаем виртуальный индекс
        var virtualIndex = GetListIndex(FileIds, index).listIndex;
        
        return FileIds[virtualIndex].Number;
    }

    private (int listIndex, int localIndex) GetListIndex(List<NumberCount> list, int expandedIndex)
    {
        var currentIndex = 0;
    
        for (var i = 0; i < list.Count; i++)
        {
            if (currentIndex + list[i].Count > expandedIndex)
                return (i, expandedIndex - currentIndex);
            currentIndex += list[i].Count;
        }
    
        Console.WriteLine("ExpandedIndex: " + expandedIndex);
        Console.WriteLine(string.Join(", ", list));
        throw new ArgumentOutOfRangeException(nameof(expandedIndex));
    }
}

[MessagePackObject]
public class NumberCount(int number, int count)
{
    [Key(1)]
    [JsonProperty("n")]
    public int Number { get; set; } = number;

    [Key(2)]
    [JsonProperty("c")]
    public int Count { get; set; } =  count;
}