using Core.Tables;
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class LineNode(int debugIndex) : ASTNode
{
    [Key(0)] 
    public int DebugIndex { get; set; } = debugIndex;
    [Key(1)]
    public ASTNode? Content { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                LineNode:
                    DebugIndex: {DebugIndex}
                    Content: {Content?.GetTree("\t") ?? "<undefined>"}
                """, indent);
    }
    
    public override string ToTokenString(StringPoolTable stringPoolTable)
        => Content.ToTokenString(stringPoolTable);

    public override ASTNode Clone()
    {
        return new LineNode(DebugIndex)
        {
            Content = Content?.Clone()
        };
    }
}