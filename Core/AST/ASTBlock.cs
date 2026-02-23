using Core.Dynamic;
using MessagePack;


namespace Core.AST;

[MessagePackObject]
public class ASTBlock : ASTNode
{
    [Key(-1)]
    
    public Dictionary<string, Variable> Variables { get; set; } = [];

    public override string GetTree(string indent = "")
    {
        throw new NotImplementedException();
    }

    public override ASTNode Clone()
    {
        throw new NotImplementedException("Cannot clone this ASTBlock");
    }
}