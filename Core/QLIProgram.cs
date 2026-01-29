using Core.AST;
using MessagePack;
using Newtonsoft.Json;

namespace Core;

[MessagePackObject]
public class QLIProgram
{
    [Key(1)]
    [JsonProperty("a")]
    public required ProgramNode ProgramNode { get; init; }
    
    [Key(2)]
    [JsonProperty("b")]
    public required List<string> StringList { get; init; }

    [Key(3)]
    [JsonProperty("c")]
    public required List<double> NumberList { get; init; }
    
    [Key(4)]
    [JsonProperty("d")]
    public required SourceFileTable SourceFileTable { get; init; }
    
    [Key(5)]
    [JsonProperty("e")]
    public required DebugTable DebugTable { get; init; }
    
    [IgnoreMember]
    [JsonIgnore]
    public List<QLIProgramLib> ExternalLibraries { get; set; }
}