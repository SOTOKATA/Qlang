using Newtonsoft.Json;

namespace Core;

[JsonObject(MemberSerialization.OptOut)]
public class DebugTable
{
    [JsonProperty("l")]
    private List<int> LineIndexes { get; set; } = [];
    
    [JsonProperty("f")]
    private List<NumberCount> FileIds { get; set; }  = [];
    
    public int Add(int lineIndex, int fileId)
    {
        // Добавляем номер строки в обычный список
        // if (!LineIndexes.Contains(lineIndex))
            LineIndexes.Add(lineIndex);
        
        // Проверяем, есть ли уже такой fileId в последнем элементе
        if (FileIds.Count > 0 && FileIds[^1].Number == fileId)
        {
            // Увеличиваем count последнего элемента
            var last = FileIds[^1];
            FileIds[^1] = new NumberCount(last.Number, last.Count + 1);
        }
        else
        {
            // Добавляем новый fileId с count = 1
            FileIds.Add(new NumberCount(fileId, 1));
        }
        
        return LineIndexes.Count - 1;
    }

    public int GetLineIndex(int index)
    {
        return LineIndexes[index];
    }
    
    public int GetFileId(int index)
    {
        // Получаем виртуальный индекс
        var virtualIndex = GetListIndex(index).listIndex;
        
        return FileIds[virtualIndex].Number;
    }

    private (int listIndex, int localIndex) GetListIndex(int expandedIndex)
    {
        var currentIndex = 0;
    
        for (var i = 0; i < FileIds.Count; i++)
        {
            if (currentIndex + FileIds[i].Count > expandedIndex)
                return (i, expandedIndex - currentIndex);
            currentIndex += FileIds[i].Count;
        }
    
        Console.WriteLine("ExpandedIndex: " + expandedIndex);
        Console.WriteLine(string.Join(", ", FileIds));
        throw new ArgumentOutOfRangeException(nameof(expandedIndex));
    }
}

public class NumberCount(int number, int count)
{
    [JsonProperty("n")]
    public int Number { get; set; } = number;

    [JsonProperty("c")]
    public int Count { get; set; } =  count;
}

// public class DebugTable
// {
//     [JsonProperty("l")]
//     public List<int> LineIndexes { get; set; } = [];
//     
//     [JsonProperty("f")]
//     public List<(int n, int c)> FileIds { get; set; }  = [];
//     
//     public int Add(int lineIndex, int fileId)
//     {
//         for (var i = 0; i < LineIndexes.Count; i++)
//             if (LineIndexes[i] == lineIndex && FileIds[i] == fileId)
//                 return i;
//     
//         LineIndexes.Add(lineIndex);
//         FileIds.Add(fileId);
//     
//         return LineIndexes.Count - 1;
//     }
//
//
//     public int GetLineIndex(int index)
//     {
//         return LineIndexes[index];
//     }
//     
//     public int GetFileId(int index)
//     {
//         // Get virtual index
//         var virtualIndex = GetListIndex(index).listIndex;
//         
//         return FileIds[virtualIndex].n;
//     }
//
//     private (int listIndex, int localIndex) GetListIndex(int expandedIndex)
//     {
//         var currentIndex = 0;
//     
//         for (var i = 0; i < FileIds.Count; i++)
//         {
//             if (currentIndex + FileIds[i].c > expandedIndex)
//                 return (i, expandedIndex - currentIndex);
//             currentIndex += FileIds[i].c;
//         }
//     
//         throw new ArgumentOutOfRangeException(nameof(expandedIndex));
//     }
//
// }