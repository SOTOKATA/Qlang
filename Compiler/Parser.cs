using Core;
using Core.AST;
using Core.Exceptions;

namespace Compiler;

/*
 * Second stage of compilation
 * Translated code from the lexer is converted into C# classes
 */
public class Parser
{
    private List<Token> _tokens = [];
    private int _position;

    private string _line = "";

    private List<CallNode> _callNodes = [];
    private List<AssignmentNode> _assignmentNodes = [];
    
    private SourceFileTable _sourceFileTable;

    private DebugTable _debugTable;

    public ProgramNode Parse(List<Token> tokens, SourceFileTable table, DebugTable debugTable)
    {
        _sourceFileTable = table;
        _debugTable = debugTable;
        _tokens = tokens;
        _position = 0;

        ProgramNode program = new(-1);

        
        while (!IsAtEnd())
        {
            if (Check(Tokens.Semicolon))
            {
                Advance();
                continue;
            }

            program.Statements.Add(ParseStatement());
        }

        var postParser = new PostParser(_sourceFileTable, _debugTable);
        
        program = postParser.CreateGlobalNamespace(program);
        postParser.MergeNamespaces(program.Statements);
        program = postParser.IncludeUsings(program, _callNodes, _assignmentNodes);
        program = postParser.IncludeExtends(program);
        program = new Optimizer().Optimize(program);

        new Validator(_sourceFileTable, _debugTable).CheckValidate(program);

        return program;
    }

