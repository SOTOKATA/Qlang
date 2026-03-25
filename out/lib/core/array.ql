import "$lib/standard"
using std;

const array = new Array();
// Dynamic array
class Array extends DataType: {
    private let<String|null> _type = null;

    // Create empty array
    function new(const<Collection|Array> arr = [], const<String|null> type = null): {
        _value = arr.getCollection();
    
        _type = type;
    }

    function<String> toString(): {
        let str = "[";

        if length() > 0: {
            for let i = 0; i < length(); i++:
                str = str + "'" + at(i) + "',";

            str = str.subString(0, str.length() - 1);

            str = str + "]";
        } else:
            str = "[]";

        return str.toString();
    }

    function join(const<String> char) => string.join(this, char);

    function<Boolean> isCollection(const collection) => _native("std", "array", "is", collection);

    function<Collection> getCollection() => _value;

    function<Boolean> contains(const item) => _native("std", "array", "contains", _value, item);

    // Add element
    function push(const item): {
        if _type != null && typeof(item) != _type:
            throw.message("Element type does not match array type.");

        _native("std", "array", "add", _value, item);
    }

    // Add element
    function pushMany(const<Array|Collection> items): {
        const args = items.getValue();

        if _type != null && args.any(fn(const i) => i != _type):
            throw.message("Element type does not match array type.");

        _native("std", "array", "add_range", _value, args);
    }

    // Clear array
    function clear():
        _value = _native("std", "array", "create");

    // Get at index
    function at(const<Number> index): {
        checkIndex(index);

        index = std::parser.asInt(index);

        return _native("std", "array", "get", _value, index);
    }

    // Set at index
    function setAt(const<Number> index, let item): {
        checkIndex(index);
        
        index = std::parser.asInt(index);

        if _type != null && typeof(item) != _type:
            throw.message("Element type does not match array type.");

        _native("std", "array", "set", _value, index, item);
    }

    function insert(const<Number> index, let item): {
        checkIndex(index);
        
        index = std::parser.asInt(index);

        if _type != null && typeof(item) != _type:
            throw.message("Element type does not match array type.");

        _native("std", "array", "insert", _value, index, item);
    }

    // Remove at index
    function removeAt(const<Number> index): {
        checkIndex(index);

        index = std::parser.asInt(index);
        _native("std", "array", "remove_at", _value, index);
    }

    // Get index of item
    function<Number> indexOf(let item) => _native("std", "array", "index_of", _value, item);

    // Get length
    function<Number> length() =>_native("std", "array", "count", _value);
    function count() => length();

    function forEach(const<Func> func): {
        const length = length();

        for let i = 0; i < length; i++:
            func(at(i));
    }

    function skip(const<Number> count): {
        if count <= 0:
            std::throw.message("Count must be more than 0");
        else if count > length():
            std::throw.message("Count must be less than array length");

        const length = length();

        const newArray = new Array([]);

        for let i = count; i < length; i++:
            newArray.push(at(i));

        return newArray;
    }

    function last() => at(length() - 1);
    
    function where(const<Func> func) => new linq::WhereEnumerable(new linq::ArraySource(this), func);
    function select(const<Func> func) => new linq::SelectEnumerable(new linq::ArraySource(this), func);
    function any(const<Func> func) => (new linq::ArraySource(this)).any(func);
    function all(const<Func> func) => (new linq::ArraySource(this)).all(func);

    function max() => (new linq::ArraySource(this)).max();
    function min() => (new linq::ArraySource(this)).min();
    function sum() => (new linq::ArraySource(this)).sum();

    function getIndexes() => _native("std", "array", "get_indexes", getCollection());

    private function checkIndex(const<Number> index):
        if index < 0 || index >= length():
            std::throw.message("Index is out of range.");
}