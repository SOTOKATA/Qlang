using Qlang.AST;

namespace Qlang.Compiler;

public class Parser
{
    private List<Token> _tokens = [];
    private int _position = 0;
    
    public ProgramNode Parse(List<Token> tokens)
    {
        _tokens = tokens;
        _position = 0;

        ProgramNode program = new();

        while (!IsAtEnd())
        {
            if (Check(Tokens.NewLine))
            {
                Advance();
                continue;
            }

            program.Statements.Add(ParseStatement());
        }

        return program;
    }

    private ASTNode ParseStatement()
    {
        // function declaration
        if (Check(Tokens.Keyword) && Current().Value == "function")
            return ParseFunction();
        
        // class declaration
        if (Check(Tokens.Keyword) && Current().Value == "class")
            return ParseClass();

        // if statement
        if (Check(Tokens.Keyword) && Current().Value == "if")
            return ParseIf();
        
        // while statement
        if (Check(Tokens.Keyword) && Current().Value == "while")
            return ParseWhile();
        
        // do-while statement
        if (Check(Tokens.Keyword) && Current().Value == "do_while")
            return ParseWhile(true);
        
        // return statement
        if (Check(Tokens.Keyword) && Current().Value == "return")
            return ParseReturn();

        // assignment
        if (Check(Tokens.Variable))
            return ParseAssignment();

        // method call statement (Object.method(...))
        if (Check(Tokens.Identifier))
        {
            ASTNode expr = ParseExpression();
            Expect(Tokens.NewLine);
            return expr;
        }

        throw new Exception($"Unexpected token: {Current().TokenType}");
    }

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

        List<ASTNode> body = ParseBlock();

        Expect(Tokens.Dedent);

