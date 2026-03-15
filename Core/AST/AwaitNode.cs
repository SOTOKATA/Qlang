using Core.Tables;

namespace Core.AST;

public class AwaitNode : ASTNode
{
    public CallNode CallNode { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                           AwaitNode:
                                CallNode: {CallNode.GetTree("\t")}
                           """, indent);
    }

    public override ASTNode Clone()
    {
        return new AwaitNode()
        {
            CallNode = (CallNode)CallNode.Clone()
        };
    }

    public override string ToTokenString(StringPoolTable stringPoolTable) => "await " + CallNode.ToTokenString(stringPoolTable);
}