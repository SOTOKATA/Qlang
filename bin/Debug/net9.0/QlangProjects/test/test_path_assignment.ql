include "$lib/base"

class Example: {
    let var = "initial value";
    let nested = null;
}

class NestedObject: {
    let property = "nested property";
}

function main(): {
    // Test 1: Simple class property assignment
    let ex = Example.new();
    Console.println("Before assignment: " + ex.var);

    ex.var = "updated value";
    Console.println("After assignment: " + ex.var);

    // Test 2: Nested object assignment
    let nested = NestedObject.new();
    ex.nested = nested;

    Console.println("Before nested assignment: " + ex.nested.property);
    ex.nested.property = "updated nested property";
    Console.println("After nested assignment: " + ex.nested.property);

    // Test 3: Multiple level path assignment
    let container = Example.new();
    container.nested = Example.new();
    container.nested.var = "deeply nested value";
    Console.println("Deep nested value: " + container.nested.var);

    Console.println("All path assignment tests completed successfully!");
}
