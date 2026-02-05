import "$lib/core"

// Dynamic array
class Array extends DataType: {
    // Create empty array
    function new(const<Collection> collection = []):
        _value = collection;

    function toString(): {
        let str = "[";

        if length() > 0: {
            for let i = 0; i < length(); i = i + 1:
                str = str + "'" + at(i) + "',";

            str = str.subString(0, str.length() - 1);

            str = str + "]";
        } else:
            str = "[]";

        return str.toString();
    }

    function isArray(const var):
        return _native("std.array.is_array", var);

    function isCollection(const collection):
        return _native("std.array.is", collection);

    function getCollection():
        return _value;

    function contains(const item):
        return _native("std.array.contains", _value, item);

    // Add element
    function push(const item):
        _native("std.array.add", _value, item);

    // Clear array
    function clear():
        _value = _native("std.array.create");

    // Get at index
    function at(const<Number> index): {
        checkIndex(index);

        index = std::Parser.asInt(index);
        return _native("std.array.get", _value, index);
    }

    // Set at index
    function setAt(const<Number> index, let item): {
        checkIndex(index);
        
        index = std::Parser.asInt(index);

        _native("std.array.set", _value, index, item);
    }

    function insert(const<Number> index, let item): {
        checkIndex(index);
        
        index = std::Parser.asInt(index);
        _native("std.array.insert", _value, index, item);
    }

    // Remove at index
    function removeAt(const<Number> index): {
        checkIndex(index);

        index = std::Parser.asInt(index);
        _native("std.array.remove_at", _value, index);
    }

    // Get index of item
    function indexOf(let item):
        return _native("std.array.index_of", _value, item);

    // Get length
    function length():
        return _native("std.array.count", _value);
    function count(): return length();

    function forEach(const func): {
        const length = length();
        for let i = 0; i < length; i = i + 1:
            func(at(i));
    }

    function skip(const<Number> count): {
        if count <= 0:
            Throw.exception("Count must be more than 0");
        else if count > length():
            Throw.exception("Count must be less than array length");

        const length = length();

        const newArray = Array.new([]);

        for let i = count; i < length; i = i + 1:
            newArray.push(at(i));

        return newArray;
    }

    function last(): {
        return at(length() - 1);
    }

    function where(const func): {
        const length = length();

        const arr = Array.new([]);

        for let i = 0; i < length; i = i + 1: {
            if func(at(i)):
                arr.push(at(i));
        }

        return arr;
    }

    function select(const func): {
        const length = length();

        const arr = Array.new([]);

        for let i = 0; i < length; i = i + 1:
            arr.push(func(at(i)));

        return arr;
    }

    function count(const func): {
        const length = length();
        let count = 0;

        for let i = 0; i < length; i = i + 1:
            if func(at(i)):
                count = count + 1;

        return count;
    }

    function firstOrDefault(const func): {
        const length = length();

        for let i = 0; i < length; i = i + 1: {
            const boolResult = func(at(i));

            if boolResult:
                return at(i);
        }

        return null;
    }

    private function checkIndex(const<Number> index): {
        if (index < 0 || index >= length()):
            std::Throw.exception("Index is out of range.");
    }
}