include "$lib/base"

class Dictionary: {
    private let _keys = [];
    private let _values = [];

    function new(): {
        _keys = [];
        _values = [];
    }

    function set(let key, let item): {
        if (_keys.contains(key)): {
            _values.setAt(get(key), item)
        }
        else: {
            _keys.push(key);
            _values.push(item);
        }
    }

    function get(let key): {
        if (!_keys.contains(key)): {
            Throw.exception("TODO: exception");
        }

        let index = _keys.indexOf(key);

        return _values.at(index);
    }
}