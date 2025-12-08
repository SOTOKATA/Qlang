// Тест 1: Break в простом while цикле
function test_break_while() {
    console.log("=== Break in While Loop ===");
    int i = 0;
    while (i < 10) {
        console.log("Iteration: " + i);
        if (i == 3) {
            console.log("Breaking at i = 3");
            break;
        }
        i = i + 1;
    }
    console.log("After while loop");
}

// Тест 2: Continue в while цикле
function test_continue_while() {
    console.log("=== Continue in While Loop ===");
    int i = 0;
    while (i < 5) {
        i = i + 1;
        if (i == 3) {
            console.log("Skipping i = 3");
            continue;
        }
        console.log("Processing: " + i);
    }
    console.log("After while loop");
}

// Тест 3: Break в for цикле
function test_break_for() {
    console.log("=== Break in For Loop ===");
    for (int i = 0; i < 10; i = i + 1) {
        console.log("For iteration: " + i);
        if (i == 4) {
            console.log("Breaking at i = 4");
            break;
        }
    }
    console.log("After for loop");
}

// Тест 4: Continue в for цикле
function test_continue_for() {
    console.log("=== Continue in For Loop ===");
    for (int i = 0; i < 6; i = i + 1) {
        if (i == 2 || i == 4) {
            console.log("Skipping i = " + i);
            continue;
        }
        console.log("Processing: " + i);
    }
    console.log("After for loop");
}

// Тест 5: Вложенные циклы с break
function test_nested_break() {
    console.log("=== Nested Loops with Break ===");
    for (int i = 0; i < 3; i = i + 1) {
        console.log("Outer loop: " + i);
        for (int j = 0; j < 5; j = j + 1) {
            console.log("  Inner loop: " + j);
            if (j == 2) {
                console.log("  Breaking inner loop at j = 2");
                break;
            }
        }
        console.log("After inner loop");
    }
    console.log("After outer loop");
}

// Тест 6: Вложенные циклы с continue
function test_nested_continue() {
    console.log("=== Nested Loops with Continue ===");
    for (int i = 0; i < 3; i = i + 1) {
        console.log("Outer loop: " + i);
        for (int j = 0; j < 4; j = j + 1) {
            if (j == 1) {
                console.log("  Skipping inner j = 1");
                continue;
            }
            console.log("  Inner processing: " + j);
        }
        console.log("After inner loop");
    }
    console.log("After outer loop");
}

// Тест 7: Break и continue в одном цикле
function test_mixed_break_continue() {
    console.log("=== Mixed Break and Continue ===");
    int i = 0;
    while (i < 10) {
        i = i + 1;
        if (i == 3 || i == 5) {
            console.log("Skipping: " + i);
            continue;
        }
        if (i == 8) {
            console.log("Breaking at: " + i);
            break;
        }
        console.log("Processing: " + i);
    }
    console.log("After mixed loop");
}

// Тест 8: Break/continue в условиях
function test_conditional_break_continue() {
    console.log("=== Conditional Break/Continue ===");
    for (int i = 0; i < 10; i = i + 1) {
        console.log("Checking: " + i);

        if (i % 2 == 0) {
            if (i == 6) {
                console.log("Breaking on even 6");
                break;
            } else {
                console.log("Skipping even: " + i);
                continue;
            }
        }

        console.log("Processing odd: " + i);
    }
    console.log("After conditional loop");
}

// Тест 9: Break/continue с неправильным использованием (вне циклов)
function test_invalid_break_continue() {
    console.log("=== Testing Invalid Usage ===");

    // Этот break должен вызвать ошибку или игнорироваться
    if (true) {
        console.log("Before invalid break");
        // break; // Раскомментируйте для тестирования ошибки
        console.log("After where break would be");
    }

    // Этот continue должен вызвать ошибку или игнорироваться
    if (true) {
        console.log("Before invalid continue");
        // continue; // Раскомментируйте для тестирования ошибки
        console.log("After where continue would be");
    }

    console.log("Invalid usage test completed");
}

// Main функция
function main() {
    console.log("=== Testing Break and Continue ===");

    test_break_while();
    console.log("");

    test_continue_while();
    console.log("");

    test_break_for();
    console.log("");

    test_continue_for();
    console.log("");

    test_nested_break();
    console.log("");

    test_nested_continue();
    console.log("");

    test_mixed_break_continue();
    console.log("");

    test_conditional_break_continue();
    console.log("");

    test_invalid_break_continue();
    console.log("");

    console.log("=== All Break/Continue Tests Completed ===");
}
