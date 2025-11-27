
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
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_6___);
        }

        
        return _native(___STRING_7___, _value, index);
        
    }

    
    function setAt(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_8___);
        }

        index = Number.toInt(index);

        _native(___STRING_9___, _value, index, item);
        
        
        
        
        
    }

    function insert(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_10___);
        }

        index = Number.toInt(index);

        _native(___STRING_11___, _value, index, item);
    }

    
    function removeAt(let index): {
        index = Number.toInt(index);

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
    
    
    
    
    

    
    private let usings = ___STRING_15___;

    
    const let MIN_VALUE = ___STRING_16___;
    const let MAX_VALUE = ___STRING_17___;

    
    function isNumber(let var): {
        return _native(___STRING_18___, var);
        
    }

    
    function randInt(let min, let max): {
        if (isNumber(min) == false) || (isNumber(max) == false): {
            Throw.exception(___STRING_19___);
        }

        
        min = toInt(min);
        max = toInt(max);

        if min >= max: {
            Throw.exception(___STRING_20___);
        }

        return _native(___STRING_21___, min, max);
    }

    
    function toInt(let float): {
        return toFixed(float, ___STRING_22___);
    }

    
    function toFixed(let number, let pattern): {
        return _native(___STRING_23___, number, _str(pattern));
        
    }
}

class String: {
    private let _value = ___STRING_24___;

    function new(let input): {
        _value = input;
    }

    function toString(): {
        return _value;
    }

    
    private function _str(let str): {
        return ___STRING_25___ + _str(str) + ___STRING_26___;
    }

    
    function append(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parceException(___STRING_27___);
        }

        let result = ___STRING_28___;

        let arr = Array.new(collection);

        for let i = ___NUMBER_0___; i < arr.length(); i = i + ___NUMBER_1___: {
            result = result + arr.at(i);
        }

        return result;
    }

    
    function length(): {
        return _native(___STRING_29___, _str(_value));
    }

    
    function isNullOrEmpty(let str): {
        return _native(___STRING_30___, _str(str));
    }
    
    
    function isNullOrWhiteSpace(let str): {
        return _native(___STRING_31___, _str(str));
    }

    
    function trim(): {
        return _native(___STRING_32___, _str(_value));
    }

    
    function trimStart(): {
        return _native(___STRING_33___, _str(_value));
    }

    
    function trimEnd(): {
        return _native(___STRING_34___, _str(_value));
    }

    
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception(___STRING_35___);
        }

        if Number.isNumber(length) == false: {
            Throw.exception(___STRING_36___);
        }

        return _native(___STRING_37___, _str(_value), startPos, length);
    }
}

class Time: {
    function wait(let millisec): {
        if Number.isNumber(millisec) == false: {
            Throw.parseException(___STRING_38___);
        }

        millisec = Number.toInt(millisec);

        _native(___STRING_39___, millisec);
    }
}


class Console: {
    private let usings = ___STRING_40___;
    private let defFColor = ___STRING_41___;
    private let defBColor = ___STRING_42___;

    
    function print(let message): {
        _native(___STRING_43___, _str(message));
        
    }

    
    function println(let message): {
        print(message + ___STRING_44___);
    }

    
    function readln(): {
        return _native(___STRING_45___);
    }

    function readkey(let intercept): {
        return _native(___STRING_46___, intercept);
    }

    function isKeyAvailable(): {
        return _native(___STRING_47___);
    }

    function cursorVisible(let visible): {
        _native(___STRING_48___, visible);
    }

    
    function clear(): {
        _native(___STRING_49___);
    }

    
    function setCursorPosition(let x, let y): {
        _native(___STRING_50___, x, y);
    }

    
    function setForeColor(let color): {
        _native(___STRING_51___, color);
    }
    
    
    function setBackColor(let color): {
        _native(___STRING_52___, color);
    }
    
    function resetColors(): {
        setForeColor(defFColor);
        setBackColor(defBColor);
    }
}

class Math: {
    
    private function throwException(let num): {
        if Number.isNumber(num) == false: {
            Throw.exception(___STRING_53___ + num + ___STRING_54___);
        }
    }

    const let PI = ___NUMBER_2___;

    
    function abs(let num): {
        throwException(num);

        return ___NUMBER_3___ - num;
    }

    
    function sum(let num, let num2): {
        throwException(num);
        throwException(num2);

        return num + num2;
    }

    
    function sub(let num, let num2): {
        throwException(num);
        throwException(num2);

        return num - num2;
    }
        
    
    function mult(let num, let num2): {
        throwException(num);
        throwException(num2);

        return num * num2;
    }

    
    function div(let num, let num2): {
        throwException(num);
        throwException(num2);

        if num2 == ___NUMBER_4___: {
            Throw.exception(___STRING_55___);
        }
        return num / num2;
    }
}
class Object: {
    function toString(): {
        return ___STRING_56___;
    }
}
class Parser: {
    static function asInt(let object): {
        return _native(___STRING_57___, object);
    }

    static function asFloat(let object): {
        return _native(___STRING_58___, object);
    }

    static function asString(let object): {
        return _native(___STRING_59___, object);
    }
}

class Throw: {
    
    function exception(let message): {
        _native(___STRING_60___, _str(message));
    }

    
    function nonImplementException(): {
        exception(___STRING_61___);
    }

    
    function parseException(let error): {
        exception(___STRING_62___ + error);
    }
}


function main(): {
    Console.println(Parser.asFloat(___STRING_63___) + ___NUMBER_5___);
}
