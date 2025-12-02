using System.Text;
using Qlang.AST;

public class QlangCompileException : Exception
{
    private int Line { get; }
    
    private string Source { get; }
    
    public QlangCompileException(
        string message, int line, string source) 
        : base(message)
    {
        Line = line;
        Source = source;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Compile ({Source}) Error: {Message}");
        sb.AppendLine($"\tat: {Line}");

        return sb.ToString();
    }
}