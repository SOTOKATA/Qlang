using Core.AST;
using Core.Native;

namespace Core;

public class QLIProgram
{
    public ProgramNode ProgramNode { get; init; }
    
    public Dictionary<string, string> StringDictionary { get; init; }

    public Dictionary<string, object> NumberDictionary { get; init; }
    
    public List<QLIProgramLib> ExternalLibraries { get; set; }
}