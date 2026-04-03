using Core;
using Core.AST;
using Core.Dynamic;
using Core.Exceptions;
using Core.Tables;

namespace Compiler;

/*
 * Second stage of compilation
 * Translated code from the lexer is converted into C# classes
 */
public class Parser(SourceFileTable? sourceFileTable, DebugTable? debugTable, StringPoolTable stringPoolTable)
{
    private List<Token> _tokens = [];
    private int _position;

    // PostParser
    private readonly List<CallNode> _callNodes = [];
    private readonly List<AssignmentNode> _assignmentNodes = [];
    
    public (ProgramNode programNode, StringPoolTable stringPoolTable) Parse(List<Token> tokens)
    {
        _tokens = tokens;
        _position = 0;

        ProgramNode program = new();

        
        while (!IsAtEnd())
        {
            if (Check(Tokens.Semicolon))
            {
                Advance();
                continue;
            }

            program.Statements.Add(ParseStatement());
        }
        
        var postParser = new PostParser(sourceFileTable, debugTable, stringPoolTable);
        program = postParser.CreateGlobalNamespace(program);
        postParser.MergeNamespaces(program.Statements);
        program = postParser.IncludeUsings(program, _callNodes, _assignmentNodes);
        program = postParser.IncludeExtends(program);
        
        new Optimizer().Optimize(program);

        new Validator(sourceFileTable, debugTable, stringPoolTable).CheckValidate(program);
        
        return (program, stringPoolTable);
    }

    private ASTNode ParseStatement(bool isOneLineReturn = false)
    {
        var lineNode = new LineNode(IsAtEnd() ? -1 : Current().DebugIndex);
        
        if (isOneLineReturn && Check(Tokens.Equals) && Peek()?.TokenType == Tokens.Greater)
            return ParseReturn(true);

        var isPrivate = false;

        if (Check(Tokens.Keyword, Keywords.PrivateModificator))
        {
            isPrivate = true;
            Advance();
        }
        
        var isAsync = false;

        if (Check(Tokens.Keyword, Keywords.AsyncKeyword))
        {
            isAsync = true;
            Advance();
        }

        if (Check(Tokens.Keyword, Keywords.BreakKeyword))
        {
            lineNode.Content =  new KeywordNode
            {
               KeywordId = stringPoolTable.Add(Keywords.BreakKeyword)
            };
            Expect(Tokens.Keyword);
            Expect(Tokens.Semicolon);
            return lineNode;
        }
        if (Check(Tokens.Keyword, Keywords.ContinueKeyword))
        {
            lineNode.Content =  new KeywordNode
            {
                KeywordId = stringPoolTable.Add(Keywords.ContinueKeyword)
            };
            Expect(Tokens.Keyword);
            Expect(Tokens.Semicolon);
            return lineNode;
        }
        
        // using declaration
        if (Check(Tokens.Keyword, Keywords.UsingKeyword))
            return ParseUsing();
        
        // namespace declaration
        if (Check(Tokens.Keyword, Keywords.NamespaceDeclaration))
            return ParseNamespace(isPrivate);

        // function declaration
        if (Check(Tokens.Keyword, Keywords.FunctionDeclaration))
            return ParseFunction(isPrivate, isAsync);

        // class declaration
        if (Check(Tokens.Keyword, Keywords.ClassDeclaration))
            return ParseClass(isPrivate);

        // for statement
        if (Check(Tokens.Keyword, Keywords.ForBlock))
            return ParseFor();

        // if statement
        if (Check(Tokens.Keyword, Keywords.IfBlock))
            return ParseIf();
        
        // switch statement
        if (Check(Tokens.Keyword, Keywords.SwitchBlock))
            return ParseSwitch();

        // while statement
        if (Check(Tokens.Keyword, Keywords.WhileBlock))
            return ParseWhile();

        // do-while statement
        if (Check(Tokens.Keyword, Keywords.DoWhileBlock))
            return ParseWhile(true);
        
        // try-catch statement
        if (Check(Tokens.Keyword, Keywords.TryKeyword))
            return ParseTryCatch();

        // return statement
        if (Check(Tokens.Keyword, Keywords.ReturnKeyword))
            return ParseReturn();

        if (Check(Tokens.Keyword, Keywords.ParallelKeyword))
        {
            lineNode.Content = ParseParallel();
            return lineNode;
        }
        
        // new statement
        if (Check(Tokens.Keyword, Keywords.CreateClassInstanceKeyword))
        {
            lineNode.Content = ParsePrimary();
            return lineNode;
        }

        // assignment
        if (Check(Tokens.Keyword, Keywords.VariableDeclaration) || Check(Tokens.Keyword, Keywords.ConstVariableDeclaration))
        {
            var var = ParseVariableDeclaration(isPrivate: isPrivate, isConst: Check(Tokens.Keyword, Keywords.ConstVariableDeclaration), isNew: true);
            Expect(Tokens.Semicolon);
            lineNode.Content = var;
            return lineNode;
        }
        
        // method call statement (Object.method(...)) || Call method from class pointer || [] || ()
        if (Check(Tokens.Identifier) || (Check(Tokens.Keyword, Keywords.ThisKeyword)) || Check(Tokens.LParen) || Check(Tokens.LSquareParen) || Check(Tokens.Keyword, Keywords.AsyncKeyword))
        {
            var expr = ParseExpression();
            Expect(Tokens.Semicolon);
            lineNode.Content = expr;
            return lineNode;
        }

        ThrowIfIsEnd();

        throw new QlangCompileException($"Undefined statement: {Token.TokenToString(Current())}", GetDebug(Current()), "Parser");
    }

