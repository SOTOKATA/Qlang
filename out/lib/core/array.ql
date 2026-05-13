import "$lib/standard"
import "$lib/core/linq"
using std;

const array = new Array();
// Dynamic array
class Array extends DataType: {
    private let<String|null> _type = null;

    // Create empty array
    function new(<Collection|Array> arr = [], <String|null> type = null): {
        _value = arr.getCollection();
    
        _type = type;
    }

    function<Array> take(<Number> count): {
        const content = new Array();
        const len = std::math.min(length, count);

        for let i = 0; i < len; i++:
            content.push(at(i));

        return content;
    }
    
    function<String> toString(): {
        let str = "[";
        const len = length;

        for let i = 0; i < len; i++: {
            const item = at(i);
            str += switch item: {
                is String => `"{item}"`,
                default => item.toString()
            };

            if i + 1 < len:
                str += ", ";
        }

        str += "]";

        return str.toString();
    }

    function join(<String> char) => string.join(this, char);

    function<Boolean> isCollection(collection) => #std.Array.IsCollection(collection);

    function<Collection> getCollection() => _value;

    function<Boolean> contains(item) => #std.Array.Contains(_value, item);

    // Add element
    function push(item): {
        if _type is not null && typeof(item) != _type:
            throw.message("Element type does not match array type.");

        #std.Array.Add(_value, item);
    }

    // Add element
    function pushMany(<Array|Collection> items): {
        const args = items.getValue();

        if _type != null && args.any(fn(i) => i != _type):
            throw.message("Element type does not match array type.");

        #std.Array.AddRange(_value, args);
    }

    // Clear array
    function clear():
        _value = #std.Array.Create();

    // Get at index
    function at(<Number> index): {
        checkIndex(index);

        index = std::parser.asInt(index);

        return #std.Array.Get(_value, index);
    }

    // Set at index
    function setAt(<Number> index, let item): {
        checkIndex(index);
        
        index = std::parser.asInt(index);

        if _type != null && typeof(item) != _type:
            throw.message("Element type does not match array type.");

        #std.Array.Set(_value, index, item);
    }

    function insert(<Number> index, let item): {
        checkIndex(index);
        
        index = std::parser.asInt(index);

        if _type != null && typeof(item) != _type:
            throw.message("Element type does not match array type.");

        #std.Array.Insert(_value, index, item);
    }

    // Remove at index
    function removeAt(<Number> index): {
        checkIndex(index);

        index = std::parser.asInt(index);
        #std.Array.RemoveAt(_value, index);
    }

    // Get index of item
    function<Number> indexOf(let item) => #std.Array.IndexOf(_value, item);

    // Get length
    const length = field(_): {
        fn get() => #std.Array.Count(_value)
    };

    function forEach(<Func> func): {
        const len = length;

        for let i = 0; i < len; i++:
            func(at(i));
    }

    function skip(<Number> count): {
        if count <= 0:
            std::throw.message("Count must be more than 0");
        else if count > length:
            std::throw.message("Count must be less than array length");

        const len = length;

        const newArray = new Array([]);

        for let i = count; i < len; i++:
            newArray.push(at(i));

        return newArray;
    }

    function last() => at(length - 1);
    
    function where(<Func> func) => new linq::WhereEnumerable(new linq::ArraySource(this), func);
    function select(<Func> func) => new linq::SelectEnumerable(new linq::ArraySource(this), func);
    function any(<Func> func) => new linq::ArraySource(this).any(func);
    function all(<Func> func) => new linq::ArraySource(this).all(func);
    function<Number> index(item) => new linq::ArraySource(this).index(item);

    function<Number> max() => new linq::ArraySource(this).max();
    function<Number> min() => new linq::ArraySource(this).min();
    function<Number> sum() => new linq::ArraySource(this).sum();

    function getIndexes() => #std.Array.GetIndexes(getCollection());

    private function checkIndex(<Number> index):
        if index < 0 || index >= length:
            std::throw.message("Index is out of range.");
}