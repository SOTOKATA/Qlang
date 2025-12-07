using Qlang.Core.Lang.AST;
using Qlang.Core.LangDebug;

namespace Qlang.Core.Lang.Interpreter;

public partial class Interpreter
{
    private bool _return = false;
    private object? _returnValue = null;
    private bool _isBreakKeyword = false;
    private bool _isContinueKeyword = false;
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
        Logger.Log("FirstCheck (node)\n" + whileNode.Condition.GetTree("     "), "While.Condition", ConsoleColor.Magenta);
        Logger.Log("FirstCheck: \n" +  condition, "While.Condition", ConsoleColor.Magenta);

        while (condition)
        {
            if (ExecuteBlock(whileNode.Body, true))
                return;
            
            condition = (bool)EvaluateExpression(whileNode.Condition);
            Logger.Log("FirstCheck: \n" +  condition, "While.Condition", ConsoleColor.Magenta);
        }
        
        RemoveLastBlockFromContext();
        Logger.Log("Ended", "While", ConsoleColor.Magenta);
    }

    private bool ExecuteBlock(List<ASTNode> block, bool isLoop)
    {
        foreach (var statement in block)
        {
            if (_return)
            {
                RemoveLastBlockFromContext();
                return true;
            }

            if (statement is ReturnNode returnNode)
            {
                _return = true;
                _returnValue = EvaluateExpression(returnNode.ReturnValue);
                RemoveLastBlockFromContext();
                return true;
            }

            if (isLoop)
            {
                if (statement is ContinueNode || _isContinueKeyword)
                {
                    _isContinueKeyword = false;
                    break;
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
        Logger.Log("FirstCheck: \n" +  condition, "For.Condition", ConsoleColor.Magenta);
        
        while (condition)
        {
            if (ExecuteBlock(forNode.Body, true))
                return;

            ExecuteStatement(forNode.Statement);
            condition = (bool)EvaluateExpression(forNode.Condition);
            Logger.Log("FirstCheck: \n" +  condition, "For.Condition", ConsoleColor.Magenta);
        }
        
        RemoveLastBlockFromContext();
        Logger.Log("Ended", "For", ConsoleColor.Magenta);

    }
    
    private void ExecuteIf(IfNode ifNode)
    {
        AddBlockToContext(ifNode);
        
        // 1. Вычисляем условие (expression -> bool)
        var condition = (bool)EvaluateExpression(ifNode.Condition);

        // 2. Выполняем нужный блок
        if (condition)
        {
            if (ExecuteBlock(ifNode.ThenBlock, false))
                return;
        }
        else if (ifNode.ElseBlock.Count > 0)
        {
            if (ExecuteBlock(ifNode.ElseBlock, false))
                return;
        }
        
        RemoveLastBlockFromContext();
    }
}