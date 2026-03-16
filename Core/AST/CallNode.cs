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
    {
        var str = "";

        for (var index = 0; index < Objects.Count; index++)
        {
            var obj = Objects[index];
            str += obj.ToTokenString(stringPoolTable);

            if (index == Objects.Count - 1)
                continue;

            if (obj is NamespacePointerNode)
                str += "::";
            else str += ".";
        }

        return str;
    }

    public override string GetTree(string indent = "")
    {
        return DebugIndent($"""
                            CallNode:
                                Objects: [{string.Join(" ,\n", Objects.Select(x => x.GetTree("\t")))}]
                            """, indent);
    }
}