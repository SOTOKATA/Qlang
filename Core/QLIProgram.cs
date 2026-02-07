using Core.AST;
using Core.Tables;
using MessagePack;


namespace Core;

[MessagePackObject]
public class QLIProgram
{
    [Key(1)]
    
    public required ProgramNode ProgramNode { get; init; }
    
    [Key(2)]
    
    public required List<string> StringList { get; init; }

    [Key(3)]
    
    public required List<double> NumberList { get; init; }
    
    [Key(4)]
    public required StringPoolTable StringPoolTable { get; init; }
    
    [IgnoreMember]
    public List<QLIProgramLib>? ExternalLibraries { get; set; }
}