using Core.AST;
using Newtonsoft.Json;

namespace Core;

public class QLIProgram
{
    [JsonProperty("a")]
    public required ProgramNode ProgramNode { get; init; }
    
    [JsonProperty("b")]
    public required List<string> StringList { get; init; }

    [JsonProperty("c")]
    public required List<double> NumberList { get; init; }
    
    [JsonProperty("d")]
    public required SourceFileTable SourceFileTable { get; init; }
    
    [JsonProperty("e")]
    public required DebugTable DebugTable { get; init; }
    
    [JsonIgnore]
    public required List<QLIProgramLib> ExternalLibraries { get; set; }
}