using Core.AST;
using Core.Native;

namespace Core;

public class QLIProgram
{
    public ProgramNode ProgramNode { get; init; }
    
    public Dictionary<string, string> StringDictionary { get; init; }

    public Dictionary<string, object> NumberDictionary { get; init; }
    
    public NativeFunctionRegistry NativeFunctions { get; init; }
    
    public List<string> Dependencies { get; set; }
}