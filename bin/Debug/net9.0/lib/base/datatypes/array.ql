// Dynamic array
class Array: {
    private let _value = _csharp("new List<object>()");

    // Create empty array
    function new(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parseException("argument is not collection");
        }
        _value = collection;
    }

    function isArray(let collection): {
        return _csharp(collection + " is List<object>");
    }

    // Add element
    function push(let item): {
        _value = _csharp(
            "var list = " + _value +
            "; list" + ".Add(" + _str(item) + ");" +
            "return list;"
            );
    }

    // Clear array
    function clear(): {
        _value = _csharp("new List<object>()");
    }

    // Get at index
    function at(let index): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        return _csharp(_value + "[" + index + "]");
    }

    // Set at index
    function setAt(let index, let item): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        _value = _csharp(
            "var list = " + _value +
            "; list" + "[" + index + "] = " + _str(item) +
            "; return list;"
            );
    }

    // Remove at index
    function removeAt(let index): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception("index is not number");
        }

        _value = _csharp(
            "var list = " + _value +
            ";  list.RemoveAt(" + index + "); " +
            "return list;"
            );
    }

    // Get length
    function length(): {
        return _csharp(
            "var list = " + _value +
            ";" +
            "return list.Count;"
            );
    }
}