    private ShortHandIfNode? ParseShortHandIf()
    {
        if (!Check(Tokens.Keyword, Keywords.IfBlock))
            return null;
        
        Expect(Tokens.Keyword, Keywords.IfBlock);

        var condition = ParseExpression();

        Expect(Tokens.Question);

        var isNullable = Check(Tokens.Question);
        if (isNullable) 
            Advance();

        var then = ParseExpression();

        if (!isNullable)
            Expect(Tokens.Colon);

        var @else = isNullable ? condition : ParseExpression();

        if (isNullable)
            condition = new BinaryOperationNode
            {
                Left = condition,
                Right = new KeywordNode { KeywordId = stringPoolTable.Add(Keywords.NullKeyword) },
                OperatorId = stringPoolTable.Add("==")
            };
        
        return new ShortHandIfNode
        {
            Condition = condition,
            Then = then,
            Else = @else
        };
    }
    
    private ShortHandSwitchNode? ParseShortHandSwitch()
    {
        if (!Check(Tokens.Keyword, Keywords.SwitchBlock))
            return null;
        
        var debugToken = Expect(Tokens.Keyword, Keywords.SwitchBlock);
        
        var value = ParseExpression();

        ASTNode? defaultCase = null;

        Expect(Tokens.Colon);
        Expect(Tokens.LBrace);

        Dictionary<ShortHandSwitchCase, ASTNode> cases = [];
        while (!Check(Tokens.RBrace))
        {
            var isDefault = Check(Tokens.Keyword, Keywords.ElseBlock) || Check(Tokens.Keyword, Keywords.DefaultKeyword);

            if (isDefault)
                Advance();

            var current = Current();
            var next = Peek();

            var operatorId = (current.TokenType, next?.TokenType) switch
            {
                (Tokens.Equals, Tokens.Equals) => Consume("==", 2),
                (Tokens.Not, Tokens.Equals)    => Consume("!=", 2),
                (Tokens.Greater, Tokens.Equals)=> Consume(">=", 2),
                (Tokens.Less, Tokens.Equals)   => Consume("<=", 2),

                (Tokens.Less, _)               => Consume("<", 1),
                (Tokens.Greater, _)            => Consume(">", 1),

                _ => null
            };

            if (current.TokenType == Tokens.Keyword && current.Value == Keywords.TypeEqualityKeyword)
            {
                if (next?.TokenType == Tokens.Keyword && next.Value == Keywords.TypeNotEqualityKeyword)
                    operatorId = Consume(Keywords.TypeNotEqualityKeyword, 2);
                else operatorId =  Consume(Keywords.TypeEqualityKeyword, 1);
            }

            var caseValue = isDefault ? null : ParsePrimary();
            
            // =>
            Expect(Tokens.Equals);
            Expect(Tokens.Greater);

            var returnValue = ParseExpression();

            if (caseValue is null)
                defaultCase = returnValue;
            else
                cases[new ShortHandSwitchCase
                {
                    BinaryOperationId = operatorId,
                    Key = caseValue
                }] = returnValue;

            if (Check(Tokens.Comma))
            {
                Advance();
                continue;
            }
            
            if (!Check(Tokens.RBrace))
                throw new QlangCompileException("Expected ','", GetDebug(debugToken), "Parser");
            continue;

            int? Consume(string op, int count)
            {
                for (var i = 0; i < count; i++)
                    Advance();

                return stringPoolTable.Add(op);
            }
        }

        Expect(Tokens.RBrace);

        return new ShortHandSwitchNode
        {
            Value = value,
            Default = defaultCase,
            Cases = cases
        };
    }
    
    private ParallelNode ParseParallel()
    {
        ThrowIfIsEnd();

        Advance();
        
        var call = ParsePrimary();

        return new ParallelNode
        {
            Object = call
        };
    }

    private AssignmentNode ParseVariableDeclaration(bool skipKeyword = false, bool isPrivate = false, bool isConst = false, bool isNew = false)
    {
        ThrowIfIsEnd();
        
        if (!skipKeyword && (!Check(Tokens.Keyword) ||
            (Current().Value != Keywords.VariableDeclaration &&
             Current().Value != Keywords.ConstVariableDeclaration)))
            throw new QlangCompileException($"Undefined keyword for variable: {Token.TokenToString(Current())}. Keywords 'const' or 'let' allowed only.", GetDebug(Current()),"Parser");
        
        // Skip const or let
        if (Check(Tokens.Keyword, Keywords.VariableDeclaration) ||
            Check(Tokens.Keyword, Keywords.ConstVariableDeclaration))
            Advance();

        // Type
        var types = Check(Tokens.Less) ? ParseVariableTypes() : [];

        // Except var name
        var token = Expect(Tokens.Identifier);
        var name = token.Value;
        
        ASTNode? value = null;
        // Set if exists value
        if (Current().TokenType == Tokens.Equals)
        {
            Advance();
            value = ParseExpression();
        }

        var assignmentNode = new AssignmentNode(isPrivate, isConst, isNew)
        {
            Path = [new ObjectPointerNode { NameId = stringPoolTable.Add(name) }],
            Value = value,
            Types = types,
            FileId = debugTable?.GetFileId(token.DebugIndex) ?? -1
        };

        _assignmentNodes.Add(assignmentNode);
        
        return assignmentNode;
    }

