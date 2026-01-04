

function main(): {
    Console.println(___STRING_0___ + (___NUMBER_0___ * ___NUMBER_1___ + ___NUMBER_2___));
    Console.println(___STRING_1___ + (___NUMBER_3___ * (___NUMBER_4___ + ___NUMBER_5___)));
}

class Console: {
    
    function print(let message): {
        _native(___STRING_2___, _str(message));
    }

    
    function println(let message): {
        _native(___STRING_3___, _str(message + ___STRING_4___));
    }

    
    function readln(): {
        return _native(___STRING_5___);
    }

    function readkey(let intercept): {
        return _native(___STRING_6___, intercept);
    }

    function isKeyAvailable(): {
        return _native(___STRING_7___);
    }

    function cursorVisible(let visible): {
        _native(___STRING_8___, visible);
    }

    
    function clear(): {
        _native(___STRING_9___);
    }

    
    function setCursorPosition(let x, let y): {
        _native(___STRING_10___, x, y);
    }

    
    function setForeColor(let color): {
        _native(___STRING_11___, color);
    }
    
    
    function setBackColor(let color): {
        _native(___STRING_12___, color);
    }
    
    function resetColors(): {
        _native(___STRING_13___);
    }
}


class Parser: {
    static function asInt(let object): {
        return _native(___STRING_14___, object);
    }

    static function asFloat(let object): {
        return _native(___STRING_15___, object);
    }

    static function asString(let object): {
        return _native(___STRING_16___, object);
    }
    
    static function asNumber(let object): {
        return _native(___STRING_17___, object);
    }
}

class Throw: {
    
    function exception(let message): {
        _native(___STRING_18___, _str(message));
    }

    
    function nonImplementException(): {
        exception(___STRING_19___);
    }

    
    function parseException(let error): {
        exception(___STRING_20___ + error);
    }
}


class Array: {
    private let _value = _native(___STRING_21___);

    
    function new(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parseException(___STRING_22___);
        }
        _value = collection;
    }

    function toString(): {
        return _value;
    }

    function isArray(let collection): {
        return _native(___STRING_23___, collection);
    }

    function getCollection(): {
        return _value;
    }

    function contains(let item): {
        return _native(___STRING_24___, _value, item);
    }

    
    function push(let item): {
        _native(___STRING_25___, _value, item);
    }

    
    function clear(): {
        _value = _native(___STRING_26___, _value);
    }

    
    function at(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_27___);
        }

        return _native(___STRING_28___, _value, index);
    }

    
    function setAt(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_29___);
        }

        index = Number.toFixedInt(index);

        _native(___STRING_30___, _value, index, item);
    }

    function insert(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_31___);
        }

        index = Number.toFixedInt(index);

        _native(___STRING_32___, _value, index, item);
    }

    
    function removeAt(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_33___);
        }

        _native(___STRING_34___, _value, index);
    }

    
    function indexOf(let item): {
        return _native(___STRING_35___, _value, item);
    }

    
    function length(): {
        return _native(___STRING_36___, _value);
    }
}


class Dictionary: {
    private let _keys;
    private let _values;

    function new(): {
        _keys = Array.new([]);
        _values = Array.new([]);
    }

    function toString(): {
        return ___STRING_37___;
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

    function get(let key): {
        if (_keys.contains(key) == false): {
            Throw.exception(___STRING_38___);
        }

        let index = _keys.indexOf(key);

        return _values.at(index);
    }
}



class Number: {
    
    const let MIN_VALUE = ___STRING_39___;
    const let MAX_VALUE = ___STRING_40___;

    
    function isNumber(let var): {
        return _native(___STRING_41___, var);
    }

    
    function randInt(let min, let max): {
        if (isNumber(min) == false) || (isNumber(max) == false): {
            Throw.exception(___STRING_42___);
        }

        
        min = Parser.asInt(min);
        max = Parser.asInt(max);

        if min >= max: {
            Throw.exception(___STRING_43___);
        }

        return Parser.asNumber(_native(___STRING_44___, min, max));
    }

    
    function toFixedInt(let float): {
        return toFixed(float, ___STRING_45___);
    }

    
    function toFixed(let number, let pattern): {
        return _native(___STRING_46___, number, _str(pattern));
    }
}

class String: {
    private let _value = ___STRING_47___;

    function new(let input): {
        _value = input;
    }

    function toString(): {
        return _value;
    }

    
    function append(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parceException(___STRING_48___);
        }

        let result = ___STRING_49___;

        let arr = Array.new(collection);

        for let i = ___NUMBER_6___; i < arr.length(); i = i + ___NUMBER_7___: {
            result = result + arr.at(i);
        }

        return result;
    }

    
    function length(): {
        return _native(___STRING_50___, _str(_value));
    }

    
    function isNullOrEmpty(let str): {
        return _native(___STRING_51___, _str(str));
    }
    
    
    function isNullOrWhitespace(let str): {
        return _native(___STRING_52___, _str(str));
    }

    
    function trim(): {
        return _native(___STRING_53___, _str(_value));
    }

    
    function trimStart(): {
        return _native(___STRING_54___, _str(_value));
    }

    
    function trimEnd(): {
        return _native(___STRING_55___, _str(_value));
    }

    
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception(___STRING_56___);
        }

        if Number.isNumber(length) == false: {
            Throw.exception(___STRING_57___);
        }

        return _native(___STRING_58___, _str(_value), startPos, length);
    }
}

class Vector2: {
    private let _x;
    private let _y;

    function toString(): {
        return ___STRING_59___ + _x + ___STRING_60___ + _y;
    }

    function equals(let other): {
        if (other.X() == _x) && (other.Y() == _y): {
            return true;
        }
        return false;
    }

    function new(let x, let y): {
        _x = Parser.asNumber(x);
        _y = Parser.asNumber(y);
    }

    function X(): {
        return Parser.asNumber(_x);
    }

    function Y(): {
        return Parser.asNumber(_y);
    }
}

class Time: {
    function wait(let millisec): {
        if Number.isNumber(millisec) == false: {
            Throw.parseException(___STRING_61___);
        }

        millisec = Parser.asInt(millisec);

        _native(___STRING_62___, millisec);
    }
}

