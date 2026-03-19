import "$lib/core"

class Dictionary: {
    private let _keys;
    private let _values;

    function new(): {
        _keys = new Array([]);
        _values = new Array([]);
    }
    
    function<String> toString(): {
        let str = _keys.toString().toString();
        str = str + _values.toString().toString();

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

    function forEach(const<Func> func): {
        const length = length();

        for let i = 0; i < length; i++:
            func({
                const key = _keys.at(i),
                const value = _values.at(i)
            });
    }

    function forEach(const<Func> func): {
        const length = length();

        for let i = 0; i < length; i++:
            func({
                const key = _keys.at(i),
                const value = _values.at(i)
            });
    }

    function forEach(const<Func> func): {
        const length = length();

        let item = null;

        for let i = 0; i < length; i++:
            func({
                const key = _keys.at(i),
                const value = _values.at(i)
            });
    }

    function<Array> where(const<Func> func): {
        const length = length();

        const arr = new Array([]);

        for let i = 0; i < length; i++:
            if func({
                const key = _keys.at(i),
                const value = _values.at(i)
            }):
            arr.push(i);
        
        return arr;
    }

    function<Array> select(const<Func> func): {
        const length = length();

        const arr = new Array([]);

        for let i = 0; i < length; i++:
            arr.push(func({
                const key = _keys.at(i),
                const value = _values.at(i)
            }));
        
        return arr;
    }

    function<Array> firstOrDefault(const<Func> func): {
        const length = length();

        let value;

        for let i = 0; i < length; i++:
            if func({
                const key = _keys.at(i),
                const value = _values.at(i)
            }): {
                value = {
                    const key = _keys.at(i),
                    const value = _values.at(i)
                };

                return value;
            }
        
        return null;
    }
}