namespace Qlang.Compiler;

public class Token(Tokens token, int line, int index, string value = "")
{
    public readonly Tokens TokenType = token;
    public readonly string Value = value;
    
    // Debug variables
    public int Line = line;
    public int Index = index;
}

public enum Tokens
{
    Keyword,
    Identifier,
    LParen,
    RParen,
    Colon,
    Comma,
    Dot,
    Not,
    Equals,
    Less,
    Greater,
    Plus,
    Minus,
    Star,
    Slash,
    StringRef,
    Indent,
    Dedent,
    NewLine,
}
