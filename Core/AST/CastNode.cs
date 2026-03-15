using Core.Tables;
using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class CastNode : ASTNode
{
    [Key(0)]
    
    public required CallNode TypeCastPath { get; set; }

    [Key(1)]
    
    public required CallNode ToCastObject { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            CastNode:
                                TypeCastPath: {TypeCastPath.GetTree()}
                                ToCastObject: {ToCastObject.GetTree()}
                            """, indent);
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
        => $"<{TypeCastPath.ToTokenString(stringPoolTable)}>{ToCastObject.ToTokenString(stringPoolTable)}";

    public override ASTNode Clone()
    {
        return new CastNode
        {
            TypeCastPath = (CallNode)TypeCastPath.Clone(),
            ToCastObject = (CallNode)ToCastObject.Clone(),
        };
    }
}