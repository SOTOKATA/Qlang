namespace Qlang.Compiler;

public class Token(Tokens token, int line, int index, string value = "")
{
    public readonly Tokens TokenType = token;
    public readonly string Value = value;
    
    // Debug variables
    public int Line = line;
    public int Index = index;

    public static string TokenToString(Tokens token)
    {
        return token switch
        {
            Tokens.LSquareParen => "[",
            Tokens.RSquareParen => "]",
            Tokens.LBrace => "{",
            Tokens.RBrace => "}",
            Tokens.Semicolon => ";",
            Tokens.Or => "|",
            Tokens.And => "&",
            Tokens.Identifier => " ",
            Tokens.LParen => "(",
            Tokens.RParen => ")",
            Tokens.Colon => ": ",
            Tokens.Comma => ", ",
            Tokens.Dot => ".",
            Tokens.Not => "!",
            Tokens.Equals => "=",
            Tokens.Less => "<",
            Tokens.Greater => ">",
            Tokens.Plus => "+",
            Tokens.Minus => "-",
            Tokens.Star => "*",
            Tokens.Slash => "/",
            Tokens.StringRef => " ",
            Tokens.NumberRef => " ",
            Tokens.Keyword => " ",
        };
    }
}

public enum Tokens
{
    LSquareParen,
    RSquareParen,
    LBrace,
    RBrace,
    Semicolon,
    Or,
    And,
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
    NumberRef,
}
