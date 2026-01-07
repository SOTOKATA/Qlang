#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\core\string.ql



class String: {
    private let _value = ___STRING_1___;

    const zeroChar = ___STRING_2___;

    
    function toString(): {
        return Object.toString(_value);
    }
    
    
    function ___create_from___(const obj):
        return String.new(obj);

    
    function ___operator_plus___(const obj1, const obj2):
        return String.new(obj1._value + obj2._value);

    
    function ___operator_star___(const obj1, const obj2): {
        let val = ___STRING_3___;

        for let i = ___NUMBER_0___; i < obj2._value; i = i + ___NUMBER_1___:
            val = val + obj1;

        return String.new(val);
    }

    
    function ___operator_slash___(const obj1, const obj2): {
        let val = ___STRING_4___;

        const index = obj1.length() / obj2._value;

        for let i = ___NUMBER_2___; i < index; i = i + ___NUMBER_3___:
            val = val + obj1.charAt(i);

        return String.new(val);
    }

    
    function ___operator_equal_equal___(const obj1, const obj2):
        return obj1._value == obj2._value;

    
    function ___operator_not_equal___(const obj1, const obj2):
        return obj1._value == obj2._value;

    
    function ___operator_greater_equal___(const obj1, const obj2): 
        return obj1.length() >= obj2.length();

    
    function ___operator_less_equal___(const obj1, const obj2): 
        return obj1.length() <= obj2.length();

    
    function ___operator_greater___(const obj1, const obj2): 
        return obj1.length() > obj2.length();

    
    function ___operator_less___(const obj1, const obj2): 
        return obj1.length() < obj2.length();

    function new(const<String> input): {
        if String.isString(input):
            _value = input.toString();
        else:
            _value = input;
    }

    function new(const<String> char, const<Number> count): {
        if (Number.isNumber(count) == false):
            Throw.exception(___STRING_5___);

        if count < ___NUMBER_4___: 
            Throw.exception(___STRING_6___);

        if String.new(char).length() < ___NUMBER_5___:
            Throw.exception(___STRING_7___);

        _value = _native(___STRING_8___, char, count);
    }

    function getPrimitive(const strOrPrimite, let allowOther = false): {
        if (Object.isNull(strOrPrimite)): {
            Throw.exception(___STRING_9___);
        }

        if (String.isString(strOrPrimite)): {
            
            return strOrPrimite.toString();
        }

        if (String.isPrimitive(strOrPrimite)): {
            return strOrPrimite;
        }

        if allowOther: {
            return strOrPrimite;
        }

        Throw.exception(___STRING_10___);
    }

    private function _checkIsCollection(let value): {
        if Array.isCollection(value) == false: {
            Throw.parceException(___STRING_11___);
        }
    }

    private function _checkIsArray(let value): {
        if Array.isArray(value) == false: {
            Throw.parceException(___STRING_12___);
        }
    }

    
    function append(let collection): {
        _checkIsCollection(collection);

        let result = ___STRING_13___;

        let arr = Array.new(collection);

        for let i = ___NUMBER_6___; i < arr.length(); i = i + ___NUMBER_7___: {
            result = result + arr.at(i);
        }

        return String.new(result);
    }

    function toLower(): {
        return String.new(_native(___STRING_14___, _str(_value)));
    }

    function toUpper(): {
        return String.new(_native(___STRING_15___, _str(_value)));
    }

    function isString(let value): {
        value =  _native(___STRING_16___, value);
        return value;
    }

    function isPrimitive(let value): {
        return _native(___STRING_17___, value);
    }

    function split(let<String> pattern): {
        return Array.new(_native(___STRING_18___, _str(_value), _str(pattern)));
    }

    function charAt(const<Number> index): {
        if length() <= index:
            Throw.exception(___STRING_19___);

        return _native(___STRING_20___, _str(_value), index);
    }

    function setAt(const<Number> index, const<String> replaceValue): {
        if length() <= index:
            Throw.exception(___STRING_21___);

        _value = _native(___STRING_22___, _str(_value), _str(replaceValue), index);
    }

