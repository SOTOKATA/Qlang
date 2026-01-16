import "$lib/core"

// Dynamic array
class Array: {
    private let _value;

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
        index = Parser.asInt(index);
        return _native("std.array.get", _value, index);
    }

    // Set at index
    function setAt(const<Number> index, let item): {
        index = Parser.asInt(index);

        _native("std.array.set", _value, index, item);
    }

    function insert(const<Number> index, let item): {
        index = Parser.asInt(index);
        _native("std.array.insert", _value, index, item);
    }

    // Remove at index
    function removeAt(const<Number> index): {
        index = Parser.asInt(index);
        _native("std.array.remove_at", _value, index);
    }

    // Get index of item
    function indexOf(let item):
        return _native("std.array.index_of", _value, item);

    // Get length
    function length():
        return _native("std.array.count", _value);
}