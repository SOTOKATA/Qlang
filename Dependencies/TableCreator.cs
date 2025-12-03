using System.Text;

namespace Qlang.Dependencies;

public class TableCreator
{
    public static string Create(List<List<string>>? items, List<string> separators = null)
    {
        if (items == null || items.Count == 0)
            return "Table is empty";

        StringBuilder str = new();

        int columnCount = items.First().Count;

        List<int> sizes = Enumerable.Repeat(0, columnCount).ToList();

        foreach (var line in items)
            for (int i = 0; i < columnCount; i++)
            {
                string cell = i < line.Count ? line[i] : "";
                sizes[i] = Math.Max(sizes[i], cell.Length);
            }

        for (int index = 0; index < items.Count; index++)
        {
            List<string>? line = items[index];
            for (int i = 0; i < columnCount; i++)
            {
                string cell = i < line.Count ? line[i] : "";
                str.Append(cell.PadRight(sizes[i] + 2));

                if (separators != null && separators.Count > i && index != 0)
                    str.Append(separators[i] + " ");
            }

            str.AppendLine();
        }

        return str.ToString();
    }

}