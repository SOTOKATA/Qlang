using Core.Tables;
using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class CallNode : ASTNode
{
    [Key(0)]
    
    public List<ASTNode> Objects = [];

    [IgnoreMember] 
    public int FileId = -1;

    public override ASTNode Clone()
    {
        return new CallNode
        {
            Objects = Objects.Select(node => node.Clone()).ToList(),
        };
    }
    
    public override string ToTokenString(StringPoolTable stringPoolTable)
        => string.Join(".", Objects.Select(x => x.ToTokenString(stringPoolTable)));

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            CallNode:
                                Objects: [{string.Join(" ,\n", Objects.Select(x => x.GetTree("\t")))}]
                            """, indent);
    }
}