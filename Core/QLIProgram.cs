using Core.AST;
using Newtonsoft.Json;

namespace Core;

public class QLIProgram
{
    [JsonProperty("a")]
    public ProgramNode ProgramNode { get; init; }
    
    [JsonProperty("b")]
    public List<string> StringList { get; init; }

    [JsonProperty("c")]
    public List<double> NumberList { get; init; }
    
    [JsonProperty("d")]
    public SourceFileTable SourceFileTable { get; init; }
    
    [JsonProperty("e")]
    public List<QLIProgramLib> ExternalLibraries { get; set; }
}