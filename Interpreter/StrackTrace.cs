using Core.AST;

namespace Interpreter;

public partial class Interpreter
{
    private List<string> GetStackTrace(Stack<ASTContext> stack, int skip = 0)
    {
        return (from context in stack
            let location = context.CurrentNode != null
                ? $"{GetDebug(context.CurrentDebugIndex).Item2}:{GetDebug(context.CurrentDebugIndex).Item1}"
                : "unknown"
            let funcName = context.Function?.Name ?? "global"
            let className = context.Class?.Name
            let namespaceName = context.Namespace?.Name
            select (namespaceName is null or "~global")
                ? $"at {(className is null ? "" : className + ".")}{funcName} ({location})"
                : $"at {namespaceName}::{(className is null ? "" : className + ".")}{funcName} ({location})").Skip(skip).ToList();
    }
}