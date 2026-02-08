using MessagePack;


namespace Core.AST;
[MessagePackObject]
public class KeywordNode(int line) : ASTNode(line)
{
    [Key(1)]
    
    public int KeywordId { get; set; }
    
    public override string GetTree(string indent = "")
    {
        return $"    DebugIndex: {DebugIndex}";
    }

    public override ASTNode Clone()
    {
        return new KeywordNode(DebugIndex)
        {
            KeywordId = KeywordId,
        };
    }
}