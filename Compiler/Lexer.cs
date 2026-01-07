using Core;
using Core.Exceptions;

namespace Compiler;

public class Lexer
{
    public static List<Token> Lex(string fileName, string script)
    {
        List<Token> tokens = [];
        var scriptLines = script.Split('\n').ToList();

        var lineIndex = 0;
        foreach (var line in scriptLines.Select(rawLine => rawLine.Trim()))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                lineIndex++;
                continue;
            }
            
            if (line.StartsWith("#FILE "))
            {
                lineIndex = 0;
                fileName = line["#FILE ".Length..];
                continue;
            }

            var pos = 0;
            while (pos < line.Length)
            {
                if (char.IsWhiteSpace(line[pos]))
                {
                    pos++;
                    continue;
                }

                if (TryCharToToken(line[pos], out var charToken))
                {
                    charToken.Line = lineIndex;
                    charToken.SourceFile = fileName;
                    
                    tokens.Add(charToken);
                    pos++;
                    continue;
                }

                var startPos = pos;

                if (IsIdentifierStart(line[pos]))
                {
                    while (pos < line.Length && IsIdentifierChar(line[pos]))
                        pos++;

                    var word = line[startPos..pos];
                    if (TryWordToToken(word, out var wordToken))
                    {
                        wordToken.Line = lineIndex;
                        wordToken.SourceFile = fileName;
                        
                        tokens.Add(wordToken);
                        continue;
                    }
                }

                throw new QlangCompileException($"Failed to detect: '{line[pos]}' at position {pos}", pos, "Lexer", fileName);
            }
            lineIndex++;
        }

        return tokens;
    }

    private static bool IsIdentifierStart(char c)
    {
        return char.IsLetter(c) || char.IsNumber(c) || c == '_';
    }

    private static bool IsIdentifierChar(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_';
    }

    private static bool TryWordToToken(string word, out Token? token)
    {
        if (IsKeyword(word, out token))
            return true;
        
        token = new Token(Tokens.Identifier, -1, word);
        return true;
    }
 
    private static bool IsKeyword(string word, out Token? token)
    {
        // Список ключевых слов твоего языка
        var keywords = Keywords.GetKeywords().ToArray();
        
        if (keywords.Contains(word.ToLower()))
        {
            token = new Token(Tokens.Keyword, -1, word);
            return true;
        }
        
        token = null;
        return false;
    }

    private static bool TryCharToToken(char keychar, out Token? token)
    {
        token = keychar switch
        {
            '[' => new Token(Tokens.LSquareParen, -1),
            ']' => new Token(Tokens.RSquareParen, -1),
            '{' => new Token(Tokens.LBrace, -1),
            '}' => new Token(Tokens.RBrace, -1),
            ';' => new Token(Tokens.Semicolon, -1),
            '=' => new Token(Tokens.Equals, -1),
            '+' => new Token(Tokens.Plus, -1),
            '-' => new Token(Tokens.Minus, -1),
            '*' => new Token(Tokens.Star, -1),
            '/' => new Token(Tokens.Slash, -1),
            '%' => new Token(Tokens.Percent, -1),
            '(' => new Token(Tokens.LParen, -1),
            ')' => new Token(Tokens.RParen, -1),
            ':' => new Token(Tokens.Colon, -1),
            ',' => new Token(Tokens.Comma, -1),
            '.' => new Token(Tokens.Dot, -1),
            '!' => new Token(Tokens.Not, -1),
            '>' => new Token(Tokens.Greater, -1),
            '<' => new Token(Tokens.Less, -1),
            '|' => new Token(Tokens.Or, -1),
            '&' => new Token(Tokens.And, -1),
            '?' => new  Token(Tokens.Question, -1),
            var _ => null
        };
        
        return token != null;
    }
}