include "$lib/special/snake"

function main(): {
    const str = String.new("Hello World!");

    Console.println((str == "Hello World!"));
    Console.println((str != "Hello World!"));
    Console.println((str <= "Hello World!"));
    Console.println((str >= "Hello World!"));
    Console.println((str > "Hello World!"));
    Console.println((str < "Hello World!"));
}

class Form: {
    private let _val;

    function new(const val):
        _val = val;

    function toString():
        return _val;

    function ___create_from___(const obj):
        return Form.new(obj);

    function ___operator_equal_equal___(const obj1, const obj2):
        return obj1._val == obj2._val;
}