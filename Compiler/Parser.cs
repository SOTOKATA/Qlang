using System.Text.RegularExpressions;
using Qlang.AST;
using Qlang.Dependencies;

namespace Qlang.Compiler;

public class Parser
{
    private List<Token> _tokens = [];
    private int _position = 0;
    
    /// <summary>
    /// Парсит список токенов и строит дерево абстрактного синтаксиса (AST).
    /// </summary>
    /// <param name="tokens">Список токенов для парсинга.</param>
    /// <returns>Корневой узел программы с набором операторов.</returns>
     public ProgramNode Parse(List<Token> tokens)
    {
        _tokens = tokens;
        _position = 0;

        var program = new ProgramNode();

        while (!IsAtEnd())
        {
            // Пропускаем пустые строки
            if (Check(Tokens.NewLine))
            {
                Advance();
                continue;
            }

            // Console.WriteLine("Parsing Statement");
            program.Statements.Add(ParseStatement());
        }

        return program;
    }

    /// <summary>
    /// Парсит отдельный оператор: объявление функции, if-блок, присваивание или вызов метода.
    /// </summary>
    /// <returns>Узел AST, представляющий оператор.</returns>
    /// <exception cref="Exception">Выбрасывается при обнаружении неожиданного токена.</exception>
    private ASTNode ParseStatement()
    {
        // function declaration
        if (Check(Tokens.Keyword) && Current().Value == "function")
        {
            // Console.WriteLine("Detected: Function declaration");
            return ParseFunction();
        }

        // if statement
        if (Check(Tokens.Keyword) && Current().Value == "if")
        {
            // Console.WriteLine("Detected: If block");
            return ParseIf();
        }
        
        // return statement
        if (Check(Tokens.Keyword) && Current().Value == "return")
        {
            // Console.WriteLine("Detected: If block");
            return ParseReturn();
        }

        // assignment
        if (Check(Tokens.Variable))
        {
            // Console.WriteLine("Detected: Variable assignment");
            return ParseAssignment();
        }

        // method call (Term.print(...))
        if (Check(Tokens.Identifier))
        {
            // Console.WriteLine("Detected: Method call");
            return ParseMethodCall();
        }

        throw new Exception($"Unexpected token: {Current().TokenType}");
    }

    /// <summary>
    /// Парсит объявление функции с параметрами и телом.
    /// </summary>
    /// <returns>Узел функции с именем, параметрами и телом.</returns>
    private FunctionNode ParseFunction()
    {
        Expect(Tokens.Keyword, "function");
        string name = Expect(Tokens.Identifier).Value;
        Expect(Tokens.LParen);
        
        List<string> parameters = [];
        while (!Check(Tokens.RParen))
        {
            if (Check(Tokens.Variable))
                parameters.Add(Advance().Value);
            if (Check(Tokens.Comma))
                Advance();
        }
        
        Expect(Tokens.RParen);
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        var body = ParseBlock();

        Expect(Tokens.Dedent);

        return new FunctionNode { Name = name, Parameters = parameters, Body = body };
    }

    /// <summary>
    /// Парсит условный оператор if с возможным else или else-if блоком.
    /// </summary>
    /// <returns>Узел условного оператора с условием, then-блоком и опциональным else-блоком.</returns>
    private IfNode ParseIf()
    {
        Expect(Tokens.Keyword, "if");
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        var thenBlock = ParseBlock();

        Expect(Tokens.Dedent);

        var elseBlock = new List<ASTNode>();
        
        // else или else if
        if (Check(Tokens.Keyword) && Current().Value == "else")
        {
            Advance();
            
            if (Check(Tokens.Keyword) && Current().Value == "if")
                // else if - парсим как новый if
                elseBlock.Add(ParseIf());
            else
            {
                // просто else
                Expect(Tokens.Colon);
                Expect(Tokens.NewLine);
                Expect(Tokens.Indent);
                elseBlock = ParseBlock();
                Expect(Tokens.Dedent);
            }
        }

        return new IfNode { Condition = condition, ThenBlock = thenBlock, ElseBlock = elseBlock };
    }

    /// <summary>
    /// Парсит блок операторов (используется в функциях и условных операторах).
    /// </summary>
    /// <returns>Список узлов AST, представляющих операторы в блоке.</returns>
    private List<ASTNode> ParseBlock()
    {
        var statements = new List<ASTNode>();

        while (!Check(Tokens.Dedent) && !IsAtEnd())
        {
            if (Check(Tokens.NewLine))
            {
                Advance();
                continue;
            }
            statements.Add(ParseStatement());
        }

        return statements;
    }

    /// <summary>
    /// Парсит присваивание значения переменной.
    /// </summary>
    /// <returns>Узел присваивания с именем переменной и выражением значения.</returns>
    private AssignmentNode ParseAssignment()
    {
        string varName = Expect(Tokens.Variable).Value;
        Expect(Tokens.Equals);
        var value = ParseExpression();
        Expect(Tokens.NewLine);

        return new AssignmentNode { VariableName = varName, Value = value };
    }

