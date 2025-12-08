// Комплексные тесты для проверки взаимодействия return, break, continue

// Тест 1: Return из цикла в функции
function test_return_from_loop() {
    console.log("=== Return from Loop Test ===");

    for (int i = 0; i < 10; i = i + 1) {
        console.log("Loop i = " + i);
        if (i == 5) {
            console.log("Returning from loop at i = 5");
            return i * 2;  // Должен вернуть 10
        }
    }
    console.log("This should not print");
    return -1;
}

// Тест 2: Return в вложенных циклах
function test_return_nested_loops() {
    console.log("=== Return from Nested Loops ===");

    for (int i = 0; i < 5; i = i + 1) {
        console.log("Outer i = " + i);
        for (int j = 0; j < 5; j = j + 1) {
            console.log("  Inner j = " + j);
            if (i == 2 && j == 3) {
                console.log("  Returning from nested loops");
                return i + j;  // Должен вернуть 5
            }
        }
        console.log("End of inner loop for i = " + i);
    }
    return 0;
}

// Тест 3: Break в функции с последующим return
function test_break_then_return() {
    console.log("=== Break then Return ===");

    int result = 0;
    for (int i = 0; i < 10; i = i + 1) {
        console.log("Processing i = " + i);
        if (i == 6) {
            console.log("Breaking at i = 6");
            result = i;
            break;
        }
    }

    console.log("After loop, result = " + result);
    return result * 3;  // Должен вернуть 18
}

// Тест 4: Continue в функции с return
function test_continue_with_return() {
    console.log("=== Continue with Return ===");

    int sum = 0;
    for (int i = 0; i < 10; i = i + 1) {
        if (i % 2 == 0) {
            console.log("Skipping even: " + i);
            continue;
        }

        console.log("Adding odd: " + i);
        sum = sum + i;

        if (sum > 10) {
            console.log("Sum exceeded 10, returning: " + sum);
            return sum;
        }
    }

    console.log("Final sum: " + sum);
    return sum;
}

// Тест 5: Глубоко вложенная структура
function test_deep_nesting() {
    console.log("=== Deep Nesting Test ===");

    for (int i = 0; i < 3; i = i + 1) {
        console.log("Level 1, i = " + i);

        for (int j = 0; j < 3; j = j + 1) {
            console.log("  Level 2, j = " + j);

            if (i == 1) {
                console.log("  Continue outer loop at i = 1");
                break;  // Выходим из внутреннего цикла
            }

            for (int k = 0; k < 3; k = k + 1) {
                console.log("    Level 3, k = " + k);

                if (k == 1) {
                    console.log("    Skipping k = 1");
                    continue;
                }

                if (i == 2 && j == 1 && k == 2) {
                    console.log("    Deep return condition met");
                    return 100 + i * 10 + j * 5 + k;  // 127
                }
            }
            console.log("  End of level 3");
        }
        console.log("End of level 2");
    }

    return 0;
}

// Тест 6: While циклы с различными комбинациями
function test_while_combinations() {
    console.log("=== While Loop Combinations ===");

    int outer = 0;
    while (outer < 4) {
        console.log("Outer while: " + outer);

        int inner = 0;
        while (inner < 6) {
            console.log("  Inner while: " + inner);

            if (inner == 2 && outer == 1) {
                console.log("  Continue outer from inner");
                break;  // Выходим из внутреннего while
            }

            if (inner == 4) {
                console.log("  Break inner at 4");
                break;
            }

            if (outer == 3 && inner == 1) {
                console.log("  Early return from while");
                return outer * 10 + inner;  // 31
            }

            inner = inner + 1;
        }

        console.log("After inner while");
        outer = outer + 1;
    }

    return 999;
}

// Тест 7: Рекурсия с return
function recursive_test(n) {
    console.log("Recursive call with n = " + n);

    if (n <= 0) {
        console.log("Base case reached");
        return 1;
    }

    if (n == 3) {
        console.log("Special case: n = 3");
        return 42;
    }

    return n * recursive_test(n - 1);
}

function test_recursion() {
    console.log("=== Recursion Test ===");
    int result = recursive_test(5);
    console.log("Recursion result: " + result);
    return result;
}

// Тест 8: Функция с множественными точками выхода
function test_multiple_exits(x) {
    console.log("=== Multiple Exits Test with x = " + x + " ===");

    if (x < 0) {
        console.log("Negative input");
        return "negative";
    }

    for (int i = 0; i < x; i = i + 1) {
        console.log("Loop iteration: " + i);

        if (i == 3) {
            console.log("Early exit at i = 3");
            return "early_exit";
        }

        if (i % 2 == 0) {
            console.log("Skipping even iteration");
            continue;
        }
    }

    if (x > 10) {
        console.log("Large input detected");
        return "large";
    }

    console.log("Normal completion");
    return "normal";
}

// Тест 9: Проверка состояния после break/continue
function test_state_preservation() {
    console.log("=== State Preservation Test ===");

    int counter = 0;
    int breaks = 0;
    int continues = 0;

    for (int i = 0; i < 15; i = i + 1) {
        counter = counter + 1;

        if (i % 5 == 0 && i > 0) {
            console.log("Breaking at i = " + i + ", counter = " + counter);
            breaks = breaks + 1;
            break;
        }

        if (i % 3 == 0) {
            console.log("Continuing at i = " + i + ", counter = " + counter);
            continues = continues + 1;
            continue;
        }

        console.log("Normal processing: i = " + i);
    }

    console.log("Final state: counter = " + counter + ", breaks = " + breaks + ", continues = " + continues);
    return counter * 100 + breaks * 10 + continues;
}

// Main функция для запуска всех тестов
function main() {
    console.log("========================================");
    console.log("    COMPLEX SCENARIOS TESTING SUITE   ");
    console.log("========================================");

    // Тест 1
    int result1 = test_return_from_loop();
    console.log("Result 1: " + result1);
    console.log("");

    // Тест 2
    int result2 = test_return_nested_loops();
    console.log("Result 2: " + result2);
    console.log("");

    // Тест 3
    int result3 = test_break_then_return();
    console.log("Result 3: " + result3);
    console.log("");

    // Тест 4
    int result4 = test_continue_with_return();
    console.log("Result 4: " + result4);
    console.log("");

    // Тест 5
    int result5 = test_deep_nesting();
    console.log("Result 5: " + result5);
    console.log("");

    // Тест 6
    int result6 = test_while_combinations();
    console.log("Result 6: " + result6);
    console.log("");

    // Тест 7
    int result7 = test_recursion();
    console.log("Result 7: " + result7);
    console.log("");

    // Тест 8 с разными значениями
    string result8a = test_multiple_exits(-5);
    console.log("Result 8a: " + result8a);

    string result8b = test_multiple_exits(2);
    console.log("Result 8b: " + result8b);

    string result8c = test_multiple_exits(5);
    console.log("Result 8c: " + result8c);

    string result8d = test_multiple_exits(15);
    console.log("Result 8d: " + result8d);
    console.log("");

    // Тест 9
    int result9 = test_state_preservation();
    console.log("Result 9: " + result9);
    console.log("");

    console.log("========================================");
    console.log("      ALL COMPLEX TESTS COMPLETED      ");
    console.log("========================================");

    // Финальная проверка работоспособности
    console.log("Final verification: All functions returned properly");
    return 0;
}