        return new FunctionNode { Name = name, Parameters = parameters, Body = body };
    }

    private ClassNode ParseClass()
    {
        Expect(Tokens.Keyword, "class");
        string name = Expect(Tokens.Identifier).Value;
        
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        List<ASTNode> body = ParseBlock();

        Expect(Tokens.Dedent);

        return new ClassNode { Name = name, Body = body };
    }
    
    private WhileNode ParseWhile(bool isDoWhile = false)
    {
        Expect(Tokens.Keyword, isDoWhile ? "do_while" : "while");
        ASTNode condition = ParseExpression();
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        List<ASTNode> whileBlock = ParseBlock();

        Expect(Tokens.Dedent);

        return new WhileNode { Condition = condition, Body = whileBlock, IsDoWhile = isDoWhile };
    }

    private IfNode ParseIf()
    {
        Expect(Tokens.Keyword, "if");
        ASTNode condition = ParseExpression();
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        List<ASTNode> thenBlock = ParseBlock();

        Expect(Tokens.Dedent);

        List<ASTNode> elseBlock = [];
        
        if (Check(Tokens.Keyword) && Current().Value == "else")
        {
            Advance();
            
            if (Check(Tokens.Keyword) && Current().Value == "if")
                elseBlock.Add(ParseIf());
            else
            {
                Expect(Tokens.Colon);
                Expect(Tokens.NewLine);
                Expect(Tokens.Indent);
                elseBlock = ParseBlock();
                Expect(Tokens.Dedent);
            }
        }

        return new IfNode { Condition = condition, ThenBlock = thenBlock, ElseBlock = elseBlock };
    }

    private List<ASTNode> ParseBlock()
    {
        List<ASTNode> statements = [];

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

    private AssignmentNode ParseAssignment()
    {
        string varName = Expect(Tokens.Variable).Value;
        Expect(Tokens.Equals);
        ASTNode value = ParseExpression();
        Expect(Tokens.NewLine);

        return new AssignmentNode { VariableName = varName, Value = value };
    }

    private ReturnNode ParseReturn()
    {
        Expect(Tokens.Keyword);
        
        ASTNode node = ParseExpression();
        
        Expect(Tokens.NewLine);
        
        return new ReturnNode { ReturnValue = node };
    }

    /// <summary>
    /// Парсит выражение с бинарными операторами (==, +, -, *, /)
    /// </summary>
    private ASTNode ParseExpression()
    {
        ASTNode left = ParsePrimary();

        // Операторы сравнения: ==
        if (Check(Tokens.Equals) && Peek()?.TokenType == Tokens.Equals)
        {
            Advance(); // первый =
            Advance(); // второй =
            ASTNode right = ParseExpression();
            return new BinaryOperationNode { Left = left, Operator = "==", Right = right };
        }
        
        if (Check(Tokens.Not) && Peek()?.TokenType == Tokens.Equals)
        {
            Advance(); // первый !
            Advance(); // второй =
            ASTNode right = ParseExpression();
            return new BinaryOperationNode { Left = left, Operator = "!=", Right = right };
        }
        
        if (Check(Tokens.Less))
        {
            string op;
            if (Peek()?.TokenType == Tokens.Equals)
            {
                Advance(); // <
                Advance(); // =
                op = "<=";
            }
            else
            {
                Advance(); // <
                op = "Less";
            }
            
            ASTNode right = ParseExpression();
            return new BinaryOperationNode { Left = left, Operator = op, Right = right };
        }
        
        if (Check(Tokens.Greater))
        {
            string op;
            if (Peek()?.TokenType == Tokens.Equals)
            {
                Advance(); // >
                Advance(); // =
                op = ">=";
            }
            else
            {
                Advance(); // >
                op = "Greater";
            }
            
            ASTNode right = ParseExpression();
            return new BinaryOperationNode { Left = left, Operator = op, Right = right };
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
    /// Парсит примитивные значения и вызовы функций/методов
    /// </summary>
    private ASTNode ParsePrimary()
    {
        // Variable
        if (Check(Tokens.Variable))
            return new VariableNode { Name = Advance().Value };

        // String reference
        if (Check(Tokens.StringRef))
            return new StringRefNode { Index = int.Parse(Advance().Value) };

        // Identifier - может быть вызовом метода или функции
        if (Check(Tokens.Identifier))
        {
            string firstIdentifier = Advance().Value;
            
            // Проверяем на специальные строковые константы
            if (firstIdentifier.StartsWith("___STRING_"))
            {
                int index = int.Parse(firstIdentifier.Replace("___STRING_", "").Replace("___", ""));
                return new StringRefNode { Index = index };
            }
            
            if (double.TryParse(firstIdentifier, out double result))
                return new NumberNode { Value = result };
            
            // Вызов метода: Object.method(...)
            if (Check(Tokens.Dot))
            {
                Advance(); // consume '.'
                string methodName = Expect(Tokens.Identifier).Value;
                Expect(Tokens.LParen);

                List<ASTNode> arguments = [];
                while (!Check(Tokens.RParen))
                {
                    arguments.Add(ParseExpression());
                    if (Check(Tokens.Comma))
                        Advance();
                }

                Expect(Tokens.RParen);

                return new MethodCallNode 
                { 
                    ObjectName = firstIdentifier, 
                    MethodName = methodName, 
                    Arguments = arguments 
                };
            }
            
            // Вызов функции: functionName(...)
            if (Check(Tokens.LParen))
            {
                Advance(); // consume '('
                
                List<ASTNode> arguments = [];
                while (!Check(Tokens.RParen))
                {
                    arguments.Add(ParseExpression());
                    if (Check(Tokens.Comma))
                        Advance();
                }

                Expect(Tokens.RParen);

                // Можно использовать MethodCallNode или создать отдельный FunctionCallNode
                return new MethodCallNode 
                { 
                    ObjectName = "", 
                    MethodName = firstIdentifier, 
                    Arguments = arguments 
                };
            }
            
            // Просто идентификатор (например, имя переменной без $)
            return new VariableNode { Name = firstIdentifier };
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
        // Console.WriteLine("Current::Token: " + _tokens[_position - 1].TokenType);
        return _tokens[_position - 1];
    }

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