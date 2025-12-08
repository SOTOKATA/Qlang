include "$lib/base"

// Тест проблемы с return в циклах

function test_return_in_loop(): {
    Console.println("=== Тест Return в циклах ===");

    let marker = "NOT_SET";

    for let i = 0; i < 5; i = i + 1: {
        Console.println("Итерация: " + i);

        if i == 2: {
            marker = "BEFORE_RETURN";
            Console.println("Выполняем return на итерации 2");
            return "EARLY_EXIT";
        }
    }

    marker = "AFTER_LOOP";
    Console.println("Код после цикла - НЕ должен выполниться!");
    return "NORMAL_EXIT";
}

function test_return_in_nested_loops(): {
    Console.println("=== Тест Return в вложенных циклах ===");

    let result = "";

    for let i = 0; i < 3; i = i + 1: {
        Console.println("Внешний цикл i = " + i);

        for let j = 0; j < 3; j = j + 1: {
            Console.println("  Внутренний цикл j = " + j);

            if i == 1 && j == 1: {
                result = "FOUND_TARGET";
                Console.println("  Выполняем return из вложенного цикла");
                return result;
            }

            result = result + i + j;
        }

        Console.println("Конец внутреннего цикла для i = " + i);
    }

    Console.println("Код после вложенных циклов - НЕ должен выполниться!");
    return "COMPLETED_ALL_LOOPS";
}

function test_return_similar_to_tetris(): {
    Console.println("=== Тест аналогичный Tetris canPlace ===");

    let testArray = Array.new([]);
    testArray.push(Array.new([1, 0, 1]));
    testArray.push(Array.new([0, 1, 0]));

    let foundProblem = true;

    for let y = 0; y < testArray.length(); y = y + 1: {
        let row = testArray.at(y);
        for let x = 0; x < row.length(); x = x + 1: {
            Console.println("Проверяем позицию (" + x + ", " + y + ") = " + row.at(x));

            if row.at(x) == 1: {
                // Имитируем проверку границ как в Tetris
                if x >= 2: {  // Имитируем выход за границу
                    foundProblem = false;
                    Console.println("НАЙДЕНА ПРОБЛЕМА! Выполняем return false");
                    return false;
                }
            }
        }
    }

    // Этот код аналогичен проблемному коду в Tetris
    if foundProblem == false: {
        Console.println("КРИТИЧЕСКАЯ ОШИБКА: Код после return выполнился!");
        Console.println("Return НЕ РАБОТАЕТ правильно в циклах!");
    }

    Console.println("Возвращаем true в конце функции");
    return true;
}

function test_return_outside_loop(): {
    Console.println("=== Тест Return вне цикла (контроль) ===");

    Console.println("Код до return");
    return "SIMPLE_RETURN";
    Console.println("Код после return - НЕ должен выполниться!");
}

function test_break_continue_vs_return(): {
    Console.println("=== Сравнение Break, Continue и Return ===");

    Console.println("--- Тест Break ---");
    for let i = 0; i < 5; i = i + 1: {
        if i == 2: {
            Console.println("Break на i = 2");
            break;
        }
        Console.println("Break test i = " + i);
    }
    Console.println("Код после break цикла выполнился");

    Console.println("--- Тест Continue ---");
    for let i = 0; i < 5; i = i + 1: {
        if i == 2: {
            Console.println("Continue на i = 2");
            continue;
        }
        Console.println("Continue test i = " + i);
    }
    Console.println("Код после continue цикла выполнился");

    Console.println("--- Тест Return ---");
    for let i = 0; i < 5; i = i + 1: {
        if i == 2: {
            Console.println("Return на i = 2");
            return "RETURN_FROM_LOOP";
        }
        Console.println("Return test i = " + i);
    }
    Console.println("ОШИБКА: Код после return цикла выполнился!");
    return "SHOULD_NOT_REACH";
}

// Главная функция
function main(): {
    Console.println("========================================");
    Console.println("   ДИАГНОСТИКА ПРОБЛЕМЫ С RETURN      ");
    Console.println("========================================");

    // Тест 1: Простой return (должен работать)
    let result1 = test_return_outside_loop();
    Console.println("Результат простого return: " + result1);
    Console.println();

    // Тест 2: Return в цикле
    let result2 = test_return_in_loop();
    Console.println("Результат return в цикле: " + result2);
    Console.println();

    // Тест 3: Return в вложенных циклах
    let result3 = test_return_in_nested_loops();
    Console.println("Результат return в вложенных циклах: " + result3);
    Console.println();

    // Тест 4: Аналогичная Tetris ситуация
    let result4 = test_return_similar_to_tetris();
    Console.println("Результат Tetris-подобного теста: " + result4);
    Console.println();

    // Тест 5: Сравнение с break/continue
    let result5 = test_break_continue_vs_return();
    Console.println("Результат сравнения: " + result5);

    Console.println("\n========================================");
    Console.println("           АНАЛИЗ ЗАВЕРШЕН             ");
    Console.println("========================================");

    Console.println("Если вы видите сообщения об ошибках выше,");
    Console.println("то return действительно НЕ РАБОТАЕТ в циклах!");
}
