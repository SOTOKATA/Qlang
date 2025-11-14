using Qlang.AST;

namespace Qlang.Interpreter;

public partial class Interpreter
{
    private void ExecuteWhile(WhileNode whileNode)
    {
        var condition = whileNode.IsDoWhile || (bool)EvaluateExpression(whileNode.Condition);

        while (condition)
        {
            foreach (var statement in whileNode.Body)
            {
                if (_break)
                {
                    Logger.Logger.Warn("Is break!");
                    return;
                }

                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    return;
                }
                
                ExecuteStatement(statement);
            }
            
            condition = (bool)EvaluateExpression(whileNode.Condition);
        }
    }

    private void ExecuteIf(IfNode ifNode)
    {
        // 1. Вычисляем условие (expression -> bool)
        var condition = (bool)EvaluateExpression(ifNode.Condition);

        // 2. Выполняем нужный блок
        if (condition)
        {
            foreach (var statement in ifNode.ThenBlock)
            {
                if (_break)
                    return;

                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
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
                    return;
                
                if (statement is ReturnNode returnNode)
                {
                    _break = true;
                    _return = returnNode;
                    return;
                }
                
                ExecuteStatement(statement);
            }
        }
    }
}