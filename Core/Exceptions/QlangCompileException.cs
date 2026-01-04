using System.Text;

namespace Core.Exceptions;

public class QlangCompileException : Exception
{
    private int Line { get; }

    public override string? Source { get; set; }

    private string SourceFile { get; }
    
    public QlangCompileException(
        string message, int line, string source, string sourceFile) 
        : base(message)
    {
        Line = line;
        Source = source;
        SourceFile = sourceFile;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"{Source} Error: {Message}");
        sb.AppendLine($"\tat: {SourceFile}:{Line}");

        return sb.ToString();
    }
}