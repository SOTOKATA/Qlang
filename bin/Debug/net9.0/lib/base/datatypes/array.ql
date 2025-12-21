include "$lib/base"

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

            str = String.new(str);

            str = str.subString(0, str.length() - 1);

            str = String.new(str.toString() + "]");
        } else:
            str = String.new("[]");

        return str;
    }

    function isArray(const var):
        return _native("lib.array.is_array", var);

    function isCollection(let collection):
        return _native("lib.array.is", collection);

    function getCollection():
        return _value;

    function contains(let item):
        return _native("lib.array.contains", _value, item);

    // Add element
    function push(let item):
        _native("lib.array.add", _value, item);

    // Clear array
    function clear():
        _value = _native("lib.array.create");

    // Get at index
    function at(const<Number> index): {
        index = Parser.asInt(index);
        return _native("lib.array.get", _value, index);
    }

    // Set at index
    function setAt(const<Number> index, let item): {
        index = Parser.asInt(index);

        _native("lib.array.set", _value, index, item);
    }

    function insert(const<Number> index, let item): {
        index = Parser.asInt(index);
        _native("lib.array.insert", _value, index, item);
    }

    // Remove at index
    function removeAt(const<Number> index): {
        index = Parser.asInt(index);
        _native("lib.array.remove_at", _value, index);
    }

    // Get index of item
    function indexOf(let item):
        return _native("lib.array.index_of", _value, item);

    // Get length
    function length():
        return _native("lib.array.count", _value);
}