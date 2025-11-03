using System.Text.RegularExpressions;
using Qlang.AST;
using Qlang.Dependencies;

namespace Qlang.Compiler;

public class Parser(Compiler compiler)
{
    Compiler _compiler = compiler;

    private List<Token> _tokens = [];
    private int _position = 0;
    
    
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

            program.Statements.Add(ParseStatement());
        }

        return program;
    }

    private ASTNode ParseStatement()
    {
        // function declaration
        if (Check(Tokens.Keyword) && Current().Value == "function")
        {
            return ParseFunction();
        }

        // if statement
        if (Check(Tokens.Keyword) && Current().Value == "if")
        {
            return ParseIf();
        }

        // assignment
        if (Check(Tokens.Variable))
        {
            return ParseAssignment();
        }

        // method call (Term.print(...))
        if (Check(Tokens.Identifier))
        {
            return ParseMethodCall();
        }

        throw new Exception($"Unexpected token: {Current().TokenType}");
    }

    private FunctionNode ParseFunction()
    {
        Expect(Tokens.Keyword, "function");
        string name = Expect(Tokens.Identifier).Value;
        Expect(Tokens.LParen);
        
        var parameters = new List<string>();
        while (!Check(Tokens.RParen))
        {
            if (Check(Tokens.Variable))
            {
                parameters.Add(Advance().Value);
            }
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
            {
                // else if - парсим как новый if
                elseBlock.Add(ParseIf());
            }
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

    private AssignmentNode ParseAssignment()
    {
        string varName = Expect(Tokens.Variable).Value;
        Expect(Tokens.Equals);
        var value = ParseExpression();
        Expect(Tokens.NewLine);

        return new AssignmentNode { VariableName = varName, Value = value };
    }

    private MethodCallNode ParseMethodCall()
    {
        string objectName = Expect(Tokens.Identifier).Value;
        Expect(Tokens.Dot);
        string methodName = Expect(Tokens.Identifier).Value;
        Expect(Tokens.LParen);

        var arguments = new List<ASTNode>();
        while (!Check(Tokens.RParen))
        {
            arguments.Add(ParseExpression());
            if (Check(Tokens.Comma))
                Advance();
        }

        Expect(Tokens.RParen);
        Expect(Tokens.NewLine);

        return new MethodCallNode { ObjectName = objectName, MethodName = methodName, Arguments = arguments };
    }

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
            var right = ParseExpression();
            return new BinaryOperationNode { Left = left, Operator = op, Right = right };
        }

        return left;
    }

    private ASTNode ParsePrimary()
    {
        // Variable
        if (Check(Tokens.Variable))
        {
            return new VariableNode { Name = Advance().Value };
        }

        // String reference
        if (Check(Tokens.StringRef))
        {
            return new StringRefNode { Index = int.Parse(Advance().Value) };
        }

        // Identifier (для строк типа ___STRING_0___)
        if (Check(Tokens.Identifier))
        {
            string value = Current().Value;
            if (value.StartsWith("___STRING_"))
            {
                Advance();
                int index = int.Parse(value.Replace("___STRING_", "").Replace("___", ""));
                return new StringRefNode { Index = index };
            }
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
        return _tokens[_position - 1];
    }

    private Token Expect(Tokens type, string? value = null)
    {
        if (!Check(type))
            throw new Exception($"Expected {type}, got {Current().TokenType}");
        
        if (value != null && Current().Value != value)
            throw new Exception($"Expected '{value}', got '{Current().Value}'");

        return Advance();
    }
}

