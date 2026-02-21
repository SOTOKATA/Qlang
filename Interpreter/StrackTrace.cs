namespace Interpreter;

public partial class Interpreter
{
    private List<string> GetStackTrace(int skip = 0)
    {
        return (from context in _contextStack
            let location = context.CurrentNode != null
                ? $"{GetDebug(context.CurrentDebugIndex).Item2}:{GetDebug(context.CurrentDebugIndex).Item1}"
                : "unknown"
            let funcName = context.Function?.Name ?? "global"
            let className = context.Class?.Name
            select className != null
                ? $"at {className}.{funcName} ({location})"
                : $"at {funcName} ({location})").Skip(skip).ToList();
    }
}