    private ASTNode ParseStatement()
    {
        var isStatic = false;
        var isPrivate = false;

        if (Check(Tokens.Keyword) && Current().Value == Keywords.StaticModificator)
        {
            isStatic = true;
            Advance();
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.PrivateModificator)
        {
            isPrivate = true;
            Advance();
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.BreakKeyword)
            return new KeywordNode(Keywords.BreakKeyword, Advance().DebugIndex);
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ContinueKeyword)
            return new KeywordNode(Keywords.ContinueKeyword, Advance().DebugIndex);
        
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
            return expr;
        }
        
        if (Check(Tokens.LParen))
        {
            var expr = ParseExpression();
            Expect(Tokens.Semicolon);
            return expr;
        }
        
        if (Check(Tokens.LSquareParen))
        {
            var expr = ParseExpression();
            Expect(Tokens.Semicolon);
            return expr;
        }

        throw new QlangCompileException($"Unexpected token: {Current().TokenType} '{Current().Value}'", (IsAtEnd() ? 0 : _debugTable.GetLineIndex(Current().DebugIndex) + 1), "Parser", _sourceFileTable[_debugTable.GetFileId(Current().DebugIndex)]);
    }

    private AssignmentNode ParseVariableDeclaration(bool canUseType, bool isStatic = false, bool isPrivate = false, bool isConst = false, bool isNew = false)
    {
        if (!Check(Tokens.Keyword) ||
            (Current().Value != Keywords.VariableDeclaration &&
             Current().Value != Keywords.ConstVariableDeclaration))
            throw new QlangCompileException($"(ParseVariableDeclaration) Unexpected token: {Current().TokenType}", (IsAtEnd() ? 0 : _debugTable.GetLineIndex(Current().DebugIndex) + 1), "Parser", _sourceFileTable[_debugTable.GetFileId(Current().DebugIndex)]);

        Advance();

        CallNode? type = null;
        if (Check(Tokens.Less))
        {
            if (!canUseType)
            {
                var token = Current();
                throw new QlangCompileException(
                    "Using variables with types is only possible with function arguments", _debugTable.GetLineIndex(token.DebugIndex), "Parser", _sourceFileTable[_debugTable.GetFileId(token.DebugIndex)]);
            }
            Advance();
            var returnValue  = ParsePrimaryPath();

            if (returnValue is not CallNode node)
            {
                var token = Current();
                throw new QlangCompileException("Cannot use follow node as path to class", (_debugTable.GetLineIndex(token.DebugIndex) + 1), "Parser", _sourceFileTable[_debugTable.GetFileId(token.DebugIndex)]);
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

        var assignmentNode = new AssignmentNode(isStatic, isPrivate, isConst, isNew, Current().DebugIndex)
        {
            Path = [new ObjectPointerNode(Current().DebugIndex) { Name = name }],
            Value = value,
            Type = type
        };
        
        _assignmentNodes.Add(assignmentNode);
        
        return assignmentNode;
    }

    private UsingNode ParseUsing()
    {
        // Skip 'using'
        Expect(Tokens.Keyword, Keywords.UsingKeyword);

        var node = ParsePrimary();

        if (node is not CallNode callNode)
            throw new QlangCompileException("Using must be path to namespace.", GetDebug(node), "Parser");

        var newObjects = new List<NamespacePointerNode>();

        foreach (var o in callNode.Objects)
        {
            switch (o)
            {
                case ObjectPointerNode pointer:
                    newObjects.Add(new NamespacePointerNode(pointer.Name!, pointer.DebugIndex));
                    break;
                case NamespacePointerNode namespacePointerNode:
                    newObjects.Add(namespacePointerNode);
                    break;
                default:
                    throw new QlangCompileException("Using objects in the path that are not namespaces is not allowed.", GetDebug(o), "Parser");
            }
        }

        callNode.Objects = newObjects.Cast<ASTNode>().ToList();
        return new UsingNode(Expect(Tokens.Semicolon).DebugIndex)
        {
            CallPath = callNode
        };
    }

    private NamespaceNode ParseNamespace(bool isPrivate)
    {

        // skip 'namespace'
        Expect(Tokens.Keyword, Keywords.NamespaceDeclaration);

        var nameToken = Expect(Tokens.Identifier);

        Expect(Tokens.Colon);
        
        if (!Check(Tokens.LBrace))
            throw new QlangCompileException("Namespace's body cannot be one-line", _debugTable.GetLineIndex(Current().DebugIndex), "Parser", _sourceFileTable[_debugTable.GetFileId(Current().DebugIndex)]);
        
        var body = ParseBlock();
        
        return new NamespaceNode(nameToken.DebugIndex)
        {
            Name = nameToken.Value,
            Body = body,
            IsPrivate = isPrivate,
        };
    }

    private FunctionNode ParseFunction(bool isStatic = false, bool isPrivate = false)
    {

        Expect(Tokens.Keyword, Keywords.FunctionDeclaration);
        var nameToken = Expect(Tokens.Identifier);
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

        return new FunctionNode(nameToken.DebugIndex)
        {
            Name = nameToken.Value,
            Parameters = parameters,
            Body = body,
            IsStatic = isStatic,
            IsPrivate = isPrivate
        };
    }

    private ClassNode ParseClass()
    {
        Expect(Tokens.Keyword, Keywords.ClassDeclaration);
        var nameToken = Expect(Tokens.Identifier);

        var extends = "";
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ExtendsKeyword &&
            Peek()?.TokenType == Tokens.Identifier)
        {
            // Skip extends keyword
            Advance();

            extends = Expect(Tokens.Identifier).Value;
        }
        
        Expect(Tokens.Colon);

        if (!Check(Tokens.LBrace))
            throw new QlangCompileException("Class's body cannot be one-line", _debugTable.GetLineIndex(Current().DebugIndex), "Parser", _sourceFileTable[_debugTable.GetFileId(Current().DebugIndex)]);

        var body = ParseBlock();

        return new ClassNode(nameToken.DebugIndex) { Name = nameToken.Value, Body = body, Extends = extends };
    }

    private ForNode ParseFor()
    {
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


        return new ForNode(Current().DebugIndex) { Assignment = assignment, Statement = statement, Condition = condition, Body = forBlock };
    }

    private WhileNode ParseWhile(bool isDoWhile = false)
    {
        Expect(Tokens.Keyword, isDoWhile ? Keywords.DoWhileBlock : Keywords.WhileBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);

        var whileBlock = ParseBlock();

        return new WhileNode(Current().DebugIndex) { Condition = condition, Body = whileBlock, IsDoWhile = isDoWhile };
    }

    private SwitchNode ParseSwitch()
    {
        Expect(Tokens.Keyword, Keywords.SwitchBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        Expect(Tokens.LBrace);

        var @switch = new SwitchNode(Current().DebugIndex) { Condition = condition };
        
        while (Check(Tokens.Keyword) && Current().Value == Keywords.CaseKeyword)
        {
            Expect(Tokens.Keyword);
            condition = ParseExpression();
            Expect(Tokens.Colon);
            var block = ParseBlock();

            @switch.CaseBlocks.Add(new SwitchCaseNode(Current().DebugIndex)
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
        Expect(Tokens.Keyword, Keywords.IfBlock);
        var condition = ParseExpression();
        Expect(Tokens.Colon);
        // Expect(Tokens.Semicolon);

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

        return new IfNode(Current().DebugIndex) { Condition = condition, ThenBlock = thenBlock, ElseBlock = elseBlock };
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

        return statements;
    }

    private ReturnNode ParseReturn()
    {
        Expect(Tokens.Keyword);

        ASTNode? node = null;
        if (!Check(Tokens.Semicolon))
            node = ParseExpression();

        Expect(Tokens.Semicolon);

        return new ReturnNode(Current().DebugIndex) { ReturnValue = node };
    }

    /// <summary>
    /// Parsing expression (ex.: 5 + 5)
    /// Приоритет (от низкого к высокому): ||, &&, ==, !=, <, >, <=, >=, +, -, *, /, %
    /// </summary>
    private ASTNode ParseExpression()
    {
        var result = ParseLogicalOr();
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
            left = new BinaryOperationNode(Current().DebugIndex)
            {
                Left = left,
                Operator = "||",
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
            Advance(); // первый &
            Advance(); // второй &
            var right = ParseEquality();
            left = new BinaryOperationNode(Current().DebugIndex)
            {
                Left = left,
                Operator = "&&",
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
        var left = ParseComparison();

        while (true)
        {
            if (Check(Tokens.Equals) && Peek()?.TokenType == Tokens.Equals)
            {
                Advance(); // первый =
                Advance(); // второй =
                var right = ParseComparison();
                left = new BinaryOperationNode(Current().DebugIndex)
                {
                    Left = left,
                    Operator = "==",
                    Right = right
                };
            }
            else if (Check(Tokens.Not) && Peek()?.TokenType == Tokens.Equals)
            {
                Advance(); // !
                Advance(); // =
                var right = ParseComparison();
                left = new BinaryOperationNode(Current().DebugIndex)
                {
                    Left = left,
                    Operator = "!=",
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
                left = new BinaryOperationNode(Current().DebugIndex)
                {
                    Left = left,
                    Operator = op,
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
                left = new BinaryOperationNode(Current().DebugIndex)
                {
                    Left = left,
                    Operator = op,
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
            left = new BinaryOperationNode(Current().DebugIndex)
            {
                Left = left,
                Operator = op,
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
            left = new BinaryOperationNode(Current().DebugIndex)
            {
                Left = left,
                Operator = op,
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
            return new KeywordNode(Keywords.NullKeyword, Current().DebugIndex);
        }

        if (Check(Tokens.Keyword) && Current().Value == Keywords.BreakKeyword)
        {
            // Advance 'break'
            Advance();
            return new KeywordNode(Keywords.BreakKeyword, Current().DebugIndex);
        }
        if (Check(Tokens.Keyword) && Current().Value == Keywords.ContinueKeyword)
        {
            // Advance 'continue'
            Advance();
            return new KeywordNode(Keywords.ContinueKeyword, Current().DebugIndex);
        }

        // bool return
        if (Check(Tokens.Keyword) && (Current().Value == Keywords.FalseKeyword || Current().Value == Keywords.TrueKeyword))
        {
            // Advance 'true' or 'false'
            return new BooleanNode(Current().DebugIndex)
            {
                Value = isMinus ? !bool.Parse(Advance().Value) : bool.Parse(Advance().Value)
            };
        }

        // String reference
        if (Check(Tokens.StringRef))
        {
            // Advance '___STRING_*___'
            return new StringRefNode(Current().DebugIndex)
            {
                Index = int.Parse(Advance().Value)
            };
        }

        if (Check(Tokens.NumberRef))
        {
            // Advance '___NUMBER_*___'
            return new NumberRefNode(Current().DebugIndex)
            {
                IsNegative = isMinus,
                Index = int.Parse(Advance().Value)
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
            var func = new FunctionNode(Current().DebugIndex)
            {
                Name = "___function_pointer___"
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
            var @class = new ClassNode(Current().DebugIndex)
            {
                Name = "___object___",
                Body = [],
                Extends = null
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

            return new CollectionNode(Current().DebugIndex)
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
            return new StringRefNode(Current().DebugIndex) { Index = index, };
        }

        if (currentIdentifier.StartsWith("___NUMBER_"))
        {
            // Advance '___NUMBER_*___'
            Advance();
            var index = int.Parse(currentIdentifier.Replace("___NUMBER_", "").Replace("___", ""));
            return new NumberRefNode(Current().DebugIndex)
            {
                IsNegative = isMinus,
                Index = index
            };
        }

        if (currentIdentifier.TryParseNumber(out var res))
        {
            // Advance '1'
            Advance();
            return new NumberNode(Current().DebugIndex)
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
            current = new ObjectPointerNode(Current().DebugIndex)
            {
                Name = Keywords.ThisKeyword
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
            
            current = new FunctionPointerNode(Current().DebugIndex)
            {
                Name = identifier,
                Arguments = args
            };
        }

        if (current is null)
            current = new ObjectPointerNode(Current().DebugIndex)
            {
                Name = identifier
            };

        // Parse assign path 
        if (Check(Tokens.Equals) && Peek()?.TokenType != Tokens.Equals)
        {
            // Advance '='
            Advance();
            current = new AssignmentNode(false, false, false, false, Current().DebugIndex)
            {
                Path = [new ObjectPointerNode(Current().DebugIndex) { Name = identifier }],
                Value = ParseExpression()
            };
            
            _assignmentNodes.Add((AssignmentNode)current);

            return current;
        }

        if (Check(Tokens.Colon) && Peek()?.TokenType == Tokens.Colon)
            current = new NamespacePointerNode(identifier, Current().DebugIndex);

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
            var returnValue = astNode switch
            {
                ObjectPointerNode obj => new CallNode(Current().DebugIndex)
                {
                    Objects = [obj]
                },
                NamespacePointerNode namespacePointer => new CallNode(Current().DebugIndex)
                {
                    Objects = [namespacePointer]
                },
                FunctionPointerNode pointerFunction => new CallNode(Current().DebugIndex)
                {
                    Objects = [pointerFunction],
                    Arguments = pointerFunction.Arguments
                },
                _ => astNode
            };

            if (returnValue is CallNode cNode)
                _callNodes.Add(cNode);
            
            return returnValue;
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
            path[^1] = new ObjectPointerNode(Current().DebugIndex) { Name = assignmentNode.GetLastName() };
            var assignment = new AssignmentNode(false, false, false, false, Current().DebugIndex)
            {
                Path = path,
                Value = assignmentNode.Value
            };
            
            _assignmentNodes.Add(assignment);

            return assignment;
        }

        var args = path[^1] is FunctionPointerNode ptr ? ptr.Arguments : [];
        
        var retValue =  new CallNode(Current().DebugIndex)
        {
            Objects =  path,
            Arguments = args
        };
        
        _callNodes.Add(retValue);

        return retValue;
    }

    private CastNode? ParseCast()
    {
        if (!Check(Tokens.Less))
            return null;

        Advance();

        var path = ParsePrimaryPath();
        
        if (path is not CallNode callNode)
            throw new QlangCompileException("Cannot find type to cast", GetDebug(path), "Parser");
        
        Expect(Tokens.Greater);
        
        path = ParsePrimaryPath();

        var callPathNode = path as CallNode ?? new CallNode(Current().DebugIndex);

        if (path is not CallNode)
        {
            callPathNode = new CallNode(Current().DebugIndex) { Objects = [path] };
            _callNodes.Add(callPathNode);
        }

        // if (path is not CallNode objCallNode)
            // throw new QlangCompileException("Cannot find object to cast", path.Line, "Parser", path.SourceFile ?? "undefined");

        return new CastNode(callNode, callPathNode, Current().DebugIndex);
    }

    
    private ASTNode ParsePrimary(bool isLoop = false)
    {

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

        baseExpression = ParseCast();
        if (baseExpression is not null)
            return baseExpression;

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
        
        throw new QlangCompileException($"Unexpected token in expression: {Current().TokenType} ({(Current().Value == "" ? "Null" : Current().Value)})", GetDebug(baseExpression), "Parser");
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

        // Logger.Log(token.TokenType.ToString() + " " + token.Value, $"Token (Ln:{token.Line})");
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
                                 """, (IsAtEnd() ? 0 : _debugTable.GetLineIndex(Current().DebugIndex) + 1), "Parser", _sourceFileTable[_debugTable.GetFileId(Current().DebugIndex)]);
        }

        if (value != null && Current().Value != value)
            throw new QlangCompileException($"Expected '{value}', got '{Current().Value}'", (IsAtEnd() ? 0 : _debugTable.GetLineIndex(Current().DebugIndex) + 1), "Parser", _sourceFileTable[_debugTable.GetFileId(Current().DebugIndex)]);

        return Advance();
    }
    
    public static List<ClassNode> GetClassesFromNamespaceRecursively(NamespaceNode @namespace)
    {
        var list = @namespace.Body.OfType<ClassNode>().ToList();

        foreach (var subNamespace in @namespace.Body.OfType<NamespaceNode>().ToList())
            list.AddRange(GetClassesFromNamespaceRecursively(subNamespace));
        
        return list;
    }

    private (int, string) GetDebug(ASTNode node)
    {
        return (_debugTable.GetLineIndex(node.DebugIndex) + 1, _sourceFileTable[_debugTable.GetFileId(node.DebugIndex)]);
    }
    
    
}