    /// <summary>
    /// Парсит вызов метода объекта (например, Object.method(args)).
    /// </summary>
    /// <returns>Узел вызова метода с именем объекта, методом и аргументами.</returns>
    private MethodCallNode ParseMethodCall()
    {
        string objectName = Expect(Tokens.Identifier).Value;
        // Console.WriteLine("ParseMethodCall/Object Name: " + objectName);
        Expect(Tokens.Dot);
        string methodName = Expect(Tokens.Identifier).Value;
        // Console.WriteLine("ParseMethodCall/Method Name: " + methodName);
        Expect(Tokens.LParen);

        List<ASTNode> arguments = [];
        while (!Check(Tokens.RParen))
        {
            arguments.Add(ParseExpression());
            if (Check(Tokens.Comma))
                Advance();
        }

        Expect(Tokens.RParen);
        
        if (Peek()?.TokenType == Tokens.NewLine)
            Expect(Tokens.NewLine);

        return new MethodCallNode { ObjectName = objectName, MethodName = methodName, Arguments = arguments };
    }

    private ASTNode ParseReturn()
    {
        Expect(Tokens.Keyword);
        
        ASTNode node = ParsePrimary();
        
        Expect(Tokens.NewLine);
        
        return new ReturnNode { ReturnValue = node };
    }

    /// <summary>
    /// Парсит выражение, включая операторы сравнения и арифметические операции.
    /// </summary>
    /// <returns>Узел AST, представляющий выражение (может быть бинарной операцией или примитивом).</returns>
    private ASTNode ParseExpression()
    {
        var left = ParsePrimary();

        // Операторы сравнения: ==, !=, <, >, etc.
        if (Check(Tokens.Equals) && Peek()?.TokenType == Tokens.Equals)
        {
            Advance(); // первый =
            Advance(); // второй =
            var right = ParseExpression();
            return new BinaryOperationNode { Left = left, Operator = "==", Right = right };
        }

        // Арифметические операторы: +, -, *, /
        if (Check(Tokens.Plus) || Check(Tokens.Minus) || Check(Tokens.Star) || Check(Tokens.Slash))
        {
            string op = Current().TokenType.ToString();
            Advance();
            ASTNode right = ParseExpression();
            return new BinaryOperationNode { Left = left, Operator = op, Right = right };
        }

        return left;
    }

    /// <summary>
    /// Парсит примитивное выражение: переменную, строковую ссылку или идентификатор.
    /// </summary>
    /// <returns>Узел AST, представляющий примитивное значение.</returns>
    /// <exception cref="Exception">Выбрасывается при обнаружении неожиданного токена в выражении.</exception>
    private ASTNode ParsePrimary()
    {
        // Variable
        if (Check(Tokens.Variable))
            return new VariableNode { Name = Advance().Value };

        // String reference
        if (Check(Tokens.StringRef))
            return new StringRefNode { Index = int.Parse(Advance().Value) };

        // Identifier (для строк типа ___STRING_0___)
        if (Check(Tokens.Identifier))
        {
            string value = Current().Value;
            if (!value.StartsWith("___STRING_")) 
                return ParseStatement();
            
            Advance();
            int index = int.Parse(value.Replace("___STRING_", "").Replace("___", ""));
            return new StringRefNode { Index = index };

        }
        
        throw new Exception($"Unexpected token in expression: {Current().TokenType}");
    }

    // Вспомогательные методы
    private Token Current() => _tokens[_position];
    private Token? Peek() => _position + 1 < _tokens.Count ? _tokens[_position + 1] : null;
    private bool IsAtEnd() => _position >= _tokens.Count;
    private bool Check(Tokens type) => !IsAtEnd() && Current().TokenType == type;
    
    private Token Advance()
    {
        if (!IsAtEnd()) _position++;
        // Console.WriteLine("TokenType: " + _tokens[_position - 1].TokenType);
        return _tokens[_position - 1];
    }

    /// <summary>
    /// Проверяет, что текущий токен соответствует ожидаемому типу и значению, затем перемещает позицию.
    /// </summary>
    /// <param name="type">Ожидаемый тип токена.</param>
    /// <param name="value">Ожидаемое значение токена (опционально).</param>
    /// <returns>Токен, соответствующий ожидаемому типу и значению.</returns>
    /// <exception cref="Exception">Выбрасывается, если токен не соответствует ожидаемому типу или значению.</exception>
    private Token Expect(Tokens type, string? value = null)
    {
        if (!Check(type))
        {
            Token current = Current();
            
            throw new Exception($"Expected {type}, got {current.TokenType} (Value: {(current.Value == "" ? "Null" : current.Value)}) ");
        }
        
        if (value != null && Current().Value != value)
            throw new Exception($"Expected '{value}', got '{Current().Value}'");

        return Advance();
    }
}

