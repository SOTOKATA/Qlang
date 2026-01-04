// Dynamic array
class Array: {
    private let _value = _native("list_create");

    // Create empty array
    function new(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parseException("argument is not collection");
        }
        _value = collection;
    }

    function toString(): {
        return _value;
    }

    function isArray(let collection): {
        return _native("list_is", collection);
    }

    function getCollection(): {
        return _value;
    }

    function contains(let item): {
        return _native("list_contains", _value, item);
    }

    // Add element
    function push(let item): {
        _native("list_add", _value, item);
    }

    // Clear array
    function clear(): {
        _value = _native("list_clear", _value);
    }

    // Get at index
    function at(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        return _native("list_get", _value, index);
    }

    // Set at index
    function setAt(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        index = Number.toFixedInt(index);

        _native("list_set", _value, index, item);
    }

    function insert(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        index = Number.toFixedInt(index);

        _native("list_insert", _value, index, item);
    }

    // Remove at index
    function removeAt(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        _native("list_remove_at", _value, index);
    }

    // Get index of item
    function indexOf(let item): {
        return _native("list_index_of", _value, item);
    }

    // Get length
    function length(): {
        return _native("list_count", _value);
    }
}