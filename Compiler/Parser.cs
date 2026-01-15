using Core;
using Core.AST;
using Core.Debug;
using Core.Exceptions;

namespace Compiler;

public class Parser
{
    private List<Token> _tokens = [];
    private int _position;

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

        var isStatic = false;
        var isPrivate = false;

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
        
        // using declaration
        if (Check(Tokens.Keyword) && Current().Value == Keywords.UsingKeyword)
            return ParseUsing();
        
        // namespace declaration
        if (Check(Tokens.Keyword) && Current().Value == Keywords.NamespaceDeclaration)
            return ParseNamespace(isPrivate);

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
        
        // switch statement
        if (Check(Tokens.Keyword) && Current().Value == Keywords.SwitchBlock)
            return ParseSwitch();

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
            return ParseVariableDeclaration(false, isStatic, isPrivate, false, true);

        if (Check(Tokens.Keyword) && Current().Value == Keywords.ConstVariableDeclaration)
            return ParseVariableDeclaration(false, isStatic, isPrivate, true, true);

        // method call statement (Object.method(...)) || Call method from class pointer
        if (Check(Tokens.Identifier) || (Check(Tokens.Keyword) && Current().Value == Keywords.ThisKeyword))
        {
            var expr = ParseExpression();
            Expect(Tokens.Semicolon);
            Logger.Log("Ended parsing statement (identifier)", "CompilationProcess");
            return expr;
        }
        
        if (Check(Tokens.LParen))
        {
            var expr = ParseExpression();
            Expect(Tokens.Semicolon);
            Logger.Log("Ended parsing statement (LParen)", "CompilationProcess");
            return expr;
        }
        
        if (Check(Tokens.LSquareParen))
        {
            var expr = ParseExpression();
            Expect(Tokens.Semicolon);
            Logger.Log("Ended parsing statement (LSquareParen)", "CompilationProcess");
            return expr;
        }

        throw new QlangCompileException($"Unexpected token: {Current().TokenType} '{Current().Value}'", (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);
    }

