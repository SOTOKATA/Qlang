# Анализ проблем с реализацией Return, Break, Continue в Qlang

## Обзор проведенного анализа

После изучения кода интерпретатора Qlang были выявлены несколько критических проблем в реализации управляющих конструкций `return`, `break`, и `continue`.

## Найденные проблемы

### 1. **Проблема с глобальными флагами break/continue**

**Файл:** `ExecuteStatements.cs`
**Строки:** 5-7

```csharp
private bool _return;
private object? _returnValue;
private bool _isBreakKeyword;
private bool _isContinueKeyword;
```

**Проблема:** 
Флаги `_isBreakKeyword` и `_isContinueKeyword` являются глобальными для всего интерпретатора. Это создает проблемы:

1. **Вложенные циклы:** При выполнении break/continue во вложенном цикле, флаг может повлиять на внешний цикл
2. **Рекурсивные вызовы:** Флаги могут "протекать" между различными уровнями рекурсии
3. **Параллельное выполнение:** Если в будущем добавится многопоточность, будут race conditions

**Пример проблемного кода:**
```qlang
for (int i = 0; i < 3; i++) {
    for (int j = 0; j < 3; j++) {
        if (j == 1) {
            break; // Может повлиять на внешний цикл
        }
    }
}
```

### 2. **Неправильная обработка break/continue вне циклов**

**Файл:** `ExecuteStatements.cs`
**Строки:** 81-90

```csharp
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
```

**Проблема:**
Когда `break` или `continue` встречается вне цикла (например, в простом `if` блоке), устанавливается соответствующий флаг, но он может повлиять на следующий цикл или функцию.

**Ожидаемое поведение:** Должна выбрасываться ошибка или игнорироваться команда.

### 3. **Проблема с очисткой флагов**

**Файл:** `Interpreter.cs` (ExecuteFunction)
**Строки:** 129-131

```csharp
_return = false;
_isBreakKeyword = false;
_isContinueKeyword = false;
```

**Проблема:**
Флаги сбрасываются только в начале выполнения функции. Если break/continue установлены в одной функции, но не обработаны (например, потому что нет циклов), они могут повлиять на следующую вызванную функцию.

### 4. **Потенциальная проблема с return в ExecuteFunction**

**Файл:** `Interpreter.cs`
**Строки:** 137-147

```csharp
foreach (var statement in function.Body.TakeWhile(_ => !_return))
{
    if (statement is ReturnNode returnNode)
    {
        if (returnNode.ReturnValue is not null)
            _returnValue = EvaluateExpression(returnNode.ReturnValue);
        
        break;
    }

    ExecuteStatement(statement);
}
```

**Потенциальная проблема:**
`TakeWhile(_ => !_return)` проверяет флаг `_return` перед каждой итерацией, но `ExecuteStatement(statement)` может установить `_return` внутри себя (например, при вызове другой функции с return). В таком случае цикл продолжится до следующей итерации.

### 5. **Дублирование логики обработки return**

**Проблема:**
Return обрабатывается и в `ExecuteFunction`, и в `ExecuteBlock`. Это может привести к inconsistent behavior.

## Рекомендуемые исправления

### 1. Использование локальных состояний вместо глобальных флагов

```csharp
public class LoopExecutionContext 
{
    public bool ShouldBreak { get; set; }
    public bool ShouldContinue { get; set; }
}

public class FunctionExecutionContext
{
    public bool ShouldReturn { get; set; }
    public object? ReturnValue { get; set; }
}
```

### 2. Передача контекста в методы выполнения

```csharp
private bool ExecuteBlock(List<ASTNode> block, LoopExecutionContext? loopContext = null)
{
    // Обработка break/continue только если передан loopContext
    // Иначе - ошибка или игнорирование
}
```

### 3. Строгая проверка контекста для break/continue

```csharp
if (statement is BreakNode || statement is ContinueNode) 
{
    if (loopContext == null) 
    {
        throw new QlangRuntimeException("break/continue outside of loop");
    }
    // Обработка...
}
```

### 4. Исправление логики return в функциях

```csharp
// Убрать TakeWhile и обрабатывать return явно в цикле
foreach (var statement in function.Body)
{
    ExecuteStatement(statement, functionContext);
    if (functionContext.ShouldReturn) 
    {
        break;
    }
}
```

## Тестовые сценарии для проверки

1. **Вложенные циклы с break/continue**
2. **Break/continue вне циклов**
3. **Return в различных контекстах**
4. **Рекурсивные вызовы с return**
5. **Комбинированные сценарии (return + break + continue)**

## Критичность проблем

- **Высокая:** Глобальные флаги break/continue (может вызвать непредсказуемое поведение)
- **Средняя:** Break/continue вне циклов (некорректное поведение)
- **Низкая:** Дублирование логики return (больше maintenance issue)

## Заключение

Текущая реализация имеет серьезные архитектурные проблемы, которые могут привести к непредсказуемому поведению программ. Рекомендуется рефакторинг с использованием локальных контекстов выполнения вместо глобальных флагов.