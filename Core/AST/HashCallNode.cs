using Core.Tables;
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class HashCallNode : ASTNode
{
    [Key(0)]
    public required ObjectPointerNode Namespace;
    
    [Key(1)]
    public required ObjectPointerNode Class;
    
    [Key(2)]
    public required FunctionPointerNode Function;
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                           HashCallNode:
                                Namespace: {Namespace.GetTree("\t")}
                                Class: {Class.GetTree("\t")}
                                Function: {Function.GetTree("\t")}
                           """, indent);
    }

    public override ASTNode Clone() => new HashCallNode
    {
        Namespace = (ObjectPointerNode)Namespace.Clone(),
        Class = (ObjectPointerNode)Class.Clone(),
        Function = (FunctionPointerNode)Function.Clone(),
    };

    public override string ToTokenString(StringPoolTable stringPoolTable)
    {
        return $"#{Namespace.ToTokenString(stringPoolTable)}.{Class.ToTokenString(stringPoolTable)}.{Function.ToTokenString(stringPoolTable)}";
    }
}