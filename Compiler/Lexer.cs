namespace Qlang.Compiler;

public class Lexer()
{
    private readonly Stack<int> _indentStack = new([0]);

    public List<Token> Lex(string script)
    {
        List<Token> tokens = [];
        List<string> scriptLines = script.Split('\n').ToList();

        for (var index = 0; index < scriptLines.Count; index++)
        {
            var rawLine = scriptLines[index];
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
                    charToken.Line = index;
                    charToken.Index = pos;
                    
                    tokens.Add(charToken);
                    pos++;
                    continue;
                }

                // Читаем слово/переменную/идентификатор
                var startPos = pos;

                // Обычное слово (keyword или identifier)
                if (IsIdentifierStart(line[pos]))
                {
                    while (pos < line.Length && IsIdentifierChar(line[pos]))
                        pos++;

                    var word = line[startPos..pos];
                    if (TryWordToToken(word, out var wordToken))
                    {
                        wordToken.Line = index;
                        wordToken.Index = pos;
                        
                        tokens.Add(wordToken!);
                        continue;
                    }
                }

                // Если ничего не подошло
                Console.WriteLine($"Failed to detect: '{line[pos]}' at position {pos}");
                pos++;
            }

            tokens.Add(new Token(Tokens.NewLine, index, pos));
        }

        // Закрываем все открытые блоки в конце файла
        while (_indentStack.Count > 1)
        {
            tokens.Add(new Token(Tokens.Dedent, -1, -1));
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
            tokens.Add(new Token(Tokens.Indent, -1, -1));
            _indentStack.Push(currentIndent);
        }
        else if (currentIndent < previousIndent)
        {
            // Уменьшился отступ - закрываем блоки
            while (_indentStack.Count > 0 && _indentStack.Peek() > currentIndent)
            {
                tokens.Add(new Token(Tokens.Dedent, -1, -1));
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
        token = new Token(Tokens.Identifier, -1, -1, word);
        return true;
    }

    private static bool IsKeyword(string word, out Token? token)
    {
        // Список ключевых слов твоего языка
        string[] keywords = ["class", "function", "if", "let", "else", "while", "false", "true", "do_while", "return", "static",
                "for", "include", "break", "continue"];
        
        if (keywords.Contains(word.ToLower()))
        {
            token = new Token(Tokens.Keyword, -1, -1, word);
            return true;
        }
        
        token = null;
        return false;
    }

    private static bool TryCharToToken(char keychar, out Token? token)
    {
        token = keychar switch
        {
            '=' => new Token(Tokens.Equals, -1, -1),
            '+' => new Token(Tokens.Plus, -1, -1),
            '-' => new Token(Tokens.Minus, -1, -1),
            '*' => new Token(Tokens.Star, -1, -1),
            '/' => new Token(Tokens.Slash, -1, -1),
            '(' => new Token(Tokens.LParen, -1, -1),
            ')' => new Token(Tokens.RParen, -1, -1),
            ':' => new Token(Tokens.Colon, -1, -1),
            ',' => new Token(Tokens.Comma, -1, -1),
            '.' => new Token(Tokens.Dot, -1, -1),
            '!' => new Token(Tokens.Not, -1, -1),
            '>' => new Token(Tokens.Greater, -1, -1),
            '<' => new Token(Tokens.Less, -1, -1),
            var _ => null
        };
        
        return token != null;
    }
}