using Qlang.AST;
using Qlang.Dependencies;
using Exception = System.Exception;

namespace Qlang.Compiler;

public class Parser
{
    private List<Token> _tokens = [];
    private int _position = 0;

    private string _line = "";
    
    public ProgramNode Parse(List<Token> tokens)
    {
        _tokens = tokens;
        _position = 0;

        ProgramNode program = new();

        Logger.Logger.SetLoggerPath(Path.Combine("Logs", "Debug", "debug_parser.log"));
        Logger.Logger.Warn("----------- Parser -----------");

        try
        {
            while (!IsAtEnd())
            {
                if (Check(Tokens.Semicolon))
                {
                    Advance();
                    continue;
                }

                program.Statements.Add(ParseStatement());
            }

            Validator.CheckValidate(program);
        }
        catch
        {
            throw;
        }

        return program;
    }

    private ASTNode ParseStatement()
    {
        Logger.Logger.Log($"Parsing statement ({Current().TokenType}, {Current().Value})", "CompilationProcess");

        bool isStatic = false;
        bool isConst = false;
        bool isPrivate = false;

        if (Check(Tokens.Keyword) && Current().Value == Keywords.StaticModificator)
        {
            Logger.Logger.Warn("IsStatic = true");
            isStatic = true;
            Advance();
        }
        
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ConstModificator)
        {
            Logger.Logger.Warn("IsConst = true");
            isConst = true;
            Advance();
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.PrivateModificator)
        {
            Logger.Logger.Warn("IsPrivate = true");
            isPrivate = true;
            Advance();
        }
        
        if (Check(Tokens.Keyword) && Current().Value == Keywords.BreakKeyword)
        {
            Advance();
            return new BreakNode();
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ContinueKeyword)
        {
            Advance();
            return new ContinueNode();
        }
        
        // function declaration
        if (Check(Tokens.Keyword) && Current().Value == Keywords.FunctionDeclaration)
            return ParseFunction(isStatic, isPrivate);
        
