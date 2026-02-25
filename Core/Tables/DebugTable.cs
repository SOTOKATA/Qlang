using MessagePack;

namespace Core.Tables;

[MessagePackObject]
public class DebugTable
{
    [Key(1)]
    
    public List<int> LineIndexes { get; set; } = [];
    
    [Key(2)]
    
    public List<NumberCount> FileIds { get; set; }  = [];
     
    private int _index = -1;
    
    public int Add(int lineIndex, int fileId)
    {
        var lineExists = LineIndexes.Count > 0 && LineIndexes[^1] == lineIndex;
        var fileExists = FileIds.Count > 0 && FileIds[^1].Number == fileId;
        switch (lineExists)
        {
            case true when fileExists:
                break;
            case true when !fileExists:
                FileIds.Add(new NumberCount(fileId, 1));
                // LineIndexes.Add(lineIndex);
                _index++;
                break;
            case false when fileExists:
                LineIndexes.Add(lineIndex);
                FileIds[^1].Count++;
                _index++;
                break;
            default:
                LineIndexes.Add(lineIndex);
                FileIds.Add(new NumberCount(fileId, 1));
                _index++;
                break;
        }

        return _index;
    }

    public int GetLineIndex(int index)
    {
        return LineIndexes[index];
    }
    
    public int GetFileId(int index)
    {
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
    
        throw new ArgumentOutOfRangeException(nameof(expandedIndex));
    }
}

[MessagePackObject]
public class NumberCount(int number, int count)
{
    [Key(1)]
    public int Number { get; set; } = number;

    [Key(2)]
    public int Count { get; set; } =  count;
}