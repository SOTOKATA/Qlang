namespace Interpreter;

public partial class Interpreter
{
    private List<string> GetStackTrace(int skip = 0)
    {
        return (from context in _contextStack.Reverse()
            let location = context.CurrentNode != null
                ? $"{GetDebug(context.CurrentNode).Item2}:{GetDebug(context.CurrentNode).Item1}"
                : "unknown"
            let funcName = context.Function?.Name ?? "global"
            let className = context.Class?.Name
            select className != null
                ? $"at {className}.{funcName} ({location})"
                : $"at {funcName} ({location})").SkipLast(skip).ToList();
    }
}