    private AssignmentNode ParseVariableDeclaration(bool canUseType, bool isStatic = false, bool isPrivate = false, bool isConst = false, bool isNew = false)
    {
        Logger.Log("Parsing variable declaration", "CompilationProcess");
        if (!Check(Tokens.Keyword) ||
            (Current().Value != Keywords.VariableDeclaration &&
             Current().Value != Keywords.ConstVariableDeclaration))
            throw new QlangCompileException($"(ParseVariableDeclaration) Unexpected token: {Current().TokenType}", (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);

        Advance();

        CallNode? type = null;
        if (Check(Tokens.Less))
        {
            if (!canUseType)
            {
                var token = Current();
                throw new QlangCompileException(
                    "Using variables with types is only possible with function arguments", (token.Line + 1), "Parser", token.SourceFile);
            }
            Advance();
            var returnValue  = ParsePrimaryPath();

            if (returnValue is not CallNode node)
            {
                var token = Current();
                throw new QlangCompileException("Cannot use follow node as path to class", (token.Line + 1), "Parser", token.SourceFile);
            }

            type = node;
            Expect(Tokens.Greater);
        }

        var name = Expect(Tokens.Identifier).Value;

        ASTNode? value = null;
        if (Current().TokenType == Tokens.Equals)
        {
            Advance();
            value = ParseExpression();
        }

        Logger.Log($"CompilationProcess.End: Parsing Variable declaration (Name: {name} Value: {value?.GetType().Name ?? "<null>"})");
        return new AssignmentNode(isStatic, isPrivate, isConst, isNew)
        {
            VariableName = name,
            Value = value,
            Type = type,
            Line = (IsAtEnd() ? 0 : Current().Line + 1),
            SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
        };
    }

    private UsingNode ParseUsing()
    {
        // Skip 'using'
        Expect(Tokens.Keyword, Keywords.UsingKeyword);

        var node = ParsePrimary();

        if (node is not CallNode callNode)
            throw new QlangCompileException("Using must be path to namespace.", node.Line, "Parser", node.SourceFile ?? "undefined");

        var newObjects = new List<NamespacePointerNode>();

        foreach (var o in callNode.Objects)
        {
            switch (o)
            {
                case ObjectPointerNode pointer:
                    newObjects.Add(new NamespacePointerNode(pointer.Name!)
                    {
                        SourceFile = pointer.SourceFile,
                        Line = pointer.Line,
                    });
                    break;
                case NamespacePointerNode namespacePointerNode:
                    newObjects.Add(namespacePointerNode);
                    break;
                default:
                    throw new QlangCompileException("Using objects in the path that are not namespaces is not allowed.", o.Line, "Parser", o.SourceFile ?? "undefined");
            }
        }

        callNode.Objects = newObjects.Cast<ASTNode>().ToList();
        Expect(Tokens.Semicolon);
        return new UsingNode
        {
            CallPath = callNode,
            SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
            Line = (IsAtEnd() ? 0 : Current().Line + 1)
        };
    }

    private NamespaceNode ParseNamespace(bool isPrivate)
    {
        Logger.Log("CompilationProcess: Parsing namespace");

        // skip 'namespace'
        Expect(Tokens.Keyword, Keywords.NamespaceDeclaration);

        var name = Expect(Tokens.Identifier).Value;

        Expect(Tokens.Colon);
        
        if (!Check(Tokens.LBrace))
            throw new QlangCompileException("Namespace's body cannot be one-line", Current().Line, "Parser", Current().SourceFile);
        
        var body = ParseBlock();
        
        return new NamespaceNode
        {
            Name = name,
            Body = body,
            IsPrivate = isPrivate,
            
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
                parameters.Add(ParseVariableDeclaration(true));
            if (Check(Tokens.Comma))
                Advance();
        }

        Expect(Tokens.RParen);
        Expect(Tokens.Colon);

        var body = ParseBlock();

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

        var extends = "";
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

        var body = ParseBlock();

        Logger.Log("CompilationProcess.End: Parsing class");
        return new ClassNode { Name = name, Body = body, Extends = extends, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
    }

    private ForNode ParseFor()
    {
        Logger.Log("CompilationProcess: Parsing for");
        Expect(Tokens.Keyword, Keywords.ForBlock);

        var assignment = ParseVariableDeclaration(false, false, false,
            (Check(Tokens.Keyword) && Current().Value == Keywords.ConstVariableDeclaration),
            true);
        Expect(Tokens.Semicolon);
        var condition = ParseExpression();
        Expect(Tokens.Semicolon);
        var statement = ParseExpression();

        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);

        var forBlock = ParseBlock();


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

        var whileBlock = ParseBlock();

        Logger.Log("CompilationProcess.End: Parsing while");
        return new WhileNode { Condition = condition, Body = whileBlock, IsDoWhile = isDoWhile, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
    }

    private SwitchNode ParseSwitch()
    {
        Logger.Log("CompilationProcess: Parsing switch");
        Expect(Tokens.Keyword, Keywords.SwitchBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        Expect(Tokens.LBrace);

        var @switch = new SwitchNode { Condition = condition };
        
        while (Check(Tokens.Keyword) && Current().Value == Keywords.CaseKeyword)
        {
            Expect(Tokens.Keyword);
            condition = ParseExpression();
            Expect(Tokens.Colon);
            var block = ParseBlock();

            @switch.CaseBlocks.Add(new SwitchCaseNode
            {
                CaseBlock = block,
                Condition = condition,
                SourceFile =  (IsAtEnd() ? "" : Current().SourceFile),
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
            });
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.DefaultKeyword)
        {
            Expect(Tokens.Keyword);
            Expect(Tokens.Colon);
            @switch.DefaultBlock = ParseBlock();
        }
        Expect(Tokens.RBrace);
        
        return @switch;
    }

    private IfNode ParseIf()
    {
        Logger.Log("CompilationProcess: Parsing if");
        Expect(Tokens.Keyword, Keywords.IfBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);

        var thenBlock = ParseBlock();


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

    private ASTNode? ParsePrimaryKeywords(bool isMinus)
    {
        if (Check(Tokens.Keyword) && Current().Value == Keywords.NullKeyword)
        {
            // Advance 'null'
            Advance();
            return new NullNode
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.BreakKeyword)
        {
            // Advance 'break'
            Advance();
            return new BreakNode
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ContinueKeyword)
        {
            // Advance 'continue'
            Advance();
            return new ContinueNode
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        // bool return
        if (Check(Tokens.Keyword) && (Current().Value == Keywords.FalseKeyword || Current().Value == Keywords.TrueKeyword))
        {
            Logger.Log("CompilationProcess.End: Parsing primary");
            // Advance 'true' or 'false'
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
            // Advance '___STRING_*___'
            return new StringRefNode { Index = int.Parse(Advance().Value), Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
        }

        if (Check(Tokens.NumberRef))
        {
            Logger.Log("CompilationProcess.End: Parsing primary (NumberRef)");
            // Advance '___NUMBER_*___'
            return new NumberRefNode
            {
                IsNegative = isMinus,
                Index = int.Parse(Advance().Value),
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        return null;
    }

    private ASTNode? ParsePrimaryPointers()
    {
        // Function pointer
        if (Check(Tokens.Keyword) && Current().Value == Keywords.FunctionDeclaration &&
            Peek()?.TokenType == Tokens.LParen)
        {
            // Advance 'function'
            Advance();
            // Advance '('
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
                    parameters.Add(ParseVariableDeclaration(false));
                if (Check(Tokens.Comma))
                    Advance();
            }
            // Advance ')'
            Advance();

            func.Parameters = parameters;

            // Advance '=>'
            Expect(Tokens.Equals);
            Expect(Tokens.Greater);

            func.Body = ParseBlock();

            return func;
        }

        // Object {}
        if (Check(Tokens.LBrace))
        {
            // Advance '{'
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
                    @class.Body.Add(ParseVariableDeclaration(false));
                if (Check(Tokens.Comma))
                    Advance();
            }

            // Advance '};'
            Expect(Tokens.RBrace);
            Expect(Tokens.Semicolon);

            return @class;
        }

        return null;
    }

    private ASTNode? ParsePrimaryParens()
    {
        // Array
        if (Check(Tokens.LSquareParen))
        {
            // Advance '['
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

            // Advance ']'
            Advance();

            return new CollectionNode { Collection = statements, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
        }

        // parse: '('expression')'
        if (Check(Tokens.LParen))
        {
            Logger.Warn("Parsing parents");
            // Advance '('
            Expect(Tokens.LParen);
            var parsed = ParseExpression();
            // Advance ')'
            Expect(Tokens.RParen);

            return parsed;
        }

        return null;
    }

    private ASTNode? ParsePrimaryExtracted(bool isMinus)
    {
        if (Current().TokenType != Tokens.Identifier)
            return null;

        var currentIdentifier = Current().Value;

        // Проверяем на специальные строковые константы
        if (currentIdentifier.StartsWith("___STRING_"))
        {
            // Advance '___STRING_*___'
            Advance();
            var index = int.Parse(currentIdentifier.Replace("___STRING_", "").Replace("___", ""));
            return new StringRefNode { Index = index, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
        }

        if (currentIdentifier.StartsWith("___NUMBER_"))
        {
            // Advance '___NUMBER_*___'
            Advance();
            var index = int.Parse(currentIdentifier.Replace("___NUMBER_", "").Replace("___", ""));
            return new NumberRefNode
            {
                IsNegative = isMinus,
                Index = index,
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        if (currentIdentifier.TryParseNumber(out var res))
        {
            // Advance '1'
            Advance();
            return new NumberNode { Value = res, Line = (IsAtEnd() ? 0 : Current().Line + 1), SourceFile = (IsAtEnd() ? "" : Current().SourceFile) };
        }

        return null;
    }

    private ASTNode? ParsePrimaryCall()
    {
        ASTNode? current = null;

        if (Current().TokenType == Tokens.Keyword && Current().Value == Keywords.ThisKeyword)
        {
            current = new ObjectPointerNode
            {
                Name = Keywords.ThisKeyword,
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
            };
            // Advance 'this'
            Expect(Tokens.Keyword);
        }

        if (Current().TokenType != Tokens.Identifier)
            return current;

        var identifier = Current().Value;
        // Advance 'identifier'
        Advance();

        if (Check(Tokens.LParen))
        {
            // Advance '('
            Advance();
            var args = new List<ASTNode>();
            while (!Check(Tokens.RParen))
            {
                args.Add(ParseExpression());
                if (Check(Tokens.Comma))
                    Advance();
            }
            // Advance ')'
            Advance();
            
            current = new FunctionPointerNode
            {
                Name = identifier,
                Arguments = args,
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
            };
        }

        if (current is null)
            current = new ObjectPointerNode
            {
                Name = identifier,
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
            };

        // Parse assign path 
        if (Check(Tokens.Equals) && Peek()?.TokenType != Tokens.Equals)
        {
            Logger.Log($"CompilationProcess.End: Parsing primary (PathAssignmentNode - single)");
            // Advance '='
            Advance();
            current = new AssignmentNode(false, false, false, false)
            {
                VariableName = identifier,
                Value = ParseExpression(),
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };
        }

        if (Check(Tokens.Colon) && Peek()?.TokenType == Tokens.Colon)
            current = new NamespacePointerNode(identifier)
            {
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile)
            };

        return current;
    }

    private ASTNode ParsePrimaryPath()
    {
        var astNode = ParsePrimary(true);

        // Is not path (NOT 'call.node.func().etc();', JUST LIKE 'call' or 'call()' etc.)
        var isDot = Check(Tokens.Dot);
        var isDoubleColon = Check(Tokens.Colon) && Peek()?.TokenType == Tokens.Colon;
        
        if (!isDot && !isDoubleColon)
        {
            return astNode switch
            {
                ObjectPointerNode obj => new CallNode
                {
                    Objects = [obj],
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
                    Line = (IsAtEnd() ? 0 : Current().Line + 1)
                },
                NamespacePointerNode namespacePointer => new CallNode()
                {
                    Objects = [namespacePointer],
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
                    Line = (IsAtEnd() ? 0 : Current().Line + 1)
                },
                FunctionPointerNode pointerFunction => new CallNode
                {
                    Objects = [pointerFunction],
                    Arguments = pointerFunction.Arguments,
                    SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
                    Line = (IsAtEnd() ? 0 : Current().Line + 1),
                },
                _ => astNode
            };
        }

        List<ASTNode> path = [];
        
        if (astNode is CallNode callNode)
            path.AddRange(callNode.Objects);
        else path.Add(astNode);
        
        do
        {
            if (Current().TokenType == Tokens.Colon && Peek()?.TokenType == Tokens.Colon)
                Advance();
            
            Advance();
            
            astNode = ParsePrimary(true);

            if (astNode is CallNode node)
                path.AddRange(node.Objects);
            else path.Add(astNode);
            
            // Do circle while '.' or '::'
        } while (Check(Tokens.Dot) || (Check(Tokens.Colon) && Peek()?.TokenType == Tokens.Colon));

        if (path[^1] is AssignmentNode assignmentNode)
        {
            path[^1] = new ObjectPointerNode { Name = assignmentNode.VariableName };
            return new AssignmentNode(false, false, false, false)
            {
                Path = path,
                SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
                Line = (IsAtEnd() ? 0 : Current().Line + 1),
                Value = assignmentNode.Value
            };
        }

        var args = path[^1] is FunctionPointerNode ptr ? ptr.Arguments : [];
        
        return new CallNode
        {
            Objects =  path,
            Arguments = args,
            SourceFile = (IsAtEnd() ? "" : Current().SourceFile),
            Line = (IsAtEnd() ? 0 : Current().Line + 1),
        };
    }

    /// <summary>
    /// Parsing primitive calls and function calls
    /// </summary>
    private ASTNode ParsePrimary(bool isLoop = false)
    {
        Logger.Log("CompilationProcess: Parsing primary");

        var isMinus = false;
        if (Check(Tokens.Minus))
        {
            isMinus = true;
            Advance();
        }

        ASTNode? baseExpression;

        if (!isLoop)
        {
            baseExpression = ParsePrimaryPath();
            return baseExpression;
        }

        baseExpression = ParsePrimaryPointers();
        if (baseExpression is not null)
            return baseExpression;

        // Try keywords (null, true, false, break, continue, string/number refs)
        baseExpression = ParsePrimaryKeywords(isMinus);
        if (baseExpression is not null)
            return baseExpression;

        // Try extracted identifiers (___STRING_*___, ___NUMBER_*___, numbers)
        baseExpression = ParsePrimaryExtracted(isMinus);
        if (baseExpression is not null)
            return baseExpression;

        baseExpression = ParsePrimaryCall();
        if (baseExpression is not null)
            return baseExpression;
        
        // Try parentheses and arrays
        baseExpression = ParsePrimaryParens();
        if (baseExpression is not null)
            return baseExpression;
        
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

        Logger.Log(token.TokenType.ToString() + " " + token.Value, $"Token (Ln:{token.Line})");
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
                                 """, (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);
        }

        if (value != null && Current().Value != value)
            throw new QlangCompileException($"Expected '{value}', got '{Current().Value}'", (IsAtEnd() ? 0 : Current().Line + 1), "Parser", Current().SourceFile);

        return Advance();
    }
}
