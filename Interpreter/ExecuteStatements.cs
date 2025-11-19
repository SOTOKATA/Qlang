using Qlang.AST;

namespace Qlang.Interpreter;

public partial class Interpreter
{
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
        Logger.Logger.Log("Interpreter.While: until.condition=\n" + whileNode.Condition.GetTree("     "), ConsoleColor.Magenta);
        Logger.Logger.Log("Interpreter.While: condition=" + condition, ConsoleColor.Magenta);

        while (condition)
        {
            foreach (var statement in whileNode.Body)
            {
                if (_break)
                {
                    Logger.Logger.Warn("Is break!");
                    RemoveLastBlockFromContext();
                    return;
                }

                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    RemoveLastBlockFromContext();
                    return;
                }
                
                ExecuteStatement(statement);
            }
            
            condition = (bool)EvaluateExpression(whileNode.Condition);
            Logger.Logger.Log("Interpreter.While: condition=" + condition, ConsoleColor.Magenta);
        }
        
        RemoveLastBlockFromContext();
        Logger.Logger.Log("Interpreter.While.End", ConsoleColor.Magenta);
    }

    private void ExecuteFor(ForNode forNode)
    {
        AddBlockToContext(forNode);
        
        // Adding variable
        ExecuteStatement(forNode.Assignment);
        
        // Add condition
        var condition = (bool)EvaluateExpression(forNode.Condition);
        Logger.Logger.Log("Interpreter.For: condition=" + condition, ConsoleColor.Magenta);

        while (condition)
        {
            foreach (var statement in forNode.Body)
            {
                if (_break)
                {
                    Logger.Logger.Warn("Is break!");
                    RemoveLastBlockFromContext();
                    return;
                }

                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    RemoveLastBlockFromContext();
                    return;
                }
                
                ExecuteStatement(statement);
            }

            ExecuteStatement(forNode.Statement);
            condition = (bool)EvaluateExpression(forNode.Condition);
            Logger.Logger.Log("Interpreter.While: condition=" + condition, ConsoleColor.Magenta);
        }
        
        RemoveLastBlockFromContext();
        Logger.Logger.Log("Interpreter.While.End", ConsoleColor.Magenta);
    }
    
    private void ExecuteIf(IfNode ifNode)
    {
        AddBlockToContext(ifNode);
        
        // 1. Вычисляем условие (expression -> bool)
        var condition = (bool)EvaluateExpression(ifNode.Condition);

        // 2. Выполняем нужный блок
        if (condition)
        {
            foreach (var statement in ifNode.ThenBlock)
            {
                if (_break)
                {
                    RemoveLastBlockFromContext();
                    return;
                }

                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    RemoveLastBlockFromContext();
                    return;
                }
                
                ExecuteStatement(statement);
            }
        }
        else if (ifNode.ElseBlock.Count > 0)
        {
            foreach (var statement in ifNode.ElseBlock)
            {
                if (_break)
                {
                    RemoveLastBlockFromContext();
                    return;
                }
                
                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    RemoveLastBlockFromContext();
                    return;
                }
                
                ExecuteStatement(statement);
            }
        }
        
        RemoveLastBlockFromContext();
    }
}