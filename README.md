# Возможности Qlang (база)

## Variables

Qlang не имеет явной типизации в переменных (то есть значение переменной может быть всем, что угодно)

### Определение переменной

Переменные назначяються аналогично python:
`$[varname] = [value]`
``$`` - обозначает, что слово есть переменная
```[varname]``` - имя переменной
```=``` - идентификатор присвоения
```[value]``` - значение, которое будет присвоено

Пример: `$age = 21`

### Получение значения

Получение значения переменной происходит через просто написание ее

## Functions

### Определение функции

`function [fnname]([arg1], [arg2]):`

`function` - указатель, что это структура функция
`[fnname]` - имя функции
`[arg1]` - аргумент (переменная)
`[arg2]` - тоже самое
`()` - "контейнер" где определяються аргументы

Пример: `function main(string[] args):`

### Вызов функции

Функции можно вызывать, для этого нужно просто написать имя функции и аргументы (если нужны)

Пример: `Math.sum(1, 1)`

### Classes

На данный момент не возможно создание пользовательских классов

Встроеные классы

- Term
- Math

## Blocks

### If-else

If-else - это смесь C и Python

Пример:

```cpp
if [condition]:
  [action]
else if [condition-2]:
  [action-2]
else
  [action-3]
```

`[condition]` - условие (например: `1 == 1`)

`[action]` - действие, если `[condition]` верно

### While, Do-While

While аналогисный While из Python.

```python
while [condition]:
  [action]
```

Do-While отличяеться (сама конструкция):

```cpp
do_while [condition]:
  [action]
```

### For

Структура for аналогична C, но не имеет скобок:

```cpp
for [variable_definition], [condition], [variable_change]:
  [action]
```

`[variable_definition]` - создание переменной

`[condition]` - условие

`[variable_change]` - изменение созданой переменной

### Список всех классов и их функций

Term

- `print` - print arguments
- `println` - print arguments and add new line on end
- `read` - read the user's input from console

Math

- `sum` - sum two numbers
- `sub` - substration of two numbers
- `mult` - multiplication of two numbers
- `div` - divide two numbers

# Compile

Компиляция в готовую програму (.exe) на данный момент не потдерживаеться.

Возможна только компиляция и интерпретация кода.

Для этого установите Qlang и используйте следущую команду: `qlang [file_path]`
