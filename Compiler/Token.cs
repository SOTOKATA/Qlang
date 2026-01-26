namespace Compiler;

public class Token(Tokens token, string value = "")
{
    public readonly Tokens TokenType = token;
    public readonly string Value = value;
    
    // Debug variables
    public int DebugIndex = -1;

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
            Tokens.Identifier => "",
            Tokens.LParen => "(",
            Tokens.RParen => ")",
            Tokens.Colon => ": ",
            Tokens.Comma => ", ",
            Tokens.Dot => ".",
            Tokens.Not => "!",
            Tokens.Equals => "=",
            Tokens.Less => "<",
            Tokens.Greater => ">",
            Tokens.Plus => " + ",
            Tokens.Minus => "-",
            Tokens.Star => " * ",
            Tokens.Slash => " / ",
            Tokens.StringRef => "",
            Tokens.NumberRef => "",
            Tokens.Keyword => " ",
            Tokens.Percent => " % ",
            Tokens.Question => " ? ",
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
    Percent,
    StringRef,
    NumberRef,
    Question
}
