include "$lib/base"

// Тест проблемы с вызовом методов в условии if при return

function test_method_call_in_if(): {
    Console.println("=== Тест: Вызов метода в условии if ===");

    let testArray = Array.new([]);
    testArray.push(Array.new([0, 1, 2]));

    for let i = 0; i < 3; i = i + 1: {
        Console.println("i = " + i);

        let row = testArray.at(0);
        let value = row.at(i);
        Console.println("value = " + value);

        // Тест с вызовом метода в условии
        if row.at(i) == 1: {
            Console.println("Метод вернул 1, делаем return");
            return "METHOD_CALL_SUCCESS";
        }

        Console.println("Метод не вернул 1, продолжаем");
    }

    Console.println("ОШИБКА: Дошли до конца функции!");
    return "METHOD_CALL_FAILED";
}

function test_method_call_stored(): {
    Console.println("=== Тест: Сохранение результата метода ===");

    let testArray = Array.new([]);
    testArray.push(Array.new([0, 1, 2]));

    for let i = 0; i < 3; i = i + 1: {
        Console.println("i = " + i);

        let row = testArray.at(0);
        let value = row.at(i);  // Сохраняем результат метода
        Console.println("value = " + value);

        // Используем сохраненное значение
        if value == 1: {
            Console.println("Сохраненное значение равно 1, делаем return");
            return "STORED_SUCCESS";
        }

        Console.println("Сохраненное значение не равно 1, продолжаем");
    }

    Console.println("ОШИБКА: Дошли до конца функции!");
    return "STORED_FAILED";
}

function test_nested_method_calls(): {
    Console.println("=== Тест: Вложенные вызовы методов ===");

    let testArray = Array.new([]);
    testArray.push(Array.new([0, 1]));
    testArray.push(Array.new([2, 3]));

    for let y = 0; y < 2; y = y + 1: {
        Console.println("y = " + y);

        for let x = 0; x < 2; x = x + 1: {
            Console.println("  x = " + x);

            // Точная имитация Tetris - вызов метода в условии
            if testArray.at(y).at(x) == 1: {
                Console.println("    Найдено значение 1, делаем return");
                return false;
            }

            Console.println("    Значение не равно 1");
        }

        Console.println("Конец внутреннего цикла");
    }

    Console.println("ОШИБКА: return НЕ сработал в вложенных вызовах!");
    return true;
}

function test_simple_comparison(): {
    Console.println("=== Тест: Простое сравнение без методов ===");

    for let i = 0; i < 3; i = i + 1: {
        Console.println("i = " + i);

        if i == 1: {
            Console.println("i равно 1, делаем return");
            return "SIMPLE_SUCCESS";
        }
    }

    Console.println("ОШИБКА: Дошли до конца функции!");
    return "SIMPLE_FAILED";
}

function check(): {
    for let i = 0; i < 10; i = i + 1: {
        for let j = 0; j < 10; j = j + 1: {
            if i == 5: {
                return false;
            }
        }
    }
    return true;
}

function call(): {
    while true: {
        if check() == false: {
            Console.println("false");
            return;
        }
    }
    Console.println("yes");

}

function main(): {
    call();
}

function mainf(): {
    Console.println("========================================");
    Console.println("   ТЕСТ ВЫЗОВОВ МЕТОДОВ В IF + RETURN  ");
    Console.println("========================================");

    // Тест 1: Простое сравнение (контроль)
    let result1 = test_simple_comparison();
    Console.println("Результат простого теста: " + result1);
    Console.println("");

    // Тест 2: Вызов метода в условии
    let result2 = test_method_call_in_if();
    Console.println("Результат с методом в if: " + result2);
    Console.println("");

    // Тест 3: Сохранение результата метода
    let result3 = test_method_call_stored();
    Console.println("Результат с сохранением: " + result3);
    Console.println("");

    // Тест 4: Вложенные вызовы методов (как в Tetris)
    let result4 = test_nested_method_calls();
    Console.println("Результат вложенных методов: " + result4);
    Console.println("");

    Console.println("========================================");
    Console.println("            АНАЛИЗ РЕЗУЛЬТАТОВ          ");
    Console.println("========================================");

    if result1 == "SIMPLE_SUCCESS": {
        Console.println("✓ Простое сравнение: return работает");
    } else: {
        Console.println("✗ Простое сравнение: return НЕ работает");
    }

    if result2 == "METHOD_CALL_SUCCESS": {
        Console.println("✓ Метод в if: return работает");
    } else: {
        Console.println("✗ Метод в if: return НЕ работает");
    }

    if result3 == "STORED_SUCCESS": {
        Console.println("✓ Сохраненный метод: return работает");
    } else: {
        Console.println("✗ Сохраненный метод: return НЕ работает");
    }

    if result4 == false: {
        Console.println("✓ Вложенные методы: return работает");
    } else: {
        Console.println("✗ Вложенные методы: return НЕ работает");
    }

    Console.println("");
    Console.println("ВЫВОД: Если 'Вложенные методы' не работает,");
    Console.println("то проблема в комбинации методов + return в циклах!");
}
