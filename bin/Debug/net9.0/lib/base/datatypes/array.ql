include "$lib/base"

// Dynamic array
class Array: {
    private let _value = _native("lib.list_create");

    // Create empty array
    function new(let collection): {
        if Array.isCollection(collection) == false: {
            Throw.parseException("argument is not collection");
        }
        _value = collection;
    }

    function toString(): {
        let str = "[";

        if length() > 0: {
            for let i = 0; i < length(); i = i + 1: {
                str = str + "'" + at(i) + "',";
            }

            str = String.new(str);

            str = str.subString(0, str.length() - 1);

            str = String.new(str.str() + "]");
        } else: {
            str = String.new("[]");
        }

        return str;
    }

    function isArray(let collection): {
        return _native("lib.list_is_array", collection);
    }

    function isCollection(let collection): {
        return _native("lib.list_is", collection);
    }

    function getCollection(): {
        return _value;
    }

    function contains(let item): {
        return _native("lib.list_contains", _value, item);
    }

    // Add element
    function push(let item): {
        _native("lib.list_add", _value, item);
    }

    // Clear array
    function clear(): {
        _value = _native("lib.list_create");
    }

    // Get at index
    function at(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        return _native("lib.list_get", _value, index);
    }

    // Set at index
    function setAt(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        index = Number.toFixedInt(index);

        _native("lib.list_set", _value, index, item);
    }

    function insert(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        index = Number.toFixedInt(index);

        _native("lib.list_insert", _value, index, item);
    }

    // Remove at index
    function removeAt(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        _native("lib.list_remove_at", _value, index);
    }

    // Get index of item
    function indexOf(let item): {
        return _native("lib.list_index_of", _value, item);
    }

    // Get length
    function length(): {
        return _native("lib.list_count", _value);
    }
}