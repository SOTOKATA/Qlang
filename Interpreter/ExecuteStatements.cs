using Core;
using Core.AST;
using Core.Dynamic;
using Core.Exceptions;

namespace Interpreter;

public partial class Interpreter
{
    private void AddBlockToContext(ASTBlock block, Stack<ASTContext> stack)
    {
        if (stack.Count > 0)
            CurrentContext(stack)!.Blocks.Add(block);
    }

    private void RemoveLastBlockFromContext(Stack<ASTContext> stack)
    {
        var currentContext = CurrentContext(stack);
        if (stack.Count > 0 && currentContext?.Blocks.Count > 0)
            currentContext.Blocks.RemoveAt(currentContext.Blocks.Count - 1);
    }

    private void ExecuteWhile(WhileNode whileNode, Stack<ASTContext> stack)
    {
        AddBlockToContext(whileNode, stack);

        var condition = whileNode.IsDoWhile || (bool)EvaluateExpression(whileNode.Condition, stack)!;

        while (condition)
        {
            if (ExecuteBlock(whileNode.Body, true, stack))
            {
                RemoveLastBlockFromContext(stack);
                return;
            }

            condition = (bool)EvaluateExpression(whileNode.Condition, stack)!;
        }

        RemoveLastBlockFromContext(stack);
    }

    private bool ExecuteBlock(List<ASTNode> block, bool isLoop, Stack<ASTContext> stack)
    {
        foreach (var statement in block)
        {
            var currentContext = CurrentContext(stack)!;
            if (currentContext.IsReturn)
                return true;

            if (statement is ReturnNode returnNode)
            {
                currentContext.ReturnValue = EvaluateExpression(returnNode.ReturnValue, stack);
                currentContext.IsReturn = true;
                return true;
            }

            if (isLoop)
            {
                if (statement is LineNode { Content: KeywordNode kNode })
                {
                    var nodeName = _stringPoolTable[kNode.KeywordId];
                    if (nodeName == Keywords.ContinueKeyword)
                    {
                        currentContext.IsContinue = false;
                        return false;
                    }
                
                    if (nodeName == Keywords.BreakKeyword)
                    {
                        currentContext.IsBreak = false;
                        return true;
                    }
                }
                if (currentContext.IsContinue)
                {
                    currentContext.IsContinue = false;
                    return false;
                }
                
                if (currentContext.IsBreak)
                {
                    currentContext.IsBreak = false;
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
                        currentContext.IsContinue = true;
                        return true;
                    }
                    if (nodeName == Keywords.BreakKeyword)
                    {
                        currentContext.IsBreak = true;
                        return true;
                    }
                }
            }

            ExecuteStatement(statement, stack);
        }

        return false;
    }

    private void ExecuteFor(ForNode forNode, Stack<ASTContext> stack)
    {
        AddBlockToContext(forNode, stack);

        // Adding variable
        ExecuteStatement(forNode.Assignment, stack);

        // Add condition
        var condition = (bool)EvaluateExpression(forNode.Condition, stack)!;

        while (condition)
        {
            if (ExecuteBlock(forNode.Body, true, stack))
            {
                RemoveLastBlockFromContext(stack);
                return;
            }

            ExecuteStatement(forNode.Statement, stack);
            condition = (bool)EvaluateExpression(forNode.Condition, stack)!;
        }

        RemoveLastBlockFromContext(stack);

    }

    private void ExecuteIf(IfNode ifNode, Stack<ASTContext> stack)
    {
        AddBlockToContext(ifNode, stack);
        var returnValue = EvaluateExpression(ifNode.Condition, stack);

        if (returnValue is not bool condition)
            throw new QlangRuntimeException("Cannot apply non bool condition in if", GetCurrentDebug(stack),
                GetStackTrace(stack));

        if (condition)
            ExecuteBlock(ifNode.ThenBlock, false, stack);
        else if (ifNode.ElseBlock.Count > 0)
            ExecuteBlock(ifNode.ElseBlock, false, stack);

        RemoveLastBlockFromContext(stack);
    }
    
    private void ExecuteTryCatch(TryCatchNode tryCatchNode, Stack<ASTContext> stack)
    {
        AddBlockToContext(tryCatchNode, stack);

        try
        {
            ExecuteBlock(tryCatchNode.TryBody, false, stack);
        }
        catch (Exception ex)
        {
            var assignment = (AssignmentNode)tryCatchNode.CatchAssignment.Content!;
            stack.Peek().CurrentDebugIndex = tryCatchNode.CatchAssignment.DebugIndex;
            
            tryCatchNode.Variables[_stringPoolTable[assignment.GetLastNameId()]] = new Variable(_stringPoolTable[assignment.GetLastNameId()], ToQlangException(ex, stack), false, true);
            
            ExecuteBlock(tryCatchNode.CatchBody, false, stack);
        }
        finally
        {
            ExecuteBlock(tryCatchNode.FinallyBody, false, stack);
        }

        RemoveLastBlockFromContext(stack);
    }
    
    private void ExecuteSwitch(SwitchNode switchNode, Stack<ASTContext> stack)
    {
        AddBlockToContext(switchNode, stack);

        var block = switchNode.DefaultBlock;

        foreach (var pair in from pair in switchNode.CaseBlocks let binOp = new BinaryOperationNode
                 {
                     Left = pair.Condition,
                     Right = switchNode.Condition,
                     OperatorId = _stringPoolTable.Add("==")
                 } let result = (bool)EvaluateBinaryOperation(binOp, stack)! where result select pair)
        {
            block = pair.CaseBlock;
            break;
        }

        if (block is null)
        {
            RemoveLastBlockFromContext(stack);
            return;
        }

        ExecuteBlock(block, false, stack);

        RemoveLastBlockFromContext(stack);
    }
}
