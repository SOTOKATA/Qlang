namespace Qlang.Compiler;

public class Lexer(Compiler compiler)
{
    private Compiler _compiler = compiler;

    private readonly Stack<int> _indentStack = new([0]);

    public List<Token> Lex(string script)
    {
        List<Token> tokens = [];
        List<string> scriptLines = script.Split('\n').ToList();

        foreach (var rawLine in scriptLines)
        {
            // Обрабатываем отступы ДО trim
            var indent = CountLeadingSpaces(rawLine);
            var line = rawLine.TrimStart();
            
            // Пропускаем пустые строки
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Добавляем INDENT/DEDENT токены
            HandleIndentation(indent, tokens);

            // Токенизация строки
            var pos = 0;
            while (pos < line.Length)
            {
                // Пропускаем пробелы внутри строки
                if (char.IsWhiteSpace(line[pos]))
                {
                    pos++;
                    continue;
                }

                // Проверяем односимвольные токены
                if (TryCharToToken(line[pos], out var charToken))
                {
                    tokens.Add(charToken!);
                    pos++;
                    continue;
                }

                // Читаем слово/переменную/идентификатор
                var startPos = pos;
                
                // Переменная начинается с $
                if (line[pos] == '$')
                {
                    pos++; // Пропускаем $
                    while (pos < line.Length && IsIdentifierChar(line[pos]))
                        pos++;
                    
                    var varName = line[startPos..pos];
                    if (IsVariable(varName, out var varToken))
                    {
                        tokens.Add(varToken!);
                        continue;
                    }
                }
                
                // Обычное слово (keyword или identifier)
                if (IsIdentifierStart(line[pos]))
                {
                    while (pos < line.Length && IsIdentifierChar(line[pos]))
                        pos++;
                    
                    var word = line[startPos..pos];
                    if (TryWordToToken(word, out var wordToken))
                    {
                        tokens.Add(wordToken!);
                        continue;
                    }
                }

                // Если ничего не подошло
                Console.WriteLine($"Failed to detect: '{line[pos]}' at position {pos}");
                pos++;
            }

            tokens.Add(new Token(Tokens.NewLine));
        }

        // Закрываем все открытые блоки в конце файла
        while (_indentStack.Count > 1)
        {
            tokens.Add(new Token(Tokens.Dedent));
            _indentStack.Pop();
        }

        return tokens;
    }

    private void HandleIndentation(int currentIndent, List<Token> tokens)
    {
        var previousIndent = _indentStack.Peek();

        if (currentIndent > previousIndent)
        {
            // Увеличился отступ - новый блок
            tokens.Add(new Token(Tokens.Indent));
            _indentStack.Push(currentIndent);
        }
        else if (currentIndent < previousIndent)
        {
            // Уменьшился отступ - закрываем блоки
            while (_indentStack.Count > 0 && _indentStack.Peek() > currentIndent)
            {
                tokens.Add(new Token(Tokens.Dedent));
                _indentStack.Pop();
            }

            // Проверка на некорректный отступ
            if (_indentStack.Peek() != currentIndent)
            {
                throw new Exception($"Inconsistent indentation: expected {_indentStack.Peek()}, got {currentIndent}");
            }
        }
    }

    private int CountLeadingSpaces(string line)
    {
        var count = 0;
        foreach (var c in line)
        {
            if (c == ' ')
                count++;
            else if (c == '\t')
                count += 4; // Табуляция = 4 пробела
            else
                break;
        }
        return count;
    }

    private static bool IsIdentifierStart(char c)
    {
        return char.IsLetter(c) || char.IsNumber(c) || c == '_';
    }

    private static bool IsIdentifierChar(char c)
    {
        return char.IsLetterOrDigit(c) || c == '_';
    }

    private bool TryWordToToken(string word, out Token? token)
    {
        if (IsKeyword(word, out token))
            return true;
        
        // Если не ключевое слово - это идентификатор
        token = new Token(Tokens.Identifier, word);
        return true;
    }

    private static bool IsVariable(string word, out Token? token)
    {
        if (!word.StartsWith("$") || word.Length < 2)
        {
            token = null;
            return false;
        }
        
        token = new Token(Tokens.Variable, word[1..]); // Без $
        return true;
    }

    private static bool IsKeyword(string word, out Token? token)
    {
        // Список ключевых слов твоего языка
        string[] keywords = ["class", "function", "if", "else", "while", "false", "true", "do_while", "return", "static",
                "for", "include", "break", "continue"];
        
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
            '=' => new Token(Tokens.Equals),
            '+' => new Token(Tokens.Plus),
            '-' => new Token(Tokens.Minus),
            '*' => new Token(Tokens.Star),
            '/' => new Token(Tokens.Slash),
            '(' => new Token(Tokens.LParen),
            ')' => new Token(Tokens.RParen),
            ':' => new Token(Tokens.Colon),
            ',' => new Token(Tokens.Comma),
            '.' => new Token(Tokens.Dot),
            '!' => new Token(Tokens.Not),
            '>' => new Token(Tokens.Greater),
            '<' => new Token(Tokens.Less),
            var _ => null
        };
        
        return token != null;
    }
    
    // public List<Token> Lex(string script)
    // {
    //     List<Token> tokens = [];
    //
    //     List<string> scriptLines = script.Split("\n").ToList();
    //
    //     foreach (string line in scriptLines.Select(line => line.Trim()))
    //     {
    //         int startPos = 0;
    //         int pos = 0;
    //         foreach (char letter in line)
    //         {
    //             Token token;
    //             
    //             if (TryCharToToken(letter, out token))
    //             {
    //                 tokens.Add(token);
    //                 startPos = pos;
    //                 continue;
    //             }
    //
    //             string word = line[startPos..pos];
    //             if (TryWordToToken(word, out token))
    //             {
    //                 tokens.Add(token);
    //                 startPos = pos;
    //                 continue;
    //             }
    //                   
    //             Console.WriteLine("Failed to detect: " + letter);
    //             pos++;
    //         }
    //     }
    //
    //     return tokens;
    // }
    //
    // private bool TryWordToToken(string word, out Token? token)
    // {
    //     if (IsVariable(word, out token))
    //         return true;
    //     if (IsKeyword(word, out token))
    //         return true;
    //     
    //     return false;
    // }
    //
    // private bool IsVariable(string word, out Token? token)
    // {
    //     if (!word.StartsWith("$"))
    //     {
    //         token = null;
    //         return false;
    //     }
    //     
    //     token = new Token(Tokens.Variable, word[1..]);
    //     return true;
    // }
    //
    // private bool IsKeyword(string word, out Token? token)
    // {
    //     if (Enum.TryParse(word, true, out Tokens tk))
    //     {
    //         token = new Token(tk, word);
    //         return true;
    //     }
    //     
    //     token = null;
    //     return false;
    // }
    //
    // private bool TryCharToToken(char keychar, out Token? token)
    // {
    //     try
    //     {
    //         token = new Token(keychar switch
    //         {
    //             '=' => Tokens.Equals,
    //             '+' => Tokens.Plus,
    //             '-' => Tokens.Minus,
    //             '*' => Tokens.Star,
    //             '/' => Tokens.Slash,
    //             '(' => Tokens.LParen,
    //             ')' => Tokens.RParen,
    //             ':' => Tokens.Colon,
    //             ',' => Tokens.Comma,
    //             '.' => Tokens.Dot,
    //             '\n' => Tokens.NewLine,
    //         });
    //     }
    //     catch (Exception e)
    //     {
    //         token = null;
    //     }
    //     
    //     return token == null;
    // }
}