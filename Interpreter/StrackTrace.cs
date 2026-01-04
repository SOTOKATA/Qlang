namespace Interpreter;

public partial class Interpreter
{
    private List<string>? GetStackTrace(int skip = 0)
    {
        return (from context in _contextStack.Reverse()
            let location = context.CurrentNode != null
                ? $"{context.CurrentNode.SourceFile}:{context.CurrentNode.Line}"
                : "unknown"
            let funcName = context.Function?.Name ?? "global"
            let className = context.Class?.Name
            select className != null
                ? $"at {className}.{funcName} ({location})"
                : $"at {funcName} ({location})").SkipLast(skip).ToList();
    }
}