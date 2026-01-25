using Newtonsoft.Json;

namespace Core.AST;

public class CastNode(CallNode call, CallNode obj, int line, int sfId) : ASTNode(line, sfId)
{
    [JsonProperty("a")]
    public CallNode TypeCastPath { get; set; } = call;

    [JsonProperty("b")]
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
        return new CastNode((CallNode)TypeCastPath.Clone(), (CallNode)ToCastObject.Clone(), Line, SourceFileId);
    }
}