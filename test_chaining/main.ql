// Test array and collection method chaining that should now work with the new parser

class ArrayUtils: {
    function process(let arr): {
        return "processed array";
    }

    function getLength(let arr): {
        return 3; // dummy implementation
    }

    function getFirst(let arr): {
        return "first element";
    }
}

class CollectionHelper: {
    function transform(let collection): {
        return "transformed";
    }

    function filter(let collection): {
        return [1, 2];
    }
}

function main(): {
    // Test 1: Method chaining on array literals
    let test1 = [1, 2, 3].toString();

    // Test 2: Method chaining on arrays in parentheses
    let test2 = ([1, 2, 3]).toString();

    // Test 3: Complex array chaining with parentheses
    let test3 = (["hello", "world"]).toString();

    // Test 4: Array as function argument with chaining
    let test4 = ArrayUtils.process([1, 2, 3]);

    // Test 5: Chaining on array in parentheses with function call
    let test5 = ArrayUtils.getLength(([1, 2, 3, 4]));

    // Test 6: Multi-dimensional array chaining
    let test6 = [[1, 2], [3, 4]].toString();

    // Test 7: Array with mixed types
    let test7 = [1, "hello", 3.14].toString();

    // Test 8: Complex parentheses with array chaining
    let test8 = ((["test", "array"])).toString();

    // Test 9: Array chaining in complex expressions
    let test9 = ArrayUtils.process([1, 2]) + ArrayUtils.process([3, 4]);

    // Test 10: Nested function calls with array chaining
    let test10 = CollectionHelper.transform(([1, 2, 3]));

    // Test 11: Array variable with chaining
    let myArray = [10, 20, 30];
    let test11 = myArray.toString();

    // Test 12: Chaining on result of array operations
    let test12 = (myArray).toString();

    // Test 13: Empty array chaining
    let test13 = [].toString();

    // Test 14: Empty array in parentheses with chaining
    let test14 = ([]).toString();

    // Test 15: Complex nested array operations
    let test15 = CollectionHelper.filter([1, 2, 3, 4, 5]);

    return test1;
}
