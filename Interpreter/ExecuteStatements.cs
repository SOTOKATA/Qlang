using Core;
using Core.AST;
using Core.Exceptions;

namespace Interpreter;

public partial class Interpreter
{
    private bool _return;
    private object? _returnValue;
    private bool _isBreakKeyword;
    private bool _isContinueKeyword;
    private void AddBlockToContext(ASTBlock block)
    {
        if (_contextStack.Count > 0 &&
            CurrentContext!.Blocks.Count > 0)
            CurrentContext.Blocks.Add(block);
    }

    private void RemoveLastBlockFromContext()
    {
        if (_contextStack.Count > 0 && CurrentContext!.Blocks.Count > 0)
            CurrentContext.Blocks.RemoveAt(CurrentContext.Blocks.Count - 1);
    }

    private void ExecuteWhile(WhileNode whileNode)
    {
        AddBlockToContext(whileNode);

        var condition = whileNode.IsDoWhile || (bool)EvaluateExpression(whileNode.Condition)!;

        while (condition)
        {
            if (ExecuteBlock(whileNode.Body, true))
            {
                RemoveLastBlockFromContext();
                return;
            }

            condition = (bool)EvaluateExpression(whileNode.Condition)!;
        }

        RemoveLastBlockFromContext();
    }

    private bool ExecuteBlock(List<ASTNode> block, bool isLoop)
    {
        foreach (var statement in block)
        {
            if (_return)
                return true;

            if (statement is ReturnNode returnNode)
            {
                _returnValue = EvaluateExpression(returnNode.ReturnValue);
                _return = true;
                return true;
            }

            if (isLoop)
            {
                if (statement is LineNode { Content: KeywordNode kNode })
                {
                    var nodeName = _stringPoolTable[kNode.KeywordId];
                    if (nodeName == Keywords.ContinueKeyword)
                    {
                        _isContinueKeyword = false;
                        return false;
                    }
                
                    if (nodeName == Keywords.BreakKeyword)
                    {
                        _isBreakKeyword = false;
                        return true;
                    }
                }
                if (_isContinueKeyword)
                {
                    _isContinueKeyword = false;
                    return false;
                }
                
                if (_isBreakKeyword)
                {
                    _isBreakKeyword = false;
                    return true;
                }
            }
            else
            {
                if (statement is LineNode { Content: KeywordNode kNode })
                {
                    var nodeName = _stringPoolTable[kNode.KeywordId];
                    if (nodeName == Keywords.ContinueKeyword)
                    {
                        _isContinueKeyword = true;
                        return true;
                    }
                    if (nodeName == Keywords.BreakKeyword)
                    {
                        _isBreakKeyword = true;
                        return true;
                    }
                }
            }

            ExecuteStatement(statement);
        }

        return false;
    }

    private void ExecuteFor(ForNode forNode)
    {
        AddBlockToContext(forNode);

        // Adding variable
        ExecuteStatement(forNode.Assignment);

        // Add condition
        var condition = (bool)EvaluateExpression(forNode.Condition)!;

        while (condition)
        {
            if (ExecuteBlock(forNode.Body, true))
            {
                RemoveLastBlockFromContext();
                return;
            }

            ExecuteStatement(forNode.Statement);
            condition = (bool)EvaluateExpression(forNode.Condition)!;
        }

        RemoveLastBlockFromContext();

    }

    private void ExecuteIf(IfNode ifNode)
    {
        AddBlockToContext(ifNode);
        var returnValue = EvaluateExpression(ifNode.Condition);

        if (returnValue is not bool condition)
            throw new QlangRuntimeException("Cannot apply non bool condition in if", GetCurrentDebug(),
                GetStackTrace());

        if (condition)
            ExecuteBlock(ifNode.ThenBlock, false);
        else if (ifNode.ElseBlock.Count > 0)
            ExecuteBlock(ifNode.ElseBlock, false);

        RemoveLastBlockFromContext();
    }
    
    private void ExecuteSwitch(SwitchNode switchNode)
    {
        AddBlockToContext(switchNode);

        var block = switchNode.DefaultBlock;

        foreach (var pair in from pair in switchNode.CaseBlocks let binOp = new BinaryOperationNode
                 {
                     Left = pair.Condition,
                     Right = switchNode.Condition,
                     OperatorId = _stringPoolTable.Add("==")
                 } let result = (bool)EvaluateBinaryOperation(binOp)! where result select pair)
        {
            block = pair.CaseBlock;
            break;
        }

        if (block is null)
        {
            RemoveLastBlockFromContext();
            return;
        }

        ExecuteBlock(block, false);

        RemoveLastBlockFromContext();
    }
}
