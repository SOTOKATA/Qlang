namespace Core.AST;

public class CastNode(CallNode call, CallNode obj) : ASTNode
{
    public CallNode TypeCastPath { get; set; } = call;

    public CallNode ToCastObject { get; set; } = obj;
    
    public override string GetTree(string indent = "")
    {
        return "!ignored";
    }

    public override ASTNode Clone()
    {
        return new CastNode((CallNode)TypeCastPath.Clone(), (CallNode)ToCastObject.Clone());
    }
}