import "$lib/core"

class Dictionary: {
    private let _keys;
    private let _values;

    function new(): {
        _keys = Array.new([]);
        _values = Array.new([]);
    }
    
    function<String> toString(): {
        let str = _keys.toString().toString();
        str = str + _values.toString().toString();

        return String.new(str);
    }

    function<Array> getKeys(): return _keys;
    function<Array> getValues(): return _values;

    function set(let key, let item): {
        if _keys.contains(key) == true:
            _values.setAt(get(key), item);
        else: {
            _keys.push(key);
            _values.push(item);
        }
    }

    function<Boolean> containsKey(let key):
        return _keys.contains(key);

    function<Boolean> containsValue(let item):
        return _values.contains(item);

    function<Array> getKeys():
        return _keys;

    function<Array> getValues():
        return _values;

    function clear(): {
        _keys.clear();
        _values.clear();
    }

    function<Number> length(): _keys.length();

    function get(let key): {
        let index = _keys.indexOf(key);

        if index == -1:
            std::Throw.message("Key is not existent in dictionary");

        return _values.at(index);
    }
}