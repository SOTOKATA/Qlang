using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class CastNode(CallNode call, CallNode obj, int line) : ASTNode(line)
{
    public CastNode() : this(null, null, -1) {}
    
    [Key(1)]
    
    public CallNode TypeCastPath { get; set; } = call;

    [Key(2)]
    
    public CallNode ToCastObject { get; set; } = obj;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            CastNode:
                                TypeCastPath: {TypeCastPath.GetTree()}
                                ToCastObject: {ToCastObject.GetTree()}
                            """, indent);
    }

    public override ASTNode Clone()
    {
        return new CastNode((CallNode)TypeCastPath.Clone(), (CallNode)ToCastObject.Clone(), DebugIndex);
    }
}