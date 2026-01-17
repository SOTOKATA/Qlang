using Core.AST;
using Core.Debug;

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
            CurrentContext.Blocks.Count > 0)
            CurrentContext.Blocks.Add(block);
    }

    private void RemoveLastBlockFromContext()
    {
        if (_contextStack.Count > 0 && CurrentContext.Blocks.Count > 0)
            CurrentContext.Blocks.RemoveAt(CurrentContext.Blocks.Count - 1);
    }

    private void ExecuteWhile(WhileNode whileNode)
    {
        AddBlockToContext(whileNode);

        var condition = whileNode.IsDoWhile || (bool)EvaluateExpression(whileNode.Condition);
        Logger.Log("FirstCheck (node)\n" + whileNode.Condition.GetTree("     "), "While.Condition");
        Logger.Log("FirstCheck: \n" + condition, "While.Condition");

        while (condition)
        {
            if (ExecuteBlock(whileNode.Body, true))
            {
                RemoveLastBlockFromContext();
                return;
            }

            condition = (bool)EvaluateExpression(whileNode.Condition);
            Logger.Log("FirstCheck: \n" + condition, "While.Condition");
        }

        RemoveLastBlockFromContext();
        Logger.Log("Ended", "While");
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
                if (statement is ContinueNode || _isContinueKeyword)
                {
                    _isContinueKeyword = false;
                    return false;
                }

                if (statement is BreakNode || _isBreakKeyword)
                {
                    _isBreakKeyword = false;
                    return true;
                }
            }
            else
            {
                switch (statement)
                {
                    case BreakNode:
                        _isBreakKeyword = true;
                        return true;
                    case ContinueNode:
                        _isContinueKeyword = true;
                        return true;
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
        var condition = (bool)EvaluateExpression(forNode.Condition);
        Logger.Log("FirstCheck: \n" + condition, "For.Condition");

        while (condition)
        {
            if (ExecuteBlock(forNode.Body, true))
            {
                RemoveLastBlockFromContext();
                return;
            }

            ExecuteStatement(forNode.Statement);
            condition = (bool)EvaluateExpression(forNode.Condition);
            Logger.Log("FirstCheck: \n" + condition, "For.Condition");
        }

        RemoveLastBlockFromContext();
        Logger.Log("Ended", "For");

    }

    private void ExecuteIf(IfNode ifNode)
    {
        AddBlockToContext(ifNode);

        var condition = (bool)EvaluateExpression(ifNode.Condition);

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
                     Operator = "=="
                 } let obj = (bool)EvaluateBinaryOperation(binOp) where obj select pair)
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