    private UsingNode ParseUsing()
    {
        // Skip 'using'
        var token = Expect(Tokens.Keyword, Keywords.UsingKeyword);

        var node = ParsePrimary();

        if (node is not CallNode callNode)
            throw new QlangCompileException("Using call path must be path to namespace.", GetDebug(token), "Parser");

        var newObjects = new List<NamespacePointerNode>();

        foreach (var o in callNode.Objects)
        {
            switch (o)
            {
                case ObjectPointerNode pointer:
                    newObjects.Add(new NamespacePointerNode
                    {
                        NameId = pointer.NameId
                    });
                    break;
                case NamespacePointerNode namespacePointerNode:
                    newObjects.Add(namespacePointerNode);
                    break;
                default:
                    throw new QlangCompileException("Using objects in the path that are not namespaces is not allowed.", GetDebug(token), "Parser");
            }
        }

        callNode.Objects = newObjects.Cast<ASTNode>().ToList();
        return new UsingNode
        {
            CallPath = callNode, FileId = debugTable?.GetFileId(token.DebugIndex) ?? -1
        };
    }

    private NamespaceNode ParseNamespace(bool isPrivate)
    {

        // skip 'namespace'
        Expect(Tokens.Keyword, Keywords.NamespaceDeclaration);

        var nameToken = Expect(Tokens.Identifier);

        var token = Expect(Tokens.Colon);
        
        if (!Check(Tokens.LBrace))
            throw new QlangCompileException("Namespace's body cannot be one-lined", GetDebug(token), "Parser");
        
        var body = ParseBlock();
        
        return new NamespaceNode
        {
            NameId = stringPoolTable.Add(nameToken.Value),
            Body = body,
            IsPrivate = isPrivate,
        };
    }

    private List<CallNode> ParseVariableTypes()
    {
        // Type
        List<CallNode> returnTypes = [];
        if (!Check(Tokens.Less)) 
            return returnTypes;
        
        var debugToken = Expect(Tokens.Less);

        while (!Check(Tokens.Greater))
        {
            var expr = ParsePrimaryPath();

            if (expr is KeywordNode keyword && keyword.KeywordId == stringPoolTable.Add(Keywords.NullKeyword))
                expr = new CallNode
                {
                    FileId = debugTable.GetFileId(debugToken.DebugIndex),
                    Objects = [new ObjectPointerNode { NameId = stringPoolTable.Add("Nullable") }]
                };

            if (expr is not CallNode callNode)
                throw new QlangCompileException(
                    $"Cannot find function type by path '{expr.ToTokenString(stringPoolTable)}'.",
                    GetDebug(debugToken), "Parser");

            returnTypes.Add(callNode);

            if (Check(Tokens.Or))
            {
                Advance();
                continue;
            }

            if (!Check(Tokens.Greater))
                throw new QlangCompileException($"Cannot define expression '{expr.ToTokenString(stringPoolTable)}'",
                    GetDebug(debugToken), "Parser");
        }

        Expect(Tokens.Greater);

        return returnTypes;
    }

    private FunctionNode ParseFunction(bool isPrivate = false, bool isAsync = false, bool allowShortFn = false)
    {
        if (!Check(Tokens.Keyword, Keywords.FunctionDeclaration) && (allowShortFn && !Check(Tokens.Keyword, Keywords.ShortFunctionDeclaration)))
            throw new QlangCompileException("Cannot instantiate function without function keyword", GetDebug(Current()), "Parser");
        
        var debug = Advance();

        // Type
        var returnTypes = Check(Tokens.Less) ? ParseVariableTypes() : [];
       
        if (!Check(Tokens.Identifier) && !Check(Tokens.Keyword, Keywords.CreateClassInstanceKeyword))
            throw new QlangCompileException("Cannot use this name for function", GetDebug(debug), "Parser");
        
        var nameToken = Advance();
        Expect(Tokens.LParen);

        List<AssignmentNode> parameters = [];
        while (!Check(Tokens.RParen))
        {
            if (Check(Tokens.Comma))
            {
                Advance();
                continue;
            }
            
            parameters.Add(ParseVariableDeclaration(true, isConst: !Check(Tokens.Keyword, Keywords.VariableDeclaration)));

            // if (!Check(Tokens.RParen))
            //     throw new QlangCompileException($"Undefined function parameter '{Token.TokenToString(Current())}'.", GetDebug(Current()), "Parser");
        }

        Expect(Tokens.RParen);
        
        if (!Check(Tokens.Equals) || Peek()?.TokenType != Tokens.Greater)
            Expect(Tokens.Colon);

        var body = ParseBlock();

        return new FunctionNode
        {
            NameId = stringPoolTable.Add(nameToken.Value),
            Parameters = parameters,
            Body = body,
            IsPrivate = isPrivate,
            IsAsync = isAsync,
            ReturnTypes = returnTypes
        };
    }

    private ClassNode ParseClass(bool isPrivate)
    {
        Expect(Tokens.Keyword, Keywords.ClassDeclaration);
        var nameToken = Expect(Tokens.Identifier);

        CallNode? extends = null;
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ExtendsKeyword &&
            Peek()?.TokenType == Tokens.Identifier)
        {
            // Skip extends keyword
            Advance();
            

            var path = ParsePrimaryPath();
            
            if (path is not CallNode callNode)
                throw new QlangCompileException("Undefined path to extended class", GetDebug(nameToken), "Parser");

            extends = callNode;
        }
        
        Expect(Tokens.Colon);

        if (!Check(Tokens.LBrace))
            throw new QlangCompileException("Class's body cannot be one-lined", GetDebug(nameToken), "Parser");

        var body = ParseBlock();

