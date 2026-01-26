using Core;
using Core.Exceptions;

namespace Compiler;

/*
 * First stage of project compilation
 * Translation of code into special characters
 * Example:
 *      Code: print(1);
 *      Translated code: Identificator(print) LParen NumberRef(___NUMBER_0___) RParen SemiColon
 * This process also saves debug data (line number and file index) and creates a file table.
 */
public static class Lexer
{
    public static (List<Token> tokens, SourceFileTable sourceFileTable, DebugTable debugTable) Lex(string fileName, string script)
    {
        List<Token> tokens = [];
        SourceFileTable sourceFileTable = new();
        DebugTable debugTable = new();
        
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
                    charToken!.DebugIndex = debugTable.Add(lineIndex, sourceFileTable.GetOrAdd(fileName));
                    
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
                        wordToken!.DebugIndex = debugTable.Add(lineIndex, sourceFileTable.GetOrAdd(fileName));
                        
                        tokens.Add(wordToken);
                        continue;
                    }
                }

                throw new QlangCompileException($"Failed to detect: '{line[pos]}' at position {lineIndex}:{pos}", lineIndex, "Lexer", fileName);
            }
            lineIndex++;
        }

        return (tokens, sourceFileTable, debugTable);
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
        
        token = new Token(Tokens.Identifier, word);
        return true;
    }
 
    private static bool IsKeyword(string word, out Token? token)
    {
        // Список ключевых слов твоего языка
        var keywords = Keywords.GetKeywords().ToArray();
        
        if (keywords.Contains(word.ToLower()))
        {
            token = new Token(Tokens.Keyword, word);
            return true;
        }
        
        token = null;
        return false;
    }

    private static bool TryCharToToken(char keychar, out Token? token)
    {
        token = keychar switch
        {
            '[' => new Token(Tokens.LSquareParen),
            ']' => new Token(Tokens.RSquareParen),
            '{' => new Token(Tokens.LBrace),
            '}' => new Token(Tokens.RBrace),
            ';' => new Token(Tokens.Semicolon),
            '=' => new Token(Tokens.Equals),
            '+' => new Token(Tokens.Plus),
            '-' => new Token(Tokens.Minus),
            '*' => new Token(Tokens.Star),
            '/' => new Token(Tokens.Slash),
            '%' => new Token(Tokens.Percent),
            '(' => new Token(Tokens.LParen),
            ')' => new Token(Tokens.RParen),
            ':' => new Token(Tokens.Colon),
            ',' => new Token(Tokens.Comma),
            '.' => new Token(Tokens.Dot),
            '!' => new Token(Tokens.Not),
            '>' => new Token(Tokens.Greater),
            '<' => new Token(Tokens.Less),
            '|' => new Token(Tokens.Or),
            '&' => new Token(Tokens.And),
            '?' => new  Token(Tokens.Question),
            var _ => null
        };
        
        return token != null;
    }
}