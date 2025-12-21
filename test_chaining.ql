include "$lib/base"

// Test method chaining on different types of expressions

// Test 1: Method chaining on parenthesized expressions
let result1 = (Console.readln()).toString();
Console.println("Test 1 - Parenthesized expression chaining: " + result1);

// Test 2: Method chaining on string references
let testString = "Hello World";
let result2 = ___STRING_0___.toString();
Console.println("Test 2 - String reference chaining: " + result2);

// Test 3: Method chaining on number references
let testNumber = 42;
let result3 = ___NUMBER_0___.toString();
Console.println("Test 3 - Number reference chaining: " + result3);

// Test 4: Complex method chaining on function results
let result4 = (Console.readln()).toString().toString();
Console.println("Test 4 - Multiple method chaining: " + result4);

// Test 5: Method chaining on array/collection
let testArray = [1, 2, 3];
let result5 = testArray.toString();
Console.println("Test 5 - Array method chaining: " + result5);

// Test 6: Method chaining with assignments
(Console.readln()).toString() = "test assignment";

Console.println("All tests completed!");