        return new ClassNode
        {
            NameId = stringPoolTable.Add(nameToken.Value), 
            Body = body, 
            ExtendsPath = extends,
            IsPrivate = isPrivate,
            Id = Guid.NewGuid()
        };
    }

    private ForNode ParseFor()
    {
        Expect(Tokens.Keyword, Keywords.ForBlock);

        var assignment = ParseVariableDeclaration(isPrivate: false,
            isConst: Check(Tokens.Keyword) && Current().Value == Keywords.ConstVariableDeclaration,
            isNew: true);
        Expect(Tokens.Semicolon);
        var condition = ParseExpression();
        Expect(Tokens.Semicolon);
        var statement = ParseExpression();

        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);

        var forBlock = ParseBlock();


        return new ForNode { Assignment = assignment, Statement = statement, Condition = condition, Body = forBlock };
    }

    private TryCatchNode ParseTryCatch()
    {
        // Expect try:
        Expect(Tokens.Keyword, Keywords.TryKeyword);
        Expect(Tokens.Colon);
        
        var tryBlock = ParseBlock();
        
        var token = Expect(Tokens.Keyword,  Keywords.CatchKeyword);
        Expect(Tokens.LParen);
        
        if (!Check(Tokens.Keyword, Keywords.ConstVariableDeclaration))
            throw new QlangCompileException("Cannot use not const property for catch block", GetDebug(token), "Parser");

        var assignment = new LineNode(token.DebugIndex)
        {
            Content = ParseVariableDeclaration(isPrivate: false, isConst: true)
        };
        
        Expect(Tokens.RParen);
        Expect(Tokens.Colon);
        var catchBlock = ParseBlock();

        List<ASTNode> finallyBlock = [];
        if (Check(Tokens.Keyword, Keywords.FinallyKeyword))
        {
            Advance();
            Expect(Tokens.Colon);
            
            finallyBlock = ParseBlock();
        }

        return new TryCatchNode
        {
            TryBody = tryBlock,
            CatchAssignment = assignment,
            CatchBody = catchBlock,
            FinallyBody = finallyBlock
        };
    }

    private WhileNode ParseWhile(bool isDoWhile = false)
    {
        var debugIndex = Expect(Tokens.Keyword, isDoWhile ? Keywords.DoWhileBlock : Keywords.WhileBlock).DebugIndex;
        var condition = new LineNode(debugIndex)
        {
            Content = ParseExpression()
        };
        Expect(Tokens.Colon);

        var whileBlock = ParseBlock();

        return new WhileNode { Condition = condition, Body = whileBlock, IsDoWhile = isDoWhile };
    }

    private SwitchNode ParseSwitch()
    {
        var debugIndex = Expect(Tokens.Keyword, Keywords.SwitchBlock).DebugIndex;
        var condition = new LineNode(debugIndex)
        {
            Content = ParseExpression()
        };
        Expect(Tokens.Colon);
        Expect(Tokens.LBrace);

        var @switch = new SwitchNode { Condition = condition };
        
        while (Check(Tokens.Keyword) && Current().Value == Keywords.CaseKeyword)
        {
            debugIndex = Expect(Tokens.Keyword).DebugIndex;
            condition = new LineNode(debugIndex)
            {
                Content = ParseExpression()
            };
            
            Expect(Tokens.Colon);
            var block = ParseBlock();

            @switch.CaseBlocks.Add(new SwitchCaseNode
            {
                CaseBlock = block,
                Condition = condition,
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
        var debugIndex = Expect(Tokens.Keyword, Keywords.IfBlock).DebugIndex;
        var condition = new LineNode(debugIndex)
        {
            Content = ParseExpression()
        };
        Expect(Tokens.Colon);

        var thenBlock = ParseBlock();

        List<ASTNode> elseBlock = [];

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

        return new IfNode { Condition = condition, ThenBlock = thenBlock, ElseBlock = elseBlock };
    }

    private List<ASTNode> ParseBlock()
    {
        List<ASTNode> statements = [];

        if (Check(Tokens.LBrace) && Peek()?.TokenType == Tokens.RBrace)
        {
            Expect(Tokens.LBrace);
            Expect(Tokens.RBrace);
            return [];
        }

        // One line block
        // {
        if (!Check(Tokens.LBrace))
            statements.Add(ParseStatement(true));
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

        return statements;
    }

    private ReturnNode ParseReturn(bool isOneLine = false)
    {
        var isGreater = Peek()?.TokenType == Tokens.Greater;
        var debugIndex = Expect(isOneLine && Check(Tokens.Equals) ? Tokens.Equals : Tokens.Keyword).DebugIndex;

        if (isOneLine && isGreater)
            Expect(Tokens.Greater);

        ASTNode? node = null;
        if (!Check(Tokens.Semicolon))
            node = ParseExpression();

        if (!isOneLine || !isGreater)
            Expect(Tokens.Semicolon);

        return new ReturnNode { ReturnValue = node is LineNode ? node : new LineNode(debugIndex) { Content = node } };
    }

    /// <summary>
    /// Parsing expression (ex.: 5 + 5)
    /// </summary>
    private ASTNode ParseExpression()
    {
        return ParseLogicalOr();
    }
    
    /// <summary>
    /// Parsing logical 'is' or 'is not'
    /// </summary>
    private ASTNode ParseLogicalIs()
    {
        var left = ParseComparison();

        while (Check(Tokens.Keyword, Keywords.TypeEqualityKeyword))
        {
            var isNotEqual = false;

            var token = Advance();

            if (Check(Tokens.Keyword, Keywords.TypeNotEqualityKeyword))
            {
                isNotEqual = true;
                Advance();
            }
            
            var right = ParseComparison();
            
            if (right is KeywordNode keyword && keyword.KeywordId == stringPoolTable.Add(Keywords.NullKeyword))
                right = new CallNode
                {
                    FileId = debugTable.GetFileId(token.DebugIndex),
                    Objects = [new ObjectPointerNode { NameId = stringPoolTable.Add("Nullable") }]
                };
            

            if (right is not CallNode callNode)
                throw new QlangCompileException($"Cannot use object of type '{right.GetType().Name}' as path to class.",
                    GetDebug(token), "Parser");
            
            left = new TypeEqualityNode
            {
                Left = left,
                IsNotEqual = isNotEqual,
                Class = callNode
            };
        }

        return left;
    }

    /// <summary>
    /// Parsing logical '||'
    /// </summary>
    private ASTNode ParseLogicalOr()
    {
        var left = ParseLogicalAnd();

        while (Check(Tokens.Or) && Peek()?.TokenType == Tokens.Or)
        {
            Advance();
            Advance();
            var right = ParseLogicalAnd();
            left = new BinaryOperationNode
            {
                Left = left,
                OperatorId = stringPoolTable.Add("||"),
                Right = right
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
            Advance(); 
            Advance(); 
            var right = ParseEquality();
            left = new BinaryOperationNode
            {
                Left = left,
                OperatorId = stringPoolTable.Add("&&"),
                Right = right
            };
        }

        return left;
    }

    /// <summary>
    /// Parsing equality ('==', '!=')
    /// </summary>
    private ASTNode ParseEquality()
    {
        var left = ParseLogicalIs();

        while (true)
        {
            if (Check(Tokens.Equals) && Peek()?.TokenType == Tokens.Equals)
            {
                Advance(); 
                Advance(); 
                var right = ParseLogicalIs();
                left = new BinaryOperationNode
                {
                    Left = left,
                    OperatorId = stringPoolTable.Add("=="),
                    Right = right
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
                    OperatorId = stringPoolTable.Add("!="),
                    Right = right
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
                    op = "<";
                }

                var right = ParseAddition();
                left = new BinaryOperationNode
                {
                    Left = left,
                    OperatorId = stringPoolTable.Add(op),
                    Right = right
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
                    op = ">";
                }

                var right = ParseAddition();
                left = new BinaryOperationNode
                {
                    Left = left,
                    OperatorId = stringPoolTable.Add(op),
                    Right = right
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
            var op = Check(Tokens.Plus) ? "+" : "-";
            Advance();
            var right = ParseMultiplication();
            left = new BinaryOperationNode
            {
                Left = left,
                OperatorId = stringPoolTable.Add(op),
                Right = right
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
            var op = Current().TokenType switch
            {
                Tokens.Star => "*",
                Tokens.Slash => "/",
                Tokens.Percent => "%"
            };
            
            Advance();
            var right = ParsePrimary();
            left = new BinaryOperationNode
            {
                Left = left,
                OperatorId = stringPoolTable.Add(op),
                Right = right
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
            return new KeywordNode
            {
                KeywordId = stringPoolTable.Add(Keywords.NullKeyword)
            };
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.BreakKeyword)
        {
            // Advance 'break'
            Advance();
            return new KeywordNode
            {
                KeywordId = stringPoolTable.Add(Keywords.BreakKeyword)
            };
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ContinueKeyword)
        {
            // Advance 'continue'
            Advance();
            return new KeywordNode
            {
               KeywordId = stringPoolTable.Add(Keywords.ContinueKeyword)
            };
        }

        // bool return
        if (Check(Tokens.Keyword) && (Current().Value == Keywords.FalseKeyword || Current().Value == Keywords.TrueKeyword))
        {
            // Advance 'true' or 'false'
            return new BooleanNode
            {
                Value = isMinus ? !bool.Parse(Advance().Value) : bool.Parse(Advance().Value)
            };
        }

        // String reference
        if (Check(Tokens.StringRef))
        {
            // Advance '___S*___'
            return new StringRefNode
            {
                Index = int.Parse(Advance().Value)
            };
        }

        if (Check(Tokens.NumberRef))
        {
            // Advance '___N*___'
            
            return new NumberRefNode
            {
                IsNegative = isMinus,
                Index = int.Parse(Advance().Value)
            };
        }

        return null;
    }

    private ASTNode? ParsePrimaryField()
    {
        if (!Check(Tokens.Keyword, Keywords.FieldKeyword))
            return null;

        var token = Advance();

        Expect(Tokens.LParen);
        
        var privateVariableName = Expect(Tokens.Identifier).Value;
        
        ASTNode? expression = null;
        if (Check(Tokens.Equals))
        {
            Advance();
            
            expression = ParseExpression();
        }
        
        Expect(Tokens.RParen);
        
        Expect(Tokens.Colon);
        
        Expect(Tokens.LBrace);
        
        // Parse functions;
        FunctionNode? getFunc = null;
        FunctionNode? setFunc = null;
        for (var i = 0; i < 4; i++)
        {
            if ((Check(Tokens.Keyword, Keywords.FunctionDeclaration) || Check(Tokens.Keyword, Keywords.ShortFunctionDeclaration)) && Peek()?.Value == "get")
            {
                getFunc = ParseFunction(true, false, true);
                continue;
            }
            
            if ((Check(Tokens.Keyword, Keywords.FunctionDeclaration) || Check(Tokens.Keyword, Keywords.ShortFunctionDeclaration)) && Peek()?.Value == "set")
            {
                setFunc = ParseFunction(true, false, true);
                continue;
            }
        }
        Expect(Tokens.RBrace);

        return new FieldNode
        {
            PrivateVariable = new AssignmentNode
            {
                FileId = debugTable.GetFileId(token.DebugIndex),
                IsConst = false,
                IsNew = false,
                IsPrivate = true,
                Path = [new ObjectPointerNode {NameId = stringPoolTable.Add(privateVariableName)}],
                Types = [],
                Value = expression
            },
            GetFunction = getFunc,
            SetFunction = setFunc
        };
    }

    private ASTNode? ParsePrimaryCallable()
    {
        // Function pointer
        if (Check(Tokens.Keyword) && 
            (Current().Value == Keywords.ShortFunctionDeclaration || Current().Value == Keywords.FunctionDeclaration) &&
            (Peek()?.TokenType == Tokens.LParen || Peek()?.TokenType == Tokens.Less))
        {
            // Advance 'function' or 'fn'
            var token = Advance();

            // Type
            var returnTypes = Check(Tokens.Less) ? ParseVariableTypes() : [];
            
            // Advance '('
            Advance();
            var func = new FunctionNode
            {
                NameId = stringPoolTable.Add("~function")
            };

            List<AssignmentNode> parameters = [];
            while (!Check(Tokens.RParen))
            {
                if (Check(Tokens.Keyword, Keywords.VariableDeclaration))
                    throw new QlangCompileException("Cannot instantiate variable as non const value", GetDebug(token), "Parser");
                
                if (Check(Tokens.Comma))
                {
                    Advance();
                    continue;
                }

                parameters.Add(ParseVariableDeclaration(true, isConst: true));
            }
            // Advance ')'
            Advance();

            func.Parameters = parameters;

            // Advance ':'
            if (!Check(Tokens.Equals) || Peek()?.TokenType != Tokens.Greater)
                Expect(Tokens.Colon);

            func.Body = ParseBlock();

            func.ReturnTypes = returnTypes.ToList();
            
            return func;
        }

        // Object {}
        if (Check(Tokens.LBrace))
        {
            // Advance '{'
            Advance();
            var @class = new ClassNode
            {
                NameId = stringPoolTable.Add("~object"),
                Body = []
            };

            // While current token is not RBrace ('}')
            while (!Check(Tokens.RBrace))
            {
                var token = Current();
                
                if (Check(Tokens.Comma))
                {
                    Advance();
                    continue;
                }
                
                @class.Body.Add(new LineNode(token.DebugIndex) { Content = ParseVariableDeclaration(true, isPrivate: false, isConst: !Check(Tokens.Keyword, Keywords.VariableDeclaration)) });
                
                // if (!Check(Tokens.RBrace))
                    // throw new QlangCompileException("Undefined variable: " + Current().TokenType + " " + Current().Value, GetDebug(Current()), "Parser");
            }

            // Advance '}'
            Expect(Tokens.RBrace);

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

            return new CollectionNode
            {
                Collection = statements
            };
        }

        // parse: '('expression')'
        if (Check(Tokens.LParen))
        {
            // Advance '('
            Expect(Tokens.LParen);
            var parsed = ParseExpression();
            // Advance ')'
            Expect(Tokens.RParen);

            return new ParensNode
            {
                Statement = parsed
            };
        }

        return null;
    }

    // Allows += -= /= *= %= = ++ --
    private bool CanBeAssignmentNode()
    {
        return (Check(Tokens.Equals) && Peek()?.TokenType != Tokens.Equals && Peek()?.TokenType != Tokens.Greater) ||
               (
                (
                    Check(Tokens.Plus) || 
                    Check(Tokens.Minus) || 
                    Check(Tokens.Star) || 
                    Check(Tokens.Slash) ||
                    Check(Tokens.Percent)
                ) && 
                Peek()?.TokenType == Tokens.Equals
               ) || 
               (Check(Tokens.Plus) && Peek()?.TokenType == Tokens.Plus) ||
               (Check(Tokens.Minus) && Peek()?.TokenType == Tokens.Minus);
    }

    private ASTNode? ParsePrimaryExtracted(bool isMinus)
    {
        if (Current().TokenType != Tokens.Identifier)
            return null;

        var currentIdentifier = Current().Value;

        if (currentIdentifier.StartsWith("___S"))
        {
            // Advance '___S*___'
            Advance();
            var index = int.Parse(currentIdentifier.Replace("___S", "").Replace("___", ""));
            return new StringRefNode { Index = index, };
        }

        if (currentIdentifier.StartsWith("___N"))
        {
            // Advance '___N*___'
            Advance();
            
            var index = int.Parse(currentIdentifier.Replace("___N", "").Replace("___", ""));
            return new NumberRefNode
            {
                IsNegative = isMinus,
                Index = index
            };
        }

        if (currentIdentifier.TryParseNumber(out var res))
        {
            // Advance '1'
            Advance();
            
            return new NumberNode
            {
                Value = res
            };
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
                NameId = stringPoolTable.Add(Keywords.ThisKeyword)
            };
            // Advance 'this'
            Expect(Tokens.Keyword);
        }

        if (Current().TokenType != Tokens.Identifier)
            return current;

        var identifier = Current().Value;
        // Advance 'identifier'
        var token = Advance();

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
                NameId = stringPoolTable.Add(identifier),
                Arguments = args,
                IsNullable = Check(Tokens.Question) && Peek()?.TokenType == Tokens.Dot
            };
        }

        var isNullable = Check(Tokens.Question) && Peek()?.TokenType == Tokens.Dot;
        if (isNullable)
            Advance();

        current ??= new ObjectPointerNode
        {
            NameId = stringPoolTable.Add(identifier),
            IsNullable = isNullable
        };

        // Parse assign path 
        if (CanBeAssignmentNode())
        {
            var @operator = Current().TokenType switch
            {
                Tokens.Plus when Peek()?.TokenType == Tokens.Plus => "++",
                Tokens.Plus => "+",
                Tokens.Minus when Peek()?.TokenType == Tokens.Minus => "--",
                Tokens.Minus => "-",
                Tokens.Star => "*",
                Tokens.Slash => "/",
                Tokens.Percent => "%",
                _ => ""
            };

            if (@operator != "")
                Advance();
            Advance();
            
            current = new AssignmentNode(false, false, false)
            {
                Path = [new ObjectPointerNode { NameId = stringPoolTable.Add(identifier) }],
                FileId = debugTable.GetFileId(token.DebugIndex)
            };

            var value = @operator switch
            {
                "++" => new BinaryOperationNode
                {
                    Left = new CallNode
                    {
                        Objects = [new ObjectPointerNode { NameId = stringPoolTable.Add(identifier) }],
                        FileId = debugTable.GetFileId(token.DebugIndex)
                    },
                    Right = new NumberNode { Value = 1 },
                    OperatorId = stringPoolTable.Add("+")
                },
                "--" => new BinaryOperationNode
                {
                    Left = new CallNode
                    {
                        Objects = [new ObjectPointerNode { NameId = stringPoolTable.Add(identifier) }],
                        FileId = debugTable.GetFileId(token.DebugIndex)
                    },
                    Right = new NumberNode { Value = 1 },
                    OperatorId = stringPoolTable.Add("-")
                },
                _ => @operator == ""
                    ? ParseExpression()
                    : new BinaryOperationNode
                    {
                        Left = new CallNode
                        {
                            Objects = [new ObjectPointerNode { NameId = stringPoolTable.Add(identifier) }],
                            FileId = debugTable.GetFileId(token.DebugIndex)
                        },
                        Right = ParseExpression(),
                        OperatorId = stringPoolTable.Add(@operator)
                    }
            };

            ((AssignmentNode)current).Value = value;

            ((AssignmentNode)current).FileId = debugTable?.GetFileId(token.DebugIndex) ?? -1;
            
            _assignmentNodes.Add((AssignmentNode)current);

            return current;
        }

        if (Check(Tokens.Colon) && Peek()?.TokenType == Tokens.Colon)
            current = new NamespacePointerNode
            {
                NameId = stringPoolTable.Add(identifier)
            };

        return current;
    }

    private ASTNode ParsePrimaryPath()
    {
        var astNode = ParsePrimary(true);

        var token = Current();
        var isDot = Check(Tokens.Dot);
        var isDoubleColon = Check(Tokens.Colon) && Peek()?.TokenType == Tokens.Colon;
        
        // Is not path (NOT 'call.node.func().etc();', JUST LIKE 'call' or 'call()' etc.)
        if (!isDot && !isDoubleColon)
        {
            var returnValue = astNode switch
            {
                ObjectPointerNode obj => new CallNode
                {
                    Objects = [obj],
                    FileId = debugTable.GetFileId(token.DebugIndex)
                },
                NamespacePointerNode namespacePointer => new CallNode
                {
                    Objects = [namespacePointer],
                    FileId = debugTable.GetFileId(token.DebugIndex)
                },
                FunctionPointerNode pointerFunction => new CallNode
                {
                    Objects = [pointerFunction],
                    FileId = debugTable.GetFileId(token.DebugIndex)
                },
                _ => astNode
            };

            if (returnValue is CallNode cNode)
            {
                cNode.FileId = debugTable?.GetFileId(token.DebugIndex) ?? -1;
                _callNodes.Add(cNode);
            }
            
            return returnValue;
        }

        List<ASTNode> path = [];

        if (astNode is CallNode callNode)
            path.AddRange(callNode.Objects);
        else
            path.Add(astNode);
        
        do
        {
            if (Current().TokenType == Tokens.Colon && Peek()?.TokenType == Tokens.Colon)
                Advance();
            
            Advance();
            
            astNode = ParsePrimary(true);

            if (astNode is CallNode node)
                path.AddRange(node.Objects);
            else
                path.Add(astNode);
            
            // Do circle while '.' or '::'
        } while (Check(Tokens.Dot) || (Check(Tokens.Colon) && Peek()?.TokenType == Tokens.Colon));
        
        if (path[^1] is AssignmentNode assignmentNode)
        {
            path[^1] = new ObjectPointerNode { NameId = assignmentNode.GetLastNameId() };
            var assignment = new AssignmentNode(false, false, false)
            {
                Path = path,
                Value = assignmentNode.Value,
                FileId = debugTable?.GetFileId(token.DebugIndex) ?? -1
            };
            
            _assignmentNodes.Add(assignment);

            return assignment;
        }

        var retValue =  new CallNode
        {
            Objects =  path,
            FileId = debugTable?.GetFileId(token.DebugIndex) ?? -1
        };
        
        _callNodes.Add(retValue);

        return retValue;
    }

    private CastNode? ParsePrimaryCast()
    {
        if (!Check(Tokens.Less))
            return null;

        var token = Advance();

        var path = ParsePrimaryPath();
        
        if (path is not CallNode callNode)
            throw new QlangCompileException($"Cannot find type to cast by path '{path.ToTokenString(stringPoolTable)}'", GetDebug(token), "Parser");
        
        Expect(Tokens.Greater);
        
        path = ParsePrimaryPath();

        var callPathNode = path as CallNode ?? new CallNode();

        if (path is not CallNode)
        {
            callPathNode = new CallNode { Objects = [path], FileId = debugTable.GetFileId(token.DebugIndex) };
            _callNodes.Add(callPathNode);
        }

        return new CastNode
        {
            ToCastObject = callPathNode,
            TypeCastPath = callNode
        };
    }

    private NewNode? ParsePrimaryNew()
    {
        if (!Check(Tokens.Keyword, Keywords.CreateClassInstanceKeyword))
            return null;
        
        var debugIndex = Advance();

        var primaryPath =  ParsePrimaryPath();
        
        if (primaryPath is not CallNode callNode)
            throw new QlangCompileException("Undefined class reference for creating instance", GetDebug(debugIndex), "Parser");
        
        var newNode = new NewNode
        {
            NodePath = callNode
        };
        
        _callNodes.Add(newNode.NodePath);
        
        return newNode;
    }

    private AwaitNode? ParsePrimaryAwait()
    {
        if (!Check(Tokens.Keyword, Keywords.AwaitKeyword))
            return null;

        var token = Advance();
        
        var content = ParsePrimaryPath();
        
        if (content is not  CallNode callNode)
            throw new QlangCompileException("Undefined await statement", GetDebug(token), "Parser");

        var await = new AwaitNode
        {
            CallNode = callNode
        };

        _callNodes.Add(await.CallNode);
        
        return await;
    }

    private BinaryOperationNode? ParsePrimaryNotBool()
    {
        if (!Check(Tokens.Not))
            return null;
        Advance();
        
        var ast = ParsePrimary();
        
        return new BinaryOperationNode
        {
            Left = ast,
            Right = new BooleanNode { Value = false },
            OperatorId = stringPoolTable.Add("==")
        };
    }
    
    private ASTNode ParsePrimary(bool isLoop = false)
    {
        if (!isLoop)
            return ParsePrimaryPath();
        
        var isMinus = false;
        if (Check(Tokens.Minus))
        {
            isMinus = true;
            Advance();
        }
        
        return ParsePrimaryAwait() 
               ?? ParsePrimaryField()
               ?? ParseShortHandIf()
               ?? ParseShortHandSwitch()
               ?? ParsePrimaryNotBool()
               ?? ParsePrimaryCast()
               ?? ParsePrimaryNew()
               ?? ParsePrimaryCallable()
               ?? ParsePrimaryKeywords(isMinus)
               ?? ParsePrimaryExtracted(isMinus)
               ?? ParsePrimaryCall()
               ?? ParsePrimaryParens()
               ?? throw new QlangCompileException($"Undefined part of expression: {Current().TokenType} ({(Current().Value == "" ? "<null>" : Current().Value)})", GetDebug(Current()), "Parser");
    }

    // Support methods
    private Token Current()
    {
        ThrowIfIsEnd();
        
        return _tokens[_position];
    }

    private Token? Previous() => _position - 1 >= _tokens.Count ? null : _tokens[_position - 1];
    private Token? Peek() => _position + 1 < _tokens.Count ? _tokens[_position + 1] : null;
    private bool IsAtEnd() => _position >= _tokens.Count;
    private bool Check(Tokens type) => !IsAtEnd() && Current().TokenType == type;
    private bool Check(Tokens type, string? value) => !IsAtEnd() && Current().TokenType == type && Current().Value == value;

    private Token Advance()
    {
        ThrowIfIsEnd();
        
        if (!IsAtEnd()) _position++;
        var token = _tokens[_position - 1];

        return token;
    }

    private Token Expect(Tokens type, string? value = null)
    {
        ThrowIfIsEnd();
        
        if (!Check(type))
        {
            var current = Current();
            
            throw new QlangCompileException(
                $"Incorrect syntax, expected '{Token.TokenToString(new Token(type, value ?? ""))}', got '{Token.TokenToString(current)}'",
                GetDebug(current), "Parser");
        }

        if (value != null && Current().Value != value)
            throw new QlangCompileException($"Expected '{value}', got '{Current().Value}'", GetDebug(Current()), "Parser");

        return Advance();
    }
    
    public static List<ClassNode> GetClassesFromNamespaceRecursively(NamespaceNode @namespace)
    {
        var list = @namespace.Body.OfType<ClassNode>().ToList();

        foreach (var subNamespace in @namespace.Body.OfType<NamespaceNode>().ToList())
            list.AddRange(GetClassesFromNamespaceRecursively(subNamespace));
        
        return list;
    }
    
    public static List<NamespaceNode> GetNamespacesFromNamespaceRecursively(NamespaceNode @namespace)
    {
        var list = @namespace.Body.OfType<NamespaceNode>().ToList();

        foreach (var subNamespace in @namespace.Body.OfType<NamespaceNode>().ToList())
            list.AddRange(GetNamespacesFromNamespaceRecursively(subNamespace));
        
        return list;
    }

    private void ThrowIfIsEnd()
    {
        if (IsAtEnd())
            throw new QlangCompileException("Cannot throw identified error, because file is ended. Line is obtained from last defined object.", GetDebug(Previous()), "Parser");
    }
    
    private (int, string) GetDebug(Token? token)
    {
        if (token is null)
            return (-1, "undefined token");

        if (debugTable is null || sourceFileTable is null)
            return (-1, "debug is not included");

        if (token.DebugIndex >= debugTable.LineIndexes.Count)
        {
            Console.WriteLine("DebugIndex of token: " + token.DebugIndex + " | Debug size: " +  debugTable.LineIndexes.Count);
            throw new QlangCompileException("Debug index of token is too big", (-1, ""), "Parser");
        }
        
        return (debugTable.GetLineIndex(token.DebugIndex) + 1,
            sourceFileTable[debugTable.GetFileId(token.DebugIndex)]);
    }
}
