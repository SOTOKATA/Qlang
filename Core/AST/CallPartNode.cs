using Core.Tables;
using MessagePack;

namespace Core.AST;

[MessagePackObject]
public class CallPartNode : ASTNode
{
    [Key(0)]
    public bool IsNullable { get; set; }
    
    [Key(1)]
    public int NameId { get; set; }

    public override string GetTree(string indent = "")
    {
        throw new NotImplementedException();
    }

    public override ASTNode Clone()
    {
        throw new NotImplementedException();
    }

    public override string ToTokenString(StringPoolTable stringPoolTable)
    {
        throw new NotImplementedException();
    }
}