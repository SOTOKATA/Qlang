using Core.Tables;
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class QLIDebug
{
    [Key(0)]
    
    public required SourceFileTable? SourceFileTable { get; init; }
    
    [Key(1)]
    
    public required DebugTable? DebugTable { get; init; }
}