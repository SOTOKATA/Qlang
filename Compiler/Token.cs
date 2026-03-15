namespace Compiler;

public class Token(Tokens token, string value = "")
{
    public readonly Tokens TokenType = token;
    public readonly string Value = value;
    
    // Debug variable
    public int DebugIndex;
    
    public static string TokenToString(Token token)
    {
        return token.TokenType switch
        {
            Tokens.LSquareParen => "[",
            Tokens.RSquareParen => "]",
            Tokens.LBrace => "{",
            Tokens.RBrace => "}",
            Tokens.Semicolon => ";",
            Tokens.Or => "|",
            Tokens.And => "&",
            Tokens.LParen => "(",
            Tokens.RParen => ")",
            Tokens.Colon => ":",
            Tokens.Comma => ",",
            Tokens.Dot => ".",
            Tokens.Not => "!",
            Tokens.Equals => "=",
            Tokens.Less => "<",
            Tokens.Greater => ">",
            Tokens.Plus => "+",
            Tokens.Minus => "-",
            Tokens.Star => "*",
            Tokens.Slash => "/",
            Tokens.StringRef => "StringReference",
            Tokens.NumberRef => "NumberReference",
            Tokens.Percent => "%",
            Tokens.Question => "?",
            _ when !string.IsNullOrWhiteSpace(token.Value) => token.Value,
            _ => token.ToString()!
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
