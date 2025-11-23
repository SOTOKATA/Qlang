
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

    
    function push(let item): {
        _native(___STRING_3___, _value, item);
        
        
        
        
        
    }

    
    function clear(): {
        _value = _native(___STRING_4___, _value);
    }

    
    function at(let index): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_5___);
        }

        
        return _native(___STRING_6___, _value, index);
        
    }

    
    function setAt(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_7___);
        }

        index = Number.toInt(index);

        _native(___STRING_8___, _value, index, item);
        
        
        
        
        
    }

    function insert(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_9___);
        }

        index = Number.toInt(index);

        _native(___STRING_10___, _value, index, item);
    }

    
    function removeAt(let index): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_11___);
        }

        _native(___STRING_12___, _value, index);
        
        
        
        
        
    }

    
    function length(): {
        return _native(___STRING_13___, _value);
        
        
        
        
        
    }
}


class Number: {
    
    
    
    
    

    
    private let usings = ___STRING_14___;

    
    const let MIN_VALUE = ___STRING_15___;
    const let MAX_VALUE = ___STRING_16___;

    
    function isNumber(let var): {
        return _native(___STRING_17___, var);
        
    }

    
    function randInt(let min, let max): {
        if (isNumber(min) == false) || (isNumber(max) == false): {
            Throw.exception(___STRING_18___);
        }

        
        min = toInt(min);
        max = toInt(max);

        if min >= max: {
            Throw.exception(___STRING_19___);
        }

        return _native(___STRING_20___, min, max);
    }

    
    function toInt(let float): {
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

    
    private function _str(let str): {
        return ___STRING_24___ + _str(str) + ___STRING_25___;
    }

    
    function append(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parceException(___STRING_26___);
        }

        let result = ___STRING_27___;

        let arr = Array.new(collection);

        for let i = ___NUMBER_0___; i < arr.length(); i = i + ___NUMBER_1___: {
            result = result + arr.at(i);
        }

        return result;
    }

    
    function length(): {
        return _native(___STRING_28___, _str(_value));
    }

    
    function isNullOrEmpty(let str): {
        return _native(___STRING_29___, _str(str));
    }
    
    
    function isNullOrWhiteSpace(let str): {
        return _native(___STRING_30___, _str(str));
    }

    
    function trim(): {
        return _native(___STRING_31___, _str(_value));
    }

    
    function trimStart(): {
        return _native(___STRING_32___, _str(_value));
    }

    
    function trimEnd(): {
        return _native(___STRING_33___, _str(_value));
    }

    
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception(___STRING_34___);
        }

        if Number.isNumber(length) == false: {
            Throw.exception(___STRING_35___);
        }

        return _native(___STRING_36___, _str(_value), startPos, length);
    }
}

class Time: {
    function wait(let millisec): {
        if Number.isNumber(millisec) == false: {
            Throw.parseException(___STRING_37___);
        }

        millisec = Number.toInt(millisec);

        _native(___STRING_38___, millisec);
    }
}


class Console: {
    private let usings = ___STRING_39___;
    private let defFColor = ___STRING_40___;
    private let defBColor = ___STRING_41___;

    
    function print(let message): {
        _native(___STRING_42___, _str(message));
        
    }

    
    function println(let message): {
        print(message + ___STRING_43___);
    }

    
    function readln(): {
        return _native(___STRING_44___);
    }

    function readkey(let intercept): {
        return _native(___STRING_45___, intercept);
    }

    function isKeyAvailable(): {
        return _native(___STRING_46___);
    }

    function cursorVisible(let visible): {
        _native(___STRING_47___, visible);
    }

    
    function clear(): {
        _native(___STRING_48___);
    }

    
    function setCursorPosition(let x, let y): {
        _native(___STRING_49___, x, y);
    }

    
    function setForeColor(let color): {
        _native(___STRING_50___, color);
    }
    
    
    function setBackColor(let color): {
        _native(___STRING_51___, color);
    }
    
    function resetColors(): {
        setForeColor(defFColor);
        setBackColor(defBColor);
    }
}

class Math: {
    
    private function throwException(let num): {
        if Number.isNumber(num) == false: {
            Throw.exception(___STRING_52___ + num + ___STRING_53___);
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
            Throw.exception(___STRING_54___);
        }
        return num / num2;
    }
}
class Object: {
    function toString(): {
        return ___STRING_55___;
    }
}

class Throw: {
    
    function exception(let message): {
        _native(___STRING_56___, _str(message));
    }

    
    function nonImplementException(): {
        exception(___STRING_57___);
    }

    
    function parseException(let error): {
        exception(___STRING_58___ + error);
    }
}


function main(): {
    Console.println(___STRING_59___)
}
