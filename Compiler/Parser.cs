using Qlang.AST;
using Qlang.Dependencies;
using Exception = System.Exception;

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
        Logger.Logger.Log($"CompilationProcess: Parsing statement ({Current().TokenType}, {Current().Value})");

        bool isStatic = false;
        bool isConst = false;
        bool isPrivate = false;

        if (Check(Tokens.Keyword) && Current().Value == "static")
        {
            Logger.Logger.Warn("IsStatic = true");
            isStatic = true;
            Advance();
        }
        
        if (Check(Tokens.Keyword) && Current().Value == "const")
        {
            Logger.Logger.Warn("IsConst = true");
            isConst = true;
            Advance();
        }

        if (Check(Tokens.Keyword) && Current().Value == "private")
        {
            Logger.Logger.Warn("IsPrivate = true");
            isPrivate = true;
            Advance();
        }
        
        // function declaration
        if (Check(Tokens.Keyword) && Current().Value == Keywords.FunctionDeclaration)
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return ParseFunction(isStatic, isPrivate);
        }
        
        // class declaration
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ClassDeclaration)
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return ParseClass();
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.ForBlock)
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return ParseFor();
        }
        
        // if statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.IfBlock)
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return ParseIf();
        }
        
        // while statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.WhileBlock)
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return ParseWhile();
        }
        
        // do-while statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.DoWhileBlock)
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return ParseWhile(true);
        }
        
        // return statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ReturnKeyword)
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return ParseReturn();
        }

        // assignment
        if (Check(Tokens.Keyword) && Current().Value == Keywords.VariableDeclaration)
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return ParseVariableDeclaration(isStatic, isPrivate, isConst);
        }

        // method call statement (Object.method(...)) || Call method from class pointer
        if (Check(Tokens.Identifier))
        {
            var expr = ParseExpression();
            Expect(Tokens.NewLine);
            Logger.Logger.Log("CompilationProcess.End: Parsing statement");
            return expr;
        }

        throw new Exception($"Unexpected token: {Current().TokenType}");
    }

    private AssignmentNode ParseVariableDeclaration(bool isStatic = false, bool isPrivate = false, bool isConst = false)
    {
        Logger.Logger.Log("CompilationProcess: Parsing variable declaration");
        if (!Check(Tokens.Keyword) || Current().Value != Keywords.VariableDeclaration)
            throw new Exception($"(ParseVariableDeclaration) Unexpected token: {Current().TokenType}");
        
        Advance();
        
        var name = Expect(Tokens.Identifier).Value;

        ASTNode value = null;
        if (Current().TokenType == Tokens.Equals)
        {
            Advance();
            value = ParseExpression();
        }
        
        Logger.Logger.Log($"CompilationProcess.End: Parsing Variable declaration (Name: {name} Value: {value?.GetType().Name ?? "Null"})");
        return new AssignmentNode(isStatic, isPrivate, isConst) { 
            VariableName = name, 
            Value = value
        };
    }

    private FunctionNode ParseFunction(bool isStatic = false, bool isPrivate = false)
    {
        Logger.Logger.Log("CompilationProcess: Parsing function");
        
        Expect(Tokens.Keyword, Keywords.FunctionDeclaration);
        var name = Expect(Tokens.Identifier).Value;
        Expect(Tokens.LParen);
        
        List<AssignmentNode> parameters = [];
        while (!Check(Tokens.RParen))
        {
            if (Check(Tokens.Keyword))
                parameters.Add(ParseVariableDeclaration());
            if (Check(Tokens.Comma))
                Advance();
        }
        
        Expect(Tokens.RParen);
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        List<ASTNode> body = ParseBlock();

        Expect(Tokens.Dedent);

        Logger.Logger.Log("CompilationProcess.End: Parsing function");
        return new FunctionNode 
        { Name = name, 
            Parameters = parameters, 
            Body = body, 
            IsStatic = isStatic, 
            IsPrivate = isPrivate 
        };
    }

    private ClassNode ParseClass()
    {
        Logger.Logger.Log("CompilationProcess: Parsing class");
        Expect(Tokens.Keyword, Keywords.ClassDeclaration);
        var name = Expect(Tokens.Identifier).Value;
        
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        List<ASTNode> body = ParseBlock();

        Expect(Tokens.Dedent);

        Logger.Logger.Log("CompilationProcess.End: Parsing class");
        return new ClassNode { Name = name, Body = body };
    }
    
    private ForNode ParseFor()
    {
        Logger.Logger.Log("CompilationProcess: Parsing for");
        Expect(Tokens.Keyword, Keywords.ForBlock);
        
        AssignmentNode assignment = ParseVariableDeclaration();
        Expect(Tokens.CommaColon);
        var condition = ParseExpression();
        Expect(Tokens.CommaColon);
        var statement = ParseExpression();
        
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        List<ASTNode> forBlock = ParseBlock();

        Expect(Tokens.Dedent);

        Logger.Logger.Log("CompilationProcess.End: Parsing for");
        return new ForNode { Assignment = assignment, Statement = statement, Condition = condition, Body = forBlock };
    }
    
    private WhileNode ParseWhile(bool isDoWhile = false)
    {
        Logger.Logger.Log("CompilationProcess: Parsing while");
        Expect(Tokens.Keyword, isDoWhile ? Keywords.DoWhileBlock : Keywords.WhileBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        List<ASTNode> whileBlock = ParseBlock();

        Expect(Tokens.Dedent);

        Logger.Logger.Log("CompilationProcess.End: Parsing while");
        return new WhileNode { Condition = condition, Body = whileBlock, IsDoWhile = isDoWhile };
    }

    private IfNode ParseIf()
    {
        Logger.Logger.Log("CompilationProcess: Parsing if");
        Expect(Tokens.Keyword, Keywords.IfBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        Expect(Tokens.NewLine);
        Expect(Tokens.Indent);

        List<ASTNode> thenBlock = ParseBlock();

        Expect(Tokens.Dedent);

        List<ASTNode> elseBlock = [];
        
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ElseBlock)
        {
            Advance();
            
            if (Check(Tokens.Keyword) && Current().Value == Keywords.IfBlock)
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

        Logger.Logger.Log("CompilationProcess.End: Parsing if");
        return new IfNode { Condition = condition, ThenBlock = thenBlock, ElseBlock = elseBlock };
    }

    private List<ASTNode> ParseBlock()
    {
        List<ASTNode> statements = [];
        Logger.Logger.Log("CompilationProcess: Parsing block");

        while (!Check(Tokens.Dedent) && !IsAtEnd())
        {
            if (Check(Tokens.NewLine))
            {
                Advance();
                continue;
            }
            statements.Add(ParseStatement());
        }

        Logger.Logger.Log("CompilationProcess.End: Parsing block");
        return statements;
    }

    private ReturnNode ParseReturn()
    {
        Logger.Logger.Log("CompilationProcess: Parsing return");
        Expect(Tokens.Keyword);
        
        var node = ParseExpression();
        
        Expect(Tokens.NewLine);
        
        Logger.Logger.Log("CompilationProcess.End: Parsing return");
        return new ReturnNode { ReturnValue = node };
    }

    /// <summary>
    /// Парсит выражение с бинарными операторами (==, +, -, *, /)
    /// </summary>
    private ASTNode ParseExpression()
    {
        Logger.Logger.Log("CompilationProcess: Parsing expression");
        var left = ParsePrimary();
        
        if (Check(Tokens.And) && Peek()?.TokenType == Tokens.And)
        {
            Advance(); // первый &
            Advance(); // второй &
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = "&&", Right = right };
        }
    
        if (Check(Tokens.Or) && Peek()?.TokenType == Tokens.Or)
        {
            Advance(); // первый |
            Advance(); // второй |
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = "||", Right = right };
        }

        // Операторы сравнения: ==
        if (Check(Tokens.Equals) && Peek()?.TokenType == Tokens.Equals)
        {
            Advance(); // первый =
            Advance(); // второй =
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = "==", Right = right };
        }
        
        if (Check(Tokens.Not) && Peek()?.TokenType == Tokens.Equals)
        {
            Advance(); // первый !
            Advance(); // второй =
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
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
            
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
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
            
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = op, Right = right };
        }

        // Арифметические операторы: +, -, *, /
        if (Check(Tokens.Plus) || Check(Tokens.Minus) || Check(Tokens.Star) || Check(Tokens.Slash))
        {
            var op = Current().TokenType.ToString();
            Advance();
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = op, Right = right };
        }

        return left;
    }

    /// <summary>
    /// Парсит примитивные значения и вызовы функций/методов
    /// </summary>
    private ASTNode ParsePrimary()
    {
        Logger.Logger.Log("CompilationProcess: Parsing primary");
        // bool return
        if (Check(Tokens.Keyword) && (Current().Value == Keywords.FalseKeyword || Current().Value == Keywords.TrueKeyword))
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing primary");
            return new BooleanNode { Value = bool.Parse(Advance().Value) };
        }

        // String reference
        if (Check(Tokens.StringRef))
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing primary (StringRef)");
            return new StringRefNode { Index = int.Parse(Advance().Value) };
        }
        
        if (Check(Tokens.NumberRef))
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing primary (NumberRef)");
            return new NumberRefNode { Index = int.Parse(Advance().Value) };
        }

        // Identifier - может быть вызовом метода или функции
        if (Check(Tokens.Identifier))
        {
            var firstIdentifier = Advance().Value;
            
            // Проверяем на специальные строковые константы
            if (firstIdentifier.StartsWith("___STRING_"))
            {
                var index = int.Parse(firstIdentifier.Replace("___STRING_", "").Replace("___", ""));
                return new StringRefNode { Index = index };
            }
            
            if (firstIdentifier.StartsWith("___NUMBER_"))
            {
                var index = int.Parse(firstIdentifier.Replace("___NUMBER_", "").Replace("___", ""));
                return new NumberRefNode { Index = index };
            }
            
            if (firstIdentifier.TryParseNumber(out var result))
                return new NumberNode { Value = result };
            
            // Вызов метода: Object.method(...)
            if (Check(Tokens.Dot))
            {
                Advance(); // consume '.'
                
                var name = Expect(Tokens.Identifier).Value;

                // Call variable from class
                // structure: objName.name <- without '('
                if (Current().TokenType != Tokens.LParen)
                {
                    // TODO: variable assign
                
                    // get from class
                    Logger.Logger.Log("CompilationProcess.End: Parsing primary (VariableNode)");
                    return new VariableNode { ClassName = firstIdentifier, Name = name };
                }
                
                Expect(Tokens.LParen);

                List<ASTNode> arguments = [];
                while (!Check(Tokens.RParen))
                {
                    arguments.Add(ParseExpression());
                    if (Check(Tokens.Comma))
                        Advance();
                }

                Expect(Tokens.RParen);

                Logger.Logger.Log("CompilationProcess.End: Parsing primary");
                return new MethodCallNode 
                { 
                    ObjectName = firstIdentifier, 
                    MethodName = name, 
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
                Logger.Logger.Log("CompilationProcess.End: Parsing primary");
                return new MethodCallNode 
                { 
                    ObjectName = "", 
                    MethodName = firstIdentifier, 
                    Arguments = arguments 
                };
            }

            if (Current().TokenType == Tokens.Equals && Peek()?.TokenType != Tokens.Equals)
            {
                Logger.Logger.Log($"CompilationProcess.End: Parsing primary (AssignmentNode)");
                Advance();
                return new AssignmentNode(false, false, false)
                {
                    VariableName = firstIdentifier,
                    Value = ParseExpression(),
                };
            }
            
            Logger.Logger.Log($"CompilationProcess.End: Parsing primary (VariableNode: {firstIdentifier})");
            return new VariableNode { Name = firstIdentifier };
        }
        
        throw new Exception($"Unexpected token in expression: {Current().TokenType} ({(Current().Value == "" ? "Null" : Current().Value)})");
    }

    // Вспомогательные методы
    private Token Current() => _tokens[_position];
    private Token? Peek() => _position + 1 < _tokens.Count ? _tokens[_position + 1] : null;
    private bool IsAtEnd() => _position >= _tokens.Count;
    private bool Check(Tokens type) => !IsAtEnd() && Current().TokenType == type;
    
    private Token Advance()
    {
        if (!IsAtEnd()) _position++;
        var token = _tokens[_position - 1];
        
        Logger.Logger.Log($"Token (Ln:{token.Line} Idx:{token.Index}): " + token.TokenType);
        return token;
    }

    private Token Expect(Tokens type, string? value = null)
    {
        if (!Check(type))
        {
            var current = Current();
            throw new Exception($"Expected {type}, got {current.TokenType} (Value: {(current.Value == "" ? "Null" : current.Value)}) ");
        }
        
        if (value != null && Current().Value != value)
            throw new Exception($"Expected '{value}', got '{Current().Value}'");

        return Advance();
    }
}