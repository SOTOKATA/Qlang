using System.Text;
using Qlang.AST;

public class QlangRuntimeException : Exception
{
    public int Line { get; }
    public int Column { get; }
    public string SourceFile { get; }
    public string SourceText { get; }
    public List<string> StackTrace { get; }

    public QlangRuntimeException(
        string message, 
        ASTNode node, 
        List<string> stackTrace = null) 
        : base(message)
    {
        Line = node.Line;
        Column = node.Column;
        SourceFile = node.SourceFile;
        // SourceText = node.SourceText;
        StackTrace = stackTrace ?? new List<string>();
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Runtime Error: {Message}");
        sb.AppendLine($"  at {SourceFile}:{Line}:{Column}");
        
        if (!string.IsNullOrEmpty(SourceText))
            sb.AppendLine($"  Source: {SourceText}");

        if (StackTrace.Count <= 0) 
            return sb.ToString();
        
        sb.AppendLine("\nCall Stack:");
        foreach (var frame in StackTrace)
            sb.AppendLine($"  {frame}");

        return sb.ToString();
    }
}