        // class declaration
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ClassDeclaration)
            return ParseClass();

        // for statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ForBlock)
            return ParseFor();
        
        // if statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.IfBlock)
            return ParseIf();
        
        // while statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.WhileBlock)
            return ParseWhile();
        
        // do-while statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.DoWhileBlock)
            return ParseWhile(true);
        
        // return statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ReturnKeyword)
            return ParseReturn();

        // assignment
        if (Check(Tokens.Keyword) && Current().Value == Keywords.VariableDeclaration)
            return ParseVariableDeclaration(isStatic, isPrivate, isConst, true);

        // method call statement (Object.method(...)) || Call method from class pointer
        if (Check(Tokens.Identifier))
        {
            var expr = ParseExpression();
            Expect(Tokens.Semicolon);
            Logger.Logger.Log("Ended parsing statement (identifier)", "CompilationProcess");
            return expr;
        }

        throw new QlangCompileException($"Unexpected token: {Current().TokenType} '{Current().Value}'", (IsAtEnd() ? 0 : Current().Line + 1), "Parser");
    }

    private AssignmentNode ParseVariableDeclaration(bool isStatic = false, bool isPrivate = false, bool isConst = false, bool isNew = false)
    {
        Logger.Logger.Log("Parsing variable declaration", "CompilationProcess");
        if (!Check(Tokens.Keyword) || Current().Value != Keywords.VariableDeclaration)
            throw new QlangCompileException($"(ParseVariableDeclaration) Unexpected token: {Current().TokenType}", (IsAtEnd() ? 0 : Current().Line + 1), "Parser");
        
        Advance();
        
        var name = Expect(Tokens.Identifier).Value;

        ASTNode value = null;
        if (Current().TokenType == Tokens.Equals)
        {
            Advance();
            value = ParseExpression();
        }
        
        Logger.Logger.Log($"CompilationProcess.End: Parsing Variable declaration (Name: {name} Value: {value?.GetType().Name ?? "Null"})");
        return new AssignmentNode(isStatic, isPrivate, isConst, isNew) { 
            VariableName = name, 
            Value = value, 
            Line = (IsAtEnd() ? 0 : Current().Line + 1)
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
        // Expect(Tokens.Semicolon);
        Expect(Tokens.LBrace);

        List<ASTNode> body = ParseBlock();

        Expect(Tokens.RBrace);

        Logger.Logger.Log("CompilationProcess.End: Parsing function");
        return new FunctionNode 
        { 
            Name = name, 
            Parameters = parameters, 
            Body = body, 
            IsStatic = isStatic, 
            IsPrivate = isPrivate,
            Line = (IsAtEnd() ? 0 : Current().Line + 1),
        };
    }

    private ClassNode ParseClass()
    {
        Logger.Logger.Log("CompilationProcess: Parsing class");
        Expect(Tokens.Keyword, Keywords.ClassDeclaration);
        var name = Expect(Tokens.Identifier).Value;
        
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);
        Expect(Tokens.LBrace);

        List<ASTNode> body = ParseBlock();

        Expect(Tokens.RBrace);

        Logger.Logger.Log("CompilationProcess.End: Parsing class");
        return new ClassNode { Name = name, Body = body, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
    }
    
    private ForNode ParseFor()
    {
        Logger.Logger.Log("CompilationProcess: Parsing for");
        Expect(Tokens.Keyword, Keywords.ForBlock);
        
        AssignmentNode assignment = ParseVariableDeclaration();
        Expect(Tokens.Semicolon);
        var condition = ParseExpression();
        Expect(Tokens.Semicolon);
        var statement = ParseExpression();
        
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);
        Expect(Tokens.LBrace);

        List<ASTNode> forBlock = ParseBlock();

        Expect(Tokens.RBrace);

        Logger.Logger.Log("CompilationProcess.End: Parsing for");
        return new ForNode { Assignment = assignment, Statement = statement, Condition = condition, Body = forBlock, Line = (IsAtEnd() ? 0 : Current().Line + 1), };
    }
    
    private WhileNode ParseWhile(bool isDoWhile = false)
    {
        Logger.Logger.Log("CompilationProcess: Parsing while");
        Expect(Tokens.Keyword, isDoWhile ? Keywords.DoWhileBlock : Keywords.WhileBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);
        Expect(Tokens.LBrace);

        List<ASTNode> whileBlock = ParseBlock();

        Expect(Tokens.RBrace);

        Logger.Logger.Log("CompilationProcess.End: Parsing while");
        return new WhileNode { Condition = condition, Body = whileBlock, IsDoWhile = isDoWhile,Line = (IsAtEnd() ? 0 : Current().Line + 1) };
    }

    private IfNode ParseIf()
    {
        Logger.Logger.Log("CompilationProcess: Parsing if");
        Expect(Tokens.Keyword, Keywords.IfBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);
        Expect(Tokens.LBrace);

        List<ASTNode> thenBlock = ParseBlock();

        Expect(Tokens.RBrace);

        List<ASTNode> elseBlock = [];
        
        Logger.Logger.Warn(Current().TokenType.ToString());
        Logger.Logger.Warn(Current().Value);
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ElseBlock)
        {
            Advance();
            
            if (Check(Tokens.Keyword) && Current().Value == Keywords.IfBlock)
                elseBlock.Add(ParseIf());
            else
            {
                Expect(Tokens.Colon);
                // Expect(Tokens.Semicolon);
                Expect(Tokens.LBrace);
                elseBlock = ParseBlock();
                Expect(Tokens.RBrace);
            }
        }

        Logger.Logger.Log("CompilationProcess.End: Parsing if");
        return new IfNode { Condition = condition, ThenBlock = thenBlock, ElseBlock = elseBlock,Line = (IsAtEnd() ? 0 : Current().Line + 1) };
    }

    private List<ASTNode> ParseBlock()
    {
        List<ASTNode> statements = [];
        Logger.Logger.Log("CompilationProcess: Parsing block");

        while (!Check(Tokens.RBrace) && !IsAtEnd())
        {
            Logger.Logger.Log("                                         CurerntToken,:" + Current().TokenType + " " + Current().Value);
            if (Check(Tokens.Semicolon))
            {
                Advance();
                continue;
            }
            statements.Add(ParseStatement());
        }

        Logger.Logger.Log("CompilationProcess.End: Parsing block");
        return statements;
    }

    private ASTNode ParseParens()
    {
        Expect(Tokens.LParen);
        var parsed = ParseExpression();
        Expect(Tokens.RParen);

        return parsed;
    }

    private ReturnNode ParseReturn()
    {
        Logger.Logger.Log("CompilationProcess: Parsing return");
        Expect(Tokens.Keyword);
        
        var node = ParseExpression();
        
        Expect(Tokens.Semicolon);
        
        Logger.Logger.Log("CompilationProcess.End: Parsing return");
        return new ReturnNode { ReturnValue = node,Line = (IsAtEnd() ? 0 : Current().Line + 1) };
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
            return new BinaryOperationNode { Left = left, Operator = "&&", Right = right,Line = (IsAtEnd() ? 0 : Current().Line + 1) };
        }
    
        if (Check(Tokens.Or) && Peek()?.TokenType == Tokens.Or)
        {
            Advance(); // первый |
            Advance(); // второй |
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = "||", Right = right,Line = (IsAtEnd() ? 0 : Current().Line + 1) };
        }

        // Операторы сравнения: ==
        if (Check(Tokens.Equals) && Peek()?.TokenType == Tokens.Equals)
        {
            Advance(); // первый =
            Advance(); // второй =
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = "==", Right = right,Line = (IsAtEnd() ? 0 : Current().Line + 1) };
        }
        
        if (Check(Tokens.Not) && Peek()?.TokenType == Tokens.Equals)
        {
            Advance(); // первый !
            Advance(); // второй =
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = "!=", Right = right,Line = (IsAtEnd() ? 0 : Current().Line + 1) };
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
            return new BinaryOperationNode { Left = left, Operator = op, Right = right,Line = (IsAtEnd() ? 0 : Current().Line + 1) };
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
            return new BinaryOperationNode { Left = left, Operator = op, Right = right, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
        }

        // Арифметические операторы: +, -, *, /
        if (Check(Tokens.Plus) || Check(Tokens.Minus) || Check(Tokens.Star) || Check(Tokens.Slash) || Check(Tokens.Percent))
        {
            var op = Current().TokenType.ToString();
            Advance();
            var right = ParseExpression();
            Logger.Logger.Log("CompilationProcess.End: Parsing expression");
            return new BinaryOperationNode { Left = left, Operator = op, Right = right, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
        }

        return left;
    }

    /// <summary>
    /// Парсит примитивные значения и вызовы функций/методов
    /// </summary>
    private ASTNode ParsePrimary()
    {
        Logger.Logger.Log("CompilationProcess: Parsing primary");

        if (Check(Tokens.Keyword) && Current().Value == Keywords.BreakKeyword)
        {
            Advance();
            return new BreakNode();
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ContinueKeyword)
        {
            Advance();
            return new ContinueNode();
        }

        // Array
        if (Check(Tokens.LSquareParen))
        {
            Advance();
            List<ASTNode> statements = [];
            while (!Check(Tokens.RSquareParen) && !IsAtEnd())
            {
                if (Current().TokenType == Tokens.Comma)
                {
                    Advance();
                    continue;
                }
                statements.Add(ParseExpression());
            }
            
            Advance();

            return new CollectionNode { Collection = statements, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
        }
        
        bool isMinus = false;
        if (Check(Tokens.Minus))
        {
            isMinus = true;
            Advance();
        }
        
        // parse: '('expression')'
        if (Check(Tokens.LParen))
        {
            Logger.Logger.Warn("Parsing parents");
            return ParseParens();
        }
        
        // bool return
        if (Check(Tokens.Keyword) && (Current().Value == Keywords.FalseKeyword || Current().Value == Keywords.TrueKeyword))
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing primary");
            return new BooleanNode
            {
                Value = isMinus ? !bool.Parse(Advance().Value) : bool.Parse(Advance().Value), Line = (IsAtEnd() ? 0 : Current().Line + 1)
            };
        }

        // String reference
        if (Check(Tokens.StringRef))
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing primary (StringRef)");
            return new StringRefNode { Index = int.Parse(Advance().Value), Line = (IsAtEnd() ? 0 : Current().Line + 1) };
        }
        
        if (Check(Tokens.NumberRef))
        {
            Logger.Logger.Log("CompilationProcess.End: Parsing primary (NumberRef)");
            return new NumberRefNode
            {
                IsNegative = isMinus,
                Index = int.Parse(Advance().Value)
                , Line = (IsAtEnd() ? 0 : Current().Line + 1)
            };
        }

        // Identifier - может быть вызовом метода или функции
        if (Check(Tokens.Identifier))
        {
            var firstIdentifier = Advance().Value;
            
            // Проверяем на специальные строковые константы
            if (firstIdentifier.StartsWith("___STRING_"))
            {
                var index = int.Parse(firstIdentifier.Replace("___STRING_", "").Replace("___", ""));
                return new StringRefNode { Index = index, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
            }
            
            if (firstIdentifier.StartsWith("___NUMBER_"))
            {
                var index = int.Parse(firstIdentifier.Replace("___NUMBER_", "").Replace("___", ""));
                return new NumberRefNode
                {
                    IsNegative = isMinus,
                    Index = index,
                    Line = (IsAtEnd() ? 0 : Current().Line + 1)
                };
            }
            
            if (firstIdentifier.TryParseNumber(out var result))
                return new NumberNode { Value = result, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
            
            // Вызов метода: Object.method(...)... or func().class.etc();
            if (Check(Tokens.Dot) || Check(Tokens.LParen))
            {
                Logger.Logger.Log("Detected object/function call");
                List<ASTNode> objects = [];
                List<ASTNode> arguments = [];
                
                // If current object is a function
                if (Check(Tokens.LParen))
                {
                    Advance();

                    arguments = [];
                    while (!Check(Tokens.RParen))
                    {
                        arguments.Add(ParseExpression());
                        if (Check(Tokens.Comma))
                            Advance();
                    }

                    Expect(Tokens.RParen);  
                    
                    Logger.Logger.Log("Arguments: " + string.Join(", ", arguments));
                        
                    objects.Add(new FunctionPointerNode
                    {
                        Name = firstIdentifier,
                        Arguments = arguments,
                        Line = (IsAtEnd() ? 0 : Current().Line + 1)
                    });
                }
                // Else will add as object
                else
                {
                    objects.Add(new ObjectPointerNode
                    {
                        Name = firstIdentifier,
                        Line = (IsAtEnd() ? 0 : Current().Line + 1)
                    });
                }
                
                if (Current().TokenType != Tokens.Dot)
                {
                    Logger.Logger.Log($"CompilationProcess.End: Parsing primary (CallNode) {Current().TokenType}");
                    return new CallNode { Objects = objects, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
                }
                

                // While next is dot
                Logger.Logger.Log($"Start Process", "CallNodeWhile");
                while (Check(Tokens.Dot))
                {
                    Advance();
                    Logger.Logger.Log($"CallNodeWhile.current: " + Current().TokenType);
                    string identifier = Expect(Tokens.Identifier).Value;

                    // Current object is function
                    if (Current().TokenType == Tokens.LParen)
                    {
                        Advance();

                        arguments = [];
                        while (!Check(Tokens.RParen))
                        {
                            arguments.Add(ParseExpression());
                            if (Check(Tokens.Comma))
                                Advance();
                        }

                        Expect(Tokens.RParen);

                        Logger.Logger.Log($"Detected function: {identifier}", "CallNodeWhile");
                        Logger.Logger.Log("Arguments: [" + string.Join(", ", arguments) + "]");
                        objects.Add(new FunctionPointerNode
                        {
                            Name = identifier,
                            Arguments = arguments, 
                            Line = (IsAtEnd() ? 0 : Current().Line + 1)
                        });
                        continue;
                    }

                    Logger.Logger.Log($"Detected object: {identifier}", "CallNodeWhile");
                    objects.Add(new ObjectPointerNode
                    {
                        Name = identifier,
                        Line = (IsAtEnd() ? 0 : Current().Line + 1)
                    });
                } 
                
                Logger.Logger.Log("CompilationProcess.End: Parsing primary (CallNode, full)");
                return new CallNode { Objects = objects, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
            }
            

            if (Current().TokenType == Tokens.Equals && Peek()?.TokenType != Tokens.Equals)
            {
                Logger.Logger.Log($"CompilationProcess.End: Parsing primary (AssignmentNode)");
                Advance();
                return new AssignmentNode(false, false, false, false)
                {
                    VariableName = firstIdentifier,
                    Value = ParseExpression(),
                    Line = (IsAtEnd() ? 0 : Current().Line + 1)
                };
            }
            
            Logger.Logger.Log($"CompilationProcess.End: Parsing primary (VariableNode: {firstIdentifier})");
            return new VariableNode { Name = firstIdentifier, Line = (IsAtEnd() ? 0 : Current().Line + 1) };
        }
        
        throw new QlangCompileException($"Unexpected token in expression: {Current().TokenType} ({(Current().Value == "" ? "Null" : Current().Value)})", (IsAtEnd() ? 0 : Current().Line + 1), "Parser");
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

        if (token.TokenType is Tokens.Semicolon or Tokens.RBrace or Tokens.LBrace)
        {
            _line = "";
            return token;
        }
        
        Logger.Logger.Log(token.TokenType.ToString() + " " + token.Value, $"Token (Ln:{token.Line} Idx:{token.Index})");
        _line += $"{Token.TokenToString(token.TokenType)}{token.Value}";
        return token;
    }

    private Token Expect(Tokens type, string? value = null)
    {
        if (!Check(type))
        {
            var current = Current();
            throw new QlangCompileException($"""
                                 Expected {type}, got {current.TokenType} (Value: {(current.Value == "" ? "Null" : current.Value)})
                                        line: '{_line}'
                                 """, (IsAtEnd() ? 0 : Current().Line + 1), "Parser");
        }
        
        if (value != null && Current().Value != value)
            throw new QlangCompileException($"Expected '{value}', got '{Current().Value}'", (IsAtEnd() ? 0 : Current().Line + 1), "Parser");

        return Advance();
    }
}