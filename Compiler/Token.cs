namespace Qlang.Compiler;

public class Token(Tokens token, string value = "")
{
    public readonly Tokens TokenType = token;
    public readonly string Value = value;
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
    Variable,
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
