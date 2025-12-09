using System.Text;

namespace Qlang.Core.ProjectManager;

public static class TableCreator
{
    public static string Create(List<List<string>>? items, List<string>? separators = null)
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
            List<string> line = items[index];
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

    public static void WriteTable(ConsoleTable table)
    {
        if (table.Items.Count == 0)
        {
            ConsoleLogger.Error("Table is empty");
            return;
        }
        
        int columnCount = table.Items.First().Count;

        List<int> sizes = Enumerable.Repeat(0, columnCount).ToList();

        foreach (var line in table.Items)
            for (int i = 0; i < columnCount; i++)
            {
                string cell = i < line.Count ? line[i].Content : "";
                sizes[i] = Math.Max(sizes[i], cell.Length);
            }

        foreach (var row in table.Items)
        {
            for (int index = 0; index < row.Count; index++)
            {
                TableCell? cell = row[index];
                Console.ForegroundColor = cell.ForeColor;
                Console.BackgroundColor = cell.BackColor;
                Console.Write(cell.Content.PadRight(sizes[index] + 2));
                Console.ResetColor();
                
                if (table.Separators != null && table.Separators.Count < (index - 1))
                    Console.Write(table.Separators[(index - 1)]);
            }

            Console.WriteLine();
        }
    }
}

public class ConsoleTable(List<List<TableCell>>? items, List<string>? separators = null)
{
    public readonly List<List<TableCell>>? Items = items;
    public readonly List<string>? Separators = separators;
}

public class TableCell(string content, ConsoleColor fColor = default, ConsoleColor bColor = default)
{
    public string Content { get; } = content;
    public ConsoleColor ForeColor { get; } = fColor == default ? Console.ForegroundColor : fColor;
    public ConsoleColor BackColor { get; } = bColor == default ? Console.BackgroundColor : bColor;
}