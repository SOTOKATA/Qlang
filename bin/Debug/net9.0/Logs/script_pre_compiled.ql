
class Array: {
    private let _value = _native(___STRING_0___);

    
    function new(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parseException(___STRING_1___);
        }
        _value = collection;
    }

    function isArray(let collection): {
        return _native(___STRING_2___, collection);
    }

    function getCollection(): {
        return _value;
    }

    function contains(let item): {
        return _native(___STRING_3___, _value, item);
    }

    
    function push(let item): {
        _native(___STRING_4___, _value, item);
    }

    
    function clear(): {
        _value = _native(___STRING_5___, _value);
    }

    
    function at(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_6___);
        }

        return _native(___STRING_7___, _value, index);
    }

    
    function setAt(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_8___);
        }

        index = Number.toFixedInt(index);

        _native(___STRING_9___, _value, index, item);
    }

    function insert(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_10___);
        }

        index = Number.toFixedInt(index);

        _native(___STRING_11___, _value, index, item);
    }

    
    function removeAt(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_12___);
        }

        _native(___STRING_13___, _value, index);
    }

    
    function length(): {
        return _native(___STRING_14___, _value);
    }
}


class Number: {
    
    const let MIN_VALUE = ___STRING_15___;
    const let MAX_VALUE = ___STRING_16___;

    
    function isNumber(let var): {
        return _native(___STRING_17___, var);
    }

    
    function randInt(let min, let max): {
        if (isNumber(min) == false) || (isNumber(max) == false): {
            Throw.exception(___STRING_18___);
        }

        
        min = toFixedInt(Parser.asNumber(min));
        max = toFixedInt(Parser.asNumber(max));

        if min >= max: {
            Throw.exception(___STRING_19___);
        }

        return _native(___STRING_20___, min, max);
    }

    
    function toFixedInt(let float): {
        return toFixed(float, ___STRING_21___);
    }

    
    function toFixed(let number, let pattern): {
        return _native(___STRING_22___, number, _str(pattern));
    }
}

class String: {
    private let _value = ___STRING_23___;

    function new(let input): {
        _value = input;
    }

    function toString(): {
        return _value;
    }

    
    function append(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parceException(___STRING_24___);
        }

        let result = ___STRING_25___;

        let arr = Array.new(collection);

        for let i = ___NUMBER_0___; i < arr.length(); i = i + ___NUMBER_1___: {
            result = result + arr.at(i);
        }

        return result;
    }

    
    function length(): {
        Console.print(_value);
        return _native(___STRING_26___, _str(_value));
    }

    
    function isNullOrEmpty(let str): {
        return _native(___STRING_27___, _str(str));
    }
    
    
    function isNullOrWhitespace(let str): {
        return _native(___STRING_28___, _str(str));
    }

    
    function trim(): {
        return _native(___STRING_29___, _str(_value));
    }

    
    function trimStart(): {
        return _native(___STRING_30___, _str(_value));
    }

    
    function trimEnd(): {
        return _native(___STRING_31___, _str(_value));
    }

    
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception(___STRING_32___);
        }

        if Number.isNumber(length) == false: {
            Throw.exception(___STRING_33___);
        }

        return _native(___STRING_34___, _str(_value), startPos, length);
    }
}

class Time: {
    function wait(let millisec): {
        if Number.isNumber(millisec) == false: {
            Throw.parseException(___STRING_35___);
        }

        millisec = Number.toFixedInt(millisec);

        _native(___STRING_36___, millisec);
    }
}


class Console: {
    private let defFColor = ___STRING_37___;
    private let defBColor = ___STRING_38___;

    
    function print(let message): {
        _native(___STRING_39___, _str(message));
    }

    
    function println(let message): {
        print(message + ___STRING_40___);
    }

    
    function readln(): {
        return _native(___STRING_41___);
    }

    function readkey(let intercept): {
        return _native(___STRING_42___, intercept);
    }

    function isKeyAvailable(): {
        return _native(___STRING_43___);
    }

    function cursorVisible(let visible): {
        _native(___STRING_44___, visible);
    }

    
    function clear(): {
        _native(___STRING_45___);
    }

    
    function setCursorPosition(let x, let y): {
        _native(___STRING_46___, x, y);
    }

    
    function setForeColor(let color): {
        _native(___STRING_47___, color);
    }
    
    
    function setBackColor(let color): {
        _native(___STRING_48___, color);
    }
    
    function resetColors(): {
        setForeColor(defFColor);
        setBackColor(defBColor);
    }
}
class Object: {
    function toString(): {
        return ___STRING_49___;
    }
}
class Parser: {
    static function asInt(let object): {
        return _native(___STRING_50___, object);
    }

    static function asFloat(let object): {
        return _native(___STRING_51___, object);
    }

    static function asString(let object): {
        return _native(___STRING_52___, object);
    }
    
    static function asNumber(let object): {
        return _native(___STRING_53___, object);
    }
}

class Throw: {
    
    function exception(let message): {
        _native(___STRING_54___, _str(message));
    }

    
    function nonImplementException(): {
        exception(___STRING_55___);
    }

    
    function parseException(let error): {
        exception(___STRING_56___ + error);
    }
}


class Cmd: {
    function pt(): {
        Console.println(___STRING_57___);
    }

    function get(): {
        return String.new(___STRING_58___);
    }
}

function main(): {
    Console.println(Cmd.get().length());
}
