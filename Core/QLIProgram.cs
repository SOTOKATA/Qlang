using Core.AST;
using Core.Native;

namespace Core;

public class QLIProgram
{
    public ProgramNode ProgramNode { get; set; }
    
    public Dictionary<string, string> StringDictionary { get; set; }

    public Dictionary<string, object> NumberDictionary { get; set; }
    
    public NativeFunctionRegistry NativeFunctions { get; set; }
}