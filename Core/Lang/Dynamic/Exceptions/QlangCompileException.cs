using System.Text;

namespace Qlang.Core.Lang.Dynamic.Exceptions;

public class QlangCompileException : Exception
{
    private int Line { get; }
    
    private string Source { get; }
    
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
        sb.AppendLine($"Compile ({Source}) Error: {Message}");
        sb.AppendLine($"\tat: {SourceFile}:{Line}");

        return sb.ToString();
    }
}