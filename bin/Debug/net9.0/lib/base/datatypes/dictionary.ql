include "$lib/base"

class Dictionary: {
    private let _keys;
    private let _values;

    function new(): {
        _keys = Array.new([]);
        _values = Array.new([]);
    }
    
    function toString(): {
        let str = _keys.toString().str();
        str = str + _values.toString().str();

        return String.new(str);
    }

    function set(let key, let item): {
        if (_keys.contains(key) == true): {
            _values.setAt(get(key), item);
        }
        else: {
            _keys.push(key);
            _values.push(item);
        }
    }

    function containsKey(let key): {
        return _keys.contains(key);
    }

    function containsValue(let item): {
        return _values.contains(item);
    }

    function getKeys(): {
        return _keys;
    }

    function getValues(): {
        return _values;
    }

    function clear(): {
        _keys.clear();
        _values.clear();
    }

    function get(let key): {
        if (_keys.contains(key) == false): {
            Throw.exception("Key is not existent in dictionary");
        }

        let index = _keys.indexOf(key);

        return _values.at(index);
    }
}