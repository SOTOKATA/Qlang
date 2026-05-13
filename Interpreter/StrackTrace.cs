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

    private void Inspect(object? obj, Stack<ASTContext> stack)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"({DateTime.Now.ToLongTimeString()}) Inspect: ");
        Console.ResetColor();
        Console.Write("object: ");
        Console.ForegroundColor = ConsoleColor.Blue;
        switch (obj)
        {
            case null:
                Console.WriteLine("null");
                Console.ResetColor();
                break;
            case ASTNode astNode:
                Console.Write(obj.GetType().Name);
                Console.ResetColor();
                Console.WriteLine($" = {astNode.ToTokenString(_stringPoolTable)}");
                break;
            default:
                Console.Write(obj.GetType().Name);
                Console.ResetColor();
                Console.WriteLine($" = {obj}");
                break;
        }
        
        var currentContext = CurrentContext(stack);

        var namespaceExists = currentContext?.Namespace != null;
        var classExists = currentContext?.Class != null;
        var functionExists = currentContext?.Function != null;

        Console.Write("\t");
        
        if (namespaceExists && currentContext.Namespace.Name != "~global")
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(currentContext.Namespace.Name + (classExists || functionExists ? "::" : ""));
            Console.ResetColor();
        }
        if (classExists)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(currentContext.Class.Name + (functionExists ? "." : ""));
            Console.ResetColor();
        }
        if (functionExists)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(currentContext.Function.Name + $"({string.Join(", ", currentContext.Function.Parameters)})");
            Console.ResetColor();
        }
        
        Console.WriteLine();
    }
}