using System.Text;
using Core.AST;

namespace Core.Exceptions;

public class QlangRuntimeException : Exception
{
    private int Line { get; }
    private int Column { get; }
    private string SourceFile { get; }
    private new List<string> StackTrace { get; }

    public QlangRuntimeException(
        string message, 
        int line, string source, 
        List<string>? stackTrace = null) 
        : base(message)
    {
        Line = line;
        SourceFile = source;

        StackTrace = stackTrace ?? [];
    }
    
    public QlangRuntimeException(
        string message, List<string>? stackTrace = null) 
        : base(message)
    {
        Line = -1;
        SourceFile = "";

        StackTrace = stackTrace ?? [];
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"{Message}");
        sb.AppendLine($"  at {(SourceFile == "" ? "(undefined file)" : SourceFile)}:{Line}:{Column}");

        if (StackTrace.Count <= 0) 
            return sb.ToString();
        
        sb.AppendLine("\nCall Stack:");
        foreach (var frame in StackTrace)
            sb.AppendLine($"  {frame}");

        return sb.ToString();
    }
}