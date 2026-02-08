using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class KeywordNode : ASTNode
{
    [Key(0)]
    
    public int KeywordId { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return KeywordId.ToString();
    }

    public override ASTNode Clone()
    {
        return new KeywordNode
        {
            KeywordId = KeywordId,
        };
    }
}