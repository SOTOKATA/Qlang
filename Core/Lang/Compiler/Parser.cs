using Qlang.Core.Lang.AST;
using Qlang.Core.Lang.Dynamic.Exceptions;
using Qlang.Core.LangDebug;

namespace Qlang.Core.Lang.Compiler;

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

        Logger.SetLoggerPath(Path.Combine("Logs", "Debug", "debug_parser.log"));
        Logger.Warn("----------- Parser -----------");

        while (!IsAtEnd())
        {
            if (Check(Tokens.Semicolon))
            {
                Advance();
                continue;
            }

            program.Statements.Add(ParseStatement());
        }
        
        program = PostParser.IncludeExtends(program);

        Validator.CheckValidate(program);

        return program;
    }

    private ASTNode ParseStatement()
    {
        Logger.Log($"Parsing statement ({Current().TokenType}, {Current().Value})", "CompilationProcess");

        bool isStatic = false;
        bool isPrivate = false;

        if (Check(Tokens.Keyword) && Current().Value == Keywords.StaticModificator)
        {
            Logger.Warn("IsStatic = true");
            isStatic = true;
            Advance();
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.PrivateModificator)
        {
            Logger.Warn("IsPrivate = true");
            isPrivate = true;
            Advance();
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.BreakKeyword)
        {
            Advance();
            return new BreakNode
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ContinueKeyword)
        {
            Advance();
            return new ContinueNode
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
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
            return ParseVariableDeclaration(isStatic, isPrivate, false, true);

        if (Check(Tokens.Keyword) && Current().Value == Keywords.ConstVariableDeclaration)
            return ParseVariableDeclaration(isStatic, isPrivate, true, true);

        // method call statement (Object.method(...)) || Call method from class pointer
        if (Check(Tokens.Identifier) || (Check(Tokens.Keyword) && Current().Value == Keywords.ThisKeyword))
        {
            var expr = ParseExpression();
            Expect(Tokens.Semicolon);
            Logger.Log("Ended parsing statement (identifier)", "CompilationProcess");
            return expr;
        }

        throw new QlangCompileException($"Unexpected token: {Current().TokenType} '{Current().Value}'", (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);
    }

    private AssignmentNode ParseVariableDeclaration(bool isStatic = false, bool isPrivate = false, bool isConst = false, bool isNew = false)
    {
        Logger.Log("Parsing variable declaration", "CompilationProcess");
        if (!Check(Tokens.Keyword) ||
            (Current().Value != Keywords.VariableDeclaration &&
             Current().Value != Keywords.ConstVariableDeclaration))
            throw new QlangCompileException($"(ParseVariableDeclaration) Unexpected token: {Current().TokenType}", (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);

        Advance();

        var name = Expect(Tokens.Identifier).Value;

        ASTNode value = null;
        if (Current().TokenType == Tokens.Equals)
        {
            Advance();
            value = ParseExpression();
        }

        Logger.Log($"CompilationProcess.End: Parsing Variable declaration (Name: {name} Value: {value?.GetType().Name ?? "Null"})");
        return new AssignmentNode(isStatic, isPrivate, isConst, isNew)
        {
            VariableName = name,
            Value = value,
            Line = (IsAtEnd() ? 0 : Current().Line + 1),
            SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
        };
    }

    private FunctionNode ParseFunction(bool isStatic = false, bool isPrivate = false)
    {
        Logger.Log("CompilationProcess: Parsing function");

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
        
        List<ASTNode> body = ParseBlock();

        Logger.Log("CompilationProcess.End: Parsing function");
        return new FunctionNode
        {
            Name = name,
            Parameters = parameters,
            Body = body,
            IsStatic = isStatic,
            IsPrivate = isPrivate,
            Line = (IsAtEnd() ? 0 : Current().Line + 1),
            SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
        };
    }

    private ClassNode ParseClass()
    {
        Logger.Log("CompilationProcess: Parsing class");
        Expect(Tokens.Keyword, Keywords.ClassDeclaration);
        var name = Expect(Tokens.Identifier).Value;

        string extends = "";
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ExtendsKeyword &&
            Peek()?.TokenType == Tokens.Identifier)
        {
            Advance();
            extends = Current().Value;
            Advance();
        }

        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);
        
        if (!Check(Tokens.LBrace))
            throw new QlangCompileException("Class's body cannot be one-line", Current().Line, "Parser", Current().SourceFile);

        List<ASTNode> body = ParseBlock();

        Logger.Log("CompilationProcess.End: Parsing class");
        return new ClassNode { Name = name, Body = body, Extends = extends, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
    }

    private ForNode ParseFor()
    {
        Logger.Log("CompilationProcess: Parsing for");
        Expect(Tokens.Keyword, Keywords.ForBlock);
        
        AssignmentNode assignment = ParseVariableDeclaration(false, false, 
            (Check(Tokens.Keyword) && Current().Value == Keywords.ConstVariableDeclaration), 
            true);
        Expect(Tokens.Semicolon);
        var condition = ParseExpression();
        Expect(Tokens.Semicolon);
        var statement = ParseExpression();

        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);

        List<ASTNode> forBlock = ParseBlock();


        Logger.Log("CompilationProcess.End: Parsing for");
        return new ForNode { Assignment = assignment, Statement = statement, Condition = condition, Body = forBlock, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile), };
    }

    private WhileNode ParseWhile(bool isDoWhile = false)
    {
        Logger.Log("CompilationProcess: Parsing while");
        Expect(Tokens.Keyword, isDoWhile ? Keywords.DoWhileBlock : Keywords.WhileBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);

        List<ASTNode> whileBlock = ParseBlock();

        Logger.Log("CompilationProcess.End: Parsing while");
        return new WhileNode { Condition = condition, Body = whileBlock, IsDoWhile = isDoWhile, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
    }

    private IfNode ParseIf()
    {
        Logger.Log("CompilationProcess: Parsing if");
        Expect(Tokens.Keyword, Keywords.IfBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);

        List<ASTNode> thenBlock = ParseBlock();


        List<ASTNode> elseBlock = [];

        Logger.Warn(Current().TokenType.ToString());
        Logger.Warn(Current().Value);
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ElseBlock)
        {
            Advance();

            if (Check(Tokens.Keyword) && Current().Value == Keywords.IfBlock)
                elseBlock.Add(ParseIf());
            else
            {
                Expect(Tokens.Colon);
                // Expect(Tokens.Semicolon);
                elseBlock = ParseBlock();
            }
        }

        Logger.Log("CompilationProcess.End: Parsing if");
        return new IfNode { Condition = condition, ThenBlock = thenBlock, ElseBlock = elseBlock, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
    }

    private List<ASTNode> ParseBlock()
    {
        List<ASTNode> statements = [];
        Logger.Log("CompilationProcess: Parsing block");
        
        if (Check(Tokens.LBrace) && Peek()?.TokenType == Tokens.RBrace)
        {
            Expect(Tokens.LBrace);
            Expect(Tokens.RBrace);
            return [];
        }
        
        // One line block
        // {
        if (!Check(Tokens.LBrace))
            statements.Add(ParseStatement());
        else
        {
            Expect(Tokens.LBrace);
            while (!Check(Tokens.RBrace) && !IsAtEnd())
            {
                if (Check(Tokens.Semicolon))
                {
                    Advance();
                    continue;
                }

                statements.Add(ParseStatement());
            }
            Expect(Tokens.RBrace);
        }

        Logger.Log("CompilationProcess.End: Parsing block");
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
        Logger.Log("CompilationProcess: Parsing return");
        Expect(Tokens.Keyword);

        ASTNode? node = null;
        if (!Check(Tokens.Semicolon))
            node = ParseExpression();

        Expect(Tokens.Semicolon);

        Logger.Log("CompilationProcess.End: Parsing return");
        return new ReturnNode { ReturnValue = node, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
    }

    /// <summary>
    /// Parsing expression (ex.: 5 + 5)
    /// Приоритет (от низкого к высокому): ||, &&, ==, !=, <, >, <=, >=, +, -, *, /, %
    /// </summary>
    private ASTNode ParseExpression()
    {
        Logger.Log("CompilationProcess: Parsing expression");
        var result = ParseLogicalOr();
        Logger.Log("CompilationProcess.End: Parsing expression");
        return result;
    }

    /// <summary>
    /// Parsing logical '||'
    /// </summary>
    private ASTNode ParseLogicalOr()
    {
        var left = ParseLogicalAnd();

        while (Check(Tokens.Or) && Peek()?.TokenType == Tokens.Or)
        {
            Advance(); // первый |
            Advance(); // второй |
            var right = ParseLogicalAnd();
            left = new BinaryOperationNode
            {
                Left = left,
                Operator = "||",
                Right = right,
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        return left;
    }

    /// <summary>
    /// Parsing logical '&&'
    /// </summary>
    private ASTNode ParseLogicalAnd()
    {
        var left = ParseEquality();

        while (Check(Tokens.And) && Peek()?.TokenType == Tokens.And)
        {
            Advance(); // первый &
            Advance(); // второй &
            var right = ParseEquality();
            left = new BinaryOperationNode
            {
                Left = left,
                Operator = "&&",
                Right = right,
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        return left;
    }

    /// <summary>
    /// Parsing equality ('==', '!=')
    /// </summary>
    private ASTNode ParseEquality()
    {
        var left = ParseComparison();

        while (true)
        {
            if (Check(Tokens.Equals) && Peek()?.TokenType == Tokens.Equals)
            {
                Advance(); // первый =
                Advance(); // второй =
                var right = ParseComparison();
                left = new BinaryOperationNode
                {
                    Left = left,
                    Operator = "==",
                    Right = right,
                    Line = (IsAtEnd() ? 0 : Current().Line + 1),
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                };
            }
            else if (Check(Tokens.Not) && Peek()?.TokenType == Tokens.Equals)
            {
                Advance(); // !
                Advance(); // =
                var right = ParseComparison();
                left = new BinaryOperationNode
                {
                    Left = left,
                    Operator = "!=",
                    Right = right,
                    Line = (IsAtEnd() ? 0 : Current().Line + 1),
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                };
            }
            else
            {
                break;
            }
        }

        return left;
    }

    /// <summary>
    /// Parsing comparison ('<', '>', '<=', '>=')
    /// </summary>
    private ASTNode ParseComparison()
    {
        var left = ParseAddition();

        while (true)
        {
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

                var right = ParseAddition();
                left = new BinaryOperationNode
                {
                    Left = left,
                    Operator = op,
                    Right = right,
                    Line = (IsAtEnd() ? 0 : Current().Line + 1),
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                };
            }
            else if (Check(Tokens.Greater))
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

                var right = ParseAddition();
                left = new BinaryOperationNode
                {
                    Left = left,
                    Operator = op,
                    Right = right,
                    Line = (IsAtEnd() ? 0 : Current().Line + 1),
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                };
            }
            else
            {
                break;
            }
        }

        return left;
    }

    /// <summary>
    /// Parsing addition and subtraction ('+', '-')
    /// </summary>
    private ASTNode ParseAddition()
    {
        var left = ParseMultiplication();

        while (Check(Tokens.Plus) || Check(Tokens.Minus))
        {
            var op = Current().TokenType.ToString();
            Advance();
            var right = ParseMultiplication();
            left = new BinaryOperationNode
            {
                Left = left,
                Operator = op,
                Right = right,
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        return left;
    }

    /// <summary>
    /// Parsing division, multiplication, percent ('*', '/', '%')
    /// </summary>
    private ASTNode ParseMultiplication()
    {
        var left = ParsePrimary();

        while (Check(Tokens.Star) || Check(Tokens.Slash) || Check(Tokens.Percent))
        {
            var op = Current().TokenType.ToString();
            Advance();
            var right = ParsePrimary();
            left = new BinaryOperationNode
            {
                Left = left,
                Operator = op,
                Right = right,
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        return left;
    }

    /// <summary>
    /// Parsing primitive calls and function calls
    /// </summary>
    private ASTNode ParsePrimary()
    {
        Logger.Log("CompilationProcess: Parsing primary");
    
        // Function pointer
        if (Check(Tokens.Keyword) && Current().Value == Keywords.FunctionDeclaration && 
            Peek()?.TokenType == Tokens.LParen)
        {
            Advance();
            Advance();
            var func = new FunctionNode
            {
                Name = "___function_pointer___",
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
            };
        
            List<AssignmentNode> parameters = [];
            while (!Check(Tokens.RParen))
            {
                if (Check(Tokens.Keyword))
                    parameters.Add(ParseVariableDeclaration());
                if (Check(Tokens.Comma))
                    Advance();
            }
            Advance();
            
            func.Parameters = parameters;

            Expect(Tokens.Equals);
            Expect(Tokens.Greater);

            func.Body = ParseBlock();

            return func;
        }
        
        // Object {}
        if (Check(Tokens.LBrace))
        {
            Advance();
            var @class = new ClassNode
            {
                Name = "___object___",
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
                Body = []
            };
            
            while (!Check(Tokens.RBrace))
            {
                if (Check(Tokens.Keyword))
                    @class.Body.Add(ParseVariableDeclaration());
                if (Check(Tokens.Comma))
                    Advance();
            }

            Expect(Tokens.RBrace);
            Expect(Tokens.Semicolon);

            return @class;
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.NullKeyword)
        {
            Advance();
            return new NullNode
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.BreakKeyword)
        {
            Advance();
            return new BreakNode
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ContinueKeyword)
        {
            Advance();
            return new ContinueNode
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
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

            return new CollectionNode { Collection = statements, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
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
            Logger.Warn("Parsing parents");
            return ParseParens();
        }

        // bool return
        if (Check(Tokens.Keyword) && (Current().Value == Keywords.FalseKeyword || Current().Value == Keywords.TrueKeyword))
        {
            Logger.Log("CompilationProcess.End: Parsing primary");
            return new BooleanNode
            {
                Value = isMinus ? !bool.Parse(Advance().Value) : bool.Parse(Advance().Value),
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        // String reference
        if (Check(Tokens.StringRef))
        {
            Logger.Log("CompilationProcess.End: Parsing primary (StringRef)");
            return new StringRefNode { Index = int.Parse(Advance().Value), Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
        }

        if (Check(Tokens.NumberRef))
        {
            Logger.Log("CompilationProcess.End: Parsing primary (NumberRef)");
            return new NumberRefNode
            {
                IsNegative = isMinus,
                Index = int.Parse(Advance().Value)
                ,
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        // Identifier - может быть вызовом метода или функции
        if (Check(Tokens.Identifier) || (Check(Tokens.Keyword) && Current().Value == Keywords.ThisKeyword))
        {
            var firstIdentifier = Advance().Value;

            // Проверяем на специальные строковые константы
            if (firstIdentifier.StartsWith("___STRING_"))
            {
                var index = int.Parse(firstIdentifier.Replace("___STRING_", "").Replace("___", ""));
                return new StringRefNode { Index = index, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
            }

            if (firstIdentifier.StartsWith("___NUMBER_"))
            {
                var index = int.Parse(firstIdentifier.Replace("___NUMBER_", "").Replace("___", ""));
                return new NumberRefNode
                {
                    IsNegative = isMinus,
                    Index = index,
                    Line = (IsAtEnd() ? 0 : Current().Line + 1),
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                };
            }

            if (firstIdentifier.TryParseNumber(out var result))
                return new NumberNode { Value = result, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };

            // Вызов метода: Object.method(...)... or func().class.etc();
            if (Check(Tokens.Dot) || Check(Tokens.LParen))
            {
                Logger.Log("Detected object/function call or assignment");
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

                    Logger.Log("Arguments: " + string.Join(", ", arguments));

                    objects.Add(new FunctionPointerNode
                    {
                        Name = firstIdentifier,
                        Arguments = arguments,
                        Line = (IsAtEnd() ? 0 : Current().Line + 1),
                        SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                    });
                }
                // Else will add as object
                else
                {
                    objects.Add(new ObjectPointerNode
                    {
                        Name = firstIdentifier,
                        Line = (IsAtEnd() ? 0 : Current().Line + 1),
                        SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                    });
                }

                // Check if this is an assignment after the first object/function
                if (Current().TokenType == Tokens.Equals && Peek()?.TokenType != Tokens.Equals)
                {
                    Logger.Log($"CompilationProcess.End: Parsing primary (PathAssignmentNode - single)");
                    Advance(); // consume '='
                    return new AssignmentNode(false, false, false, false)
                    {
                        Path = objects,
                        Value = ParseExpression(),
                        Line = (IsAtEnd() ? 0 : Current().Line + 1),
                        SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                    };
                }

                if (Current().TokenType != Tokens.Dot)
                {
                    Logger.Log($"CompilationProcess.End: Parsing primary (CallNode) {Current().TokenType}");
                    return new CallNode { Objects = objects, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
                }


                // While next is dot
                Logger.Log($"Start Process", "CallNodeWhile");
                while (Check(Tokens.Dot))
                {
                    Advance();
                    Logger.Log($"CallNodeWhile.current: " + Current().TokenType);
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

                        Logger.Log($"Detected function: {identifier}", "CallNodeWhile");
                        Logger.Log("Arguments: [" + string.Join(", ", arguments) + "]");
                        objects.Add(new FunctionPointerNode
                        {
                            Name = identifier,
                            Arguments = arguments,
                            Line = (IsAtEnd() ? 0 : Current().Line + 1),
                            SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                        });
                    }
                    else
                    {
                        Logger.Log($"Detected object: {identifier}", "CallNodeWhile");
                        objects.Add(new ObjectPointerNode
                        {
                            Name = identifier,
                            Line = (IsAtEnd() ? 0 : Current().Line + 1),
                            SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                        });
                    }

                    // Check if this is an assignment after building the full path
                    if (Current().TokenType == Tokens.Equals && Peek()?.TokenType != Tokens.Equals)
                    {
                        Logger.Log($"CompilationProcess.End: Parsing primary (PathAssignmentNode - full path)");
                        Advance(); // consume '='
                        return new AssignmentNode(false, false, false, false)
                        {
                            Path = objects,
                            Value = ParseExpression(),
                            Line = (IsAtEnd() ? 0 : Current().Line + 1),
                            SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                        };
                    }
                }

                Logger.Log("CompilationProcess.End: Parsing primary (CallNode, full)");

                return new CallNode { Objects = objects, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
            }


            if (Current().TokenType == Tokens.Equals && Peek()?.TokenType != Tokens.Equals)
            {
                Logger.Log($"CompilationProcess.End: Parsing primary (AssignmentNode)");
                Advance();
                return new AssignmentNode(false, false, false, false)
                {
                    VariableName = firstIdentifier,
                    Value = ParseExpression(),
                    Line = (IsAtEnd() ? 0 : Current().Line + 1),
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
                };
            }

            Logger.Log($"CompilationProcess.End: Parsing primary (VariableNode: {firstIdentifier})");
            return new VariableNode { Name = firstIdentifier, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
        }

        throw new QlangCompileException($"Unexpected token in expression: {Current().TokenType} ({(Current().Value == "" ? "Null" : Current().Value)})", (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);
    }

    // Support methods
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

        Logger.Log(token.TokenType.ToString() + " " + token.Value, $"Token (Ln:{token.Line} Idx:{token.Index})");
        _line += $"{Token.TokenToString(token.TokenType)}{token.Value}";
        return token;
    }

    private Token Expect(Tokens type, string? value = null)
    {
        if (!Check(type))
        {
            var current = Current();
            // throw new Exception(":");
            throw new QlangCompileException($"""
                                 Expected {type}, got {current.TokenType} (Value: {(current.Value == "" ? "Null" : current.Value)})
                                        line: '{_line}'
                                 """, (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);
        }

        if (value != null && Current().Value != value)
            throw new QlangCompileException($"Expected '{value}', got '{Current().Value}'", (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);

        return Advance();
    }
}
