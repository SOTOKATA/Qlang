import "$lib/core"
import "$lib/meta"

class Dictionary: {
    private let _keys;
    private let _values;

    function new(): {
        _keys = new Array([]);
        _values = new Array([]);
    }

    function new(const<Object|null> obj = null): {
        _keys = new Array([]);
        _values = new Array([]);

        if typeof(obj) != "null":
            meta::getVariableList(obj).forEach(fn(var): {
                _keys.push(var.name);
                _values.push(var.value);
            });
    }
    
    function<String> toString(): {
        let str = "[";

        const length = _keys.length();
        for let i = 0; i < length; i++: {
            const value = _values.at(i);

            str += _keys.at(i).toString() + " => " + switch typeof(value): {
                "String" => `"{value}"`,
                default => value.toString()
            };
        
            if i + 1 < length:
                str += ", ";
        }

        str += "]";
        
        return new String(str);
    }

    function<Array> getKeys() => _keys;
    function<Array> getValues() => _values;

    function set(let key, let item): {
        if _keys.contains(key) == true:
            _values.setAt(get(key), item);
        else: {
            _keys.push(key);
            _values.push(item);
        }
    }

    function<Boolean> containsKey(let key) => _keys.contains(key);

    function<Boolean> containsValue(let item) => _values.contains(item);

    function<Array> getKeys() => _keys;

    function<Array> getValues() => _values;

    function clear(): {
        _keys.clear();
        _values.clear();
    }

    function<Number> length(): _keys.length();

    function get(let key): {
        let index = _keys.indexOf(key);

        if index == -1:
            std::throw.message("Key is not existent in dictionary");

        return _values.at(index);
    }

    function where(<Func> func) => 
        new linq::WhereEnumerable(new linq::DictionarySource(this), func);
        
    function select(<Func> func) => 
        new linq::SelectEnumerable(new linq::DictionarySource(this), func);

    function any(<Func> func) => 
        (new linq::DictionarySource(this)).any(func);

    function all(<Func> func) => 
        (new linq::DictionarySource(this)).all(func);
}