    function join(let strArr, let<String> pattern): {
        if (Array.isCollection(strArr) == false) && (Array.isArray(strArr) == false):
            Throw.exception(___STRING_23___);

        if Array.isArray(strArr):
            strArr = strArr.getCollection();

        return String.new(_native(___STRING_24___, strArr, pattern));
    }

    
    function length():
        return _native(___STRING_25___, _str(_value));

    
    function isNullOrEmpty(let<String> str): {
        if (String.isPrimitive(str) == false) && (String.isString(str) == false):
            Throw.exception(___STRING_26___);

        if String.isString(str):
            str = str.toString();

        return _native(___STRING_27___, _str(str));
    }
    
    
    function isNullOrWhitespace(let<String> str): {
        if (String.isPrimitive(str) == false) && (String.isString(str) == false):
            Throw.exception(___STRING_28___);

        if String.isString(str): 
            str = str.toString();

        return _native(___STRING_29___, _str(str));
    }

    
    function trim():
        return String.new(_native(___STRING_30___, _str(_value)));

    
    function trimStart():
        return String.new(_native(___STRING_31___, _str(_value)));

    
    function trimEnd():
        return String.new(_native(___STRING_32___, _str(_value)));

    
    function subString(let<Number> startPos, let<Number> length): {
        if Number.isNumber(startPos) == false:
            Throw.exception(___STRING_33___);

        if Number.isNumber(length) == false:
            Throw.exception(___STRING_34___);

        if length() <= length:
            Throw.exception(___STRING_35___);

        return String.new(_native(___STRING_36___, _str(_value), startPos, length));
    }
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\core\object.ql


class Object: {
    function isNull(let obj):
        return _native(___STRING_38___, obj);

    function isSimplify(const val): 
        return _native(___STRING_39___, val);

    function toString(const obj = nameof(this)):
        return _native(___STRING_40___, obj);
}

function str(const obj):
    return Object.toString(obj);
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\core\number.ql




class Number: {
    
    const MIN_VALUE = ___STRING_42___;
    const MAX_VALUE = ___STRING_43___;

    
    function isNumber(let var): {
        return _native(___STRING_44___, var);
    }


    
    function randInt(let min, let max): {
        if (isNumber(min) == false) || (isNumber(max) == false): {
            Throw.exception(___STRING_45___);
        }

        
        min = Parser.asInt(min);
        max = Parser.asInt(max);

        if min >= max: {
            Throw.exception(___STRING_46___);
        }

        return Parser.asNumber(_native(___STRING_47___, min, max));
    }

    
    function toFixedInt(let float): {
        return toFixed(float, ___STRING_48___);
    }

    
    function toFixed(let number, let pattern): {
        return _native(___STRING_49___, number, _str(pattern));
    }
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\core\dictionary.ql


class Dictionary: {
    private let _keys;
    private let _values;

    function new(): {
        _keys = Array.new([]);
        _values = Array.new([]);
    }
    
    function toString(): {
        let str = _keys.toString().toString();
        str = str + _values.toString().toString();

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
            Throw.exception(___STRING_51___);
        }

        let index = _keys.indexOf(key);

        return _values.at(index);
    }
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\core\array.ql



class Array: {
    private let _value;

    
    function new(const<Collection> collection = []):
        _value = collection;

    function toString(): {
        let str = ___STRING_53___;

        if length() > ___NUMBER_8___: {
            for let i = ___NUMBER_9___; i < length(); i = i + ___NUMBER_10___:
                str = str + ___STRING_54___ + at(i) + ___STRING_55___;

            str = str.subString(___NUMBER_11___, str.length() - ___NUMBER_12___);

            str = str + ___STRING_56___;
        } else:
            str = ___STRING_57___;

        return str.toString();
    }

    function isArray(const var):
        return _native(___STRING_58___, var);

    function isCollection(let collection):
        return _native(___STRING_59___, collection);

    function getCollection():
        return _value;

    function contains(let item):
        return _native(___STRING_60___, _value, item);

    
    function push(let item):
        _native(___STRING_61___, _value, item);

    
    function clear():
        _value = _native(___STRING_62___);

    
    function at(const<Number> index): {
        index = Parser.asInt(index);
        return _native(___STRING_63___, _value, index);
    }

    
    function setAt(const<Number> index, let item): {
        index = Parser.asInt(index);

        _native(___STRING_64___, _value, index, item);
    }

    function insert(const<Number> index, let item): {
        index = Parser.asInt(index);
        _native(___STRING_65___, _value, index, item);
    }

    
    function removeAt(const<Number> index): {
        index = Parser.asInt(index);
        _native(___STRING_66___, _value, index);
    }

    
    function indexOf(let item):
        return _native(___STRING_67___, _value, item);

    
    function length():
        return _native(___STRING_68___, _value);
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\base\throw.ql




class Throw: {
    
    function exception(const<String> message): {
        _native(___STRING_70___, _str(message));
    }

    
    function nonImplementException(): {
        exception(___STRING_71___);
    }

    
    function parseException(const<String> error): {
        exception(___STRING_72___ + error);
    }

    
    function nullException(): {
        exception(___STRING_73___);
    }
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\base\regex.ql



class Regex: {
    function replace(const<String> input, const<String> pattern, const<String> replacement = ___STRING_75___): {
        return _native(___STRING_76___, input, pattern, replacement);
    }

    
    
    
    
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\base\parser.ql



class Parser: {
    static function asInt(const object): {
        return _native(___STRING_78___, object);
    }

