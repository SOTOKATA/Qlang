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

    function isArray(let collection): {
        return _native("list_is", collection);
    }

    // Add element
    function push(let item): {
        _native("list_add", _value, item);
        // _value = _csharp(
        //     "var list = " + _value +
        //     "; list" + ".Add(" + _str(item) + ");" +
        //     "return list;"
        //     );
    }

    // Clear array
    function clear(): {
        _value = _native("list_clear", _value);
    }

    // Get at index
    function at(let index): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        // Console.println("List: " + _value);
        return _native("list_get", _value, index);
        // return _csharp(_value + "[" + index + "]");
    }

    // Set at index
    function setAt(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        index = Number.toInt(index);

        _native("list_set", _value, index, item);
        // _value = _csharp(
        //     "var list = " + _value +
        //     "; list" + "[" + index + "] = " + _str(item) +
        //     "; return list;"
        //     );
    }

    function insert(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        index = Number.toInt(index);

        _native("list_insert", _value, index, item);
    }

    // Remove at index
    function removeAt(let index): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        _native("list_remove_at", _value, index);
        // _value = _csharp(
        //     "var list = " + _value +
        //     ";  list.RemoveAt(" + index + "); " +
        //     "return list;"
        //     );
    }

    // Get length
    function length(): {
        return _native("list_count", _value);
        // return _csharp(
        //     "var list = " + _value +
        //     ";" +
        //     "return list.Count;"
        //     );
    }
}