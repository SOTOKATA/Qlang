using System.Text;
using Qlang.AST;

public class QlangRuntimeException : Exception
{
    private int Line { get; }
    private int Column { get; }
    private string SourceFile { get; }
    private new List<string> StackTrace { get; }

    public QlangRuntimeException(
        string message, 
        ASTNode? node, 
        List<string>? stackTrace = null) 
        : base(message)
    {
        if (node is not null)
        {
            Line = node.Line;
            Column = node.LineIndex;
            SourceFile = node.SourceFile;
        }

        StackTrace = stackTrace ?? [];
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Runtime Error: {Message}");
        sb.AppendLine($"  at {SourceFile}:{Line}:{Column}");

        if (StackTrace.Count <= 0) 
            return sb.ToString();
        
        sb.AppendLine("\nCall Stack:");
        foreach (var frame in StackTrace)
            sb.AppendLine($"  {frame}");

        return sb.ToString();
    }
}