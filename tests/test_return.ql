// Тест 1: Простой return в функции
function test_simple_return() {
    console.log("Before return");
    return 42;
    console.log("After return - should not print");
}

// Тест 2: Return без значения
function test_empty_return() {
    console.log("Before empty return");
    return;
    console.log("After empty return - should not print");
}

// Тест 3: Return в условии
function test_conditional_return(x) {
    console.log("Start function");
    if (x > 10) {
        console.log("x is greater than 10");
        return "big";
    }
    console.log("x is not greater than 10");
    return "small";
}

// Тест 4: Return в цикле внутри функции
function test_return_in_loop() {
    console.log("Function start");
    for (int i = 0; i < 10; i = i + 1) {
        console.log("Loop iteration: " + i);
        if (i == 3) {
            console.log("Returning from loop");
            return i;
        }
    }
    console.log("After loop - should not print");
    return -1;
}

// Тест 5: Вложенные функции с return
function outer_function() {
    console.log("Outer function start");

    function inner_function() {
        console.log("Inner function");
        return "inner_result";
    }

    string result = inner_function();
    console.log("Inner result: " + result);
    return "outer_result";
}

// Main функция для запуска тестов
function main() {
    console.log("=== Testing Return ===");

    // Тест 1
    console.log("\n--- Test 1: Simple Return ---");
    int result1 = test_simple_return();
    console.log("Result: " + result1);

    // Тест 2
    console.log("\n--- Test 2: Empty Return ---");
    test_empty_return();
    console.log("Empty return completed");

    // Тест 3
    console.log("\n--- Test 3: Conditional Return ---");
    string result3a = test_conditional_return(15);
    console.log("Result for 15: " + result3a);
    string result3b = test_conditional_return(5);
    console.log("Result for 5: " + result3b);

    // Тест 4
    console.log("\n--- Test 4: Return in Loop ---");
    int result4 = test_return_in_loop();
    console.log("Result: " + result4);

    // Тест 5
    console.log("\n--- Test 5: Nested Functions ---");
    string result5 = outer_function();
    console.log("Final result: " + result5);

    console.log("\n=== Return Tests Completed ===");
}