    static function asFloat(const object): {
        return _native(___STRING_79___, object);
    }

    static function asString(const object): {
        return _native(___STRING_80___, object);
    }
    
    static function asNumber(const object): {
        return _native(___STRING_81___, object);
    }
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\base\math.ql



class Math: {
    function max(const<Number> a, const<Number> b): {
        if a > b:
            return a;
        return b;
    }

    function min(const<Number> a, const<Number> b): {
        if a < b:
            return a;
        return b;
    }

    function abs(const<Number> n): {
        return ___NUMBER_13___-n;
    }
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\base\vectors\vector2.ql



class Vector2: {
    private let _x;
    private let _y;

    function toString(): {
        return String.new(___STRING_84___ + _x + ___STRING_85___ + _y);
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
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\base\datetime\time.ql


class Time: {
    function wait(let<Number> millisec): {
        if Number.isNumber(millisec) == false: {
            Throw.parseException(___STRING_87___);
        }

        millisec = Parser.asInt(millisec);

        _native(___STRING_88___, millisec);
    }
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\base\include.ql


#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\meta\Meta.ql



class Meta extends Object: {
    private function _isClass(const object):
        return _native(___STRING_91___, object);

    function getMethodListOf(const object): {
        if (_isClass(object) == false):
            Throw.exception(___STRING_92___);

        return Array.new(_native(___STRING_93___, object));
    }

    function getVariableListOf(const object): {
        if (_isClass(object) == false):
            Throw.exception(___STRING_94___);

        return Array.new(_native(___STRING_95___, object));
    }

    function getInfoOf(const object): {
        if (_isClass(object) == false):
            Throw.exception(___STRING_96___);

        return Array.new(_native(___STRING_97___, object));
    }
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\lib\base\console.ql





class Console: {
    private function getStr(const message): {
        if (Object.isNull(message)):
            return ___STRING_99___;

        if (Object.isSimplify(message) == false): {
            if (Meta.getMethodListOf(message).contains(___STRING_100___)): 
                return message.toString();
            else: 
                return str(message);
        }

        return null;
    }

    
    function print(let message): {
        message = getStr(message);

        _native(___STRING_101___, _str(message));
    }

    
    function println(let message = ___STRING_102___): {
        message = getStr(message);
        _native(___STRING_103___, _str(___STRING_104___));

        _native(___STRING_105___, _str(message + ___STRING_106___));
    }

    
    function printVerbatim(let message): {
        message = getStr(message);

        _native(___STRING_107___, message);
    }

    
    function printlnVerbatim(let message = ___STRING_108___): {
        message = getStr(message);

        _native(___STRING_109___, message + _str(___STRING_110___));
    }

    
    function readln(): {
        return String.new(_native(___STRING_111___));
    }

    function readkey(const<Boolean> intercept = false): {
        return String.new(_native(___STRING_112___, intercept));
    }

    function isKeyAvailable(): {
        return _native(___STRING_113___);
    }

    function cursorVisible(const<Boolean> visible): {
        _native(___STRING_114___, visible);
    }

    
    function clear(): {
        _native(___STRING_115___);
    }

    
    function setCursorPosition(let<Number> x, let<Number> y): {
        x = Parser.asInt(x);
        y = Parser.asInt(y);

        _native(___STRING_116___, x, y);
    }

    
    function setForeColor(let<String> color):
        _native(___STRING_117___, color);
    
    
    function setBackColor(let<String> color):
        _native(___STRING_118___, color);

    
    function resetColors():
        _native(___STRING_119___);

    function width(): 
        return _native(___STRING_120___);

    function height(): 
        return _native(___STRING_121___);
}
#FILE C:\Users\sotok\Documents\Робочий Стіл\Estagio\Qlang\out\projectexample\main.ql



namespace base: {

}

namespace system: {
   class Console: {
        function WriteLine(const message): {
              Console.println(message);
        }
   }

   class Console2 extends Object: {
        function WriteLine(const message): {
              Console.println(message);
        }
   }

   const var = ___STRING_123___;

   function out():{
        Console.println(___STRING_124___);
   }
}

namespace system: {
    class OtherClass: {
        function DoSomething(): {
            Console.println(___STRING_125___);
        }
    }
}

function main(): {
    system::Console.WriteLine(___STRING_126___);
    
    
}
