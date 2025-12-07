







function main(): {
    

    let ex = ___STRING_0___;

    ex = ___STRING_1___;

    

    

    

    

    
    
    
    
    

    
    

    
    

    
    
    

    
    
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\console.ql

class Console: {
    
    function print(let message): {
        _native(___STRING_3___, _str(message));
    }

    
    function println(let message): {
        _native(___STRING_4___, _str(message + ___STRING_5___));
    }

    
    function printVerbatim(let message): {
        _native(___STRING_6___, message);
    }

    
    function printlnVerbatim(let message): {
        _native(___STRING_7___, message + _str(___STRING_8___));
    }

    
    function readln(): {
        return _native(___STRING_9___);
    }

    function readkey(let intercept): {
        return _native(___STRING_10___, intercept);
    }

    function isKeyAvailable(): {
        return _native(___STRING_11___);
    }

    function cursorVisible(let visible): {
        _native(___STRING_12___, visible);
    }

    
    function clear(): {
        _native(___STRING_13___);
    }

    
    function setCursorPosition(let x, let y): {
        _native(___STRING_14___, x, y);
    }

    
    function setForeColor(let color): {
        _native(___STRING_15___, color);
    }
    
    
    function setBackColor(let color): {
        _native(___STRING_16___, color);
    }
    
    function resetColors(): {
        _native(___STRING_17___);
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\include.ql


#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\datatypes\array.ql

class Array: {
    private let _value = _native(___STRING_20___);

    
    function new(let collection): {
        if Array.isCollection(collection) == false: {
            Throw.parseException(___STRING_21___);
        }
        _value = collection;
    }

    function toString(): {
        let str = ___STRING_22___;

        if length() > ___NUMBER_0___: {
            for let i = ___NUMBER_1___; i < length(); i = i + ___NUMBER_2___: {
                str = str + ___STRING_23___ + at(i) + ___STRING_24___;
            }

            str = String.new(str);

            str = str.subString(___NUMBER_3___, str.length() - ___NUMBER_4___);

            str = String.new(str.str() + ___STRING_25___);
        } else: {
            str = String.new(___STRING_26___);
        }

        

        return str;
    }

    function isArray(let collection): {
        return _native(___STRING_27___, collection);
    }

    function isCollection(let collection): {
        return _native(___STRING_28___, collection);
    }

    function getCollection(): {
        return _value;
    }

    function contains(let item): {
        return _native(___STRING_29___, _value, item);
    }

    
    function push(let item): {
        _native(___STRING_30___, _value, item);
    }

    
    function clear(): {
        _value = _native(___STRING_31___);
    }

    
    function at(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_32___);
        }

        return _native(___STRING_33___, _value, index);
    }

    
    function setAt(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_34___);
        }

        index = Number.toFixedInt(index);

        _native(___STRING_35___, _value, index, item);
    }

    function insert(let index, let item): {
        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_36___);
        }

        index = Number.toFixedInt(index);

        _native(___STRING_37___, _value, index, item);
    }

    
    function removeAt(let index): {
        index = Number.toFixedInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_38___);
        }

        _native(___STRING_39___, _value, index);
    }

    
    function indexOf(let item): {
        return _native(___STRING_40___, _value, item);
    }

    
    function length(): {
        return _native(___STRING_41___, _value);
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\datatypes\dictionary.ql


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
            Throw.exception(___STRING_43___);
        }

        let index = _keys.indexOf(key);

        return _values.at(index);
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\datatypes\include.ql

#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\datatypes\advanced\vector2.ql
class Vector2: {
    private let _x;
    private let _y;

    function toString(): {
        return String.new(___STRING_46___ + _x + ___STRING_47___ + _y);
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

#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\datatypes\number.ql


class Number: {
    
    const MIN_VALUE = ___STRING_49___;
    const MAX_VALUE = ___STRING_50___;

    
    function isNumber(let var): {
        return _native(___STRING_51___, var);
    }

    
    function randInt(let min, let max): {
        if (isNumber(min) == false) || (isNumber(max) == false): {
            Throw.exception(___STRING_52___);
        }

        
        min = Parser.asInt(min);
        max = Parser.asInt(max);

        if min >= max: {
            Throw.exception(___STRING_53___);
        }

        return Parser.asNumber(_native(___STRING_54___, min, max));
    }

    
    function toFixedInt(let float): {
        return toFixed(float, ___STRING_55___);
    }

    
    function toFixed(let number, let pattern): {
        return _native(___STRING_56___, number, _str(pattern));
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\datatypes\object.ql

class Object: {
    function isNull(let obj): {
        return _native(___STRING_58___, obj);
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\datatypes\string.ql



class String: {
    private let _value = ___STRING_60___;

    function new(let input): {
        _value = input;
    }

    function str(): {
        return _value;
    }

    private function _checkIsCollection(let value): {
        if Array.isCollection(value) == false: {
            Throw.parceException(___STRING_61___);
        }
    }

    private function _checkIsArray(let value): {
        if Array.isArray(value) == false: {
            Throw.parceException(___STRING_62___);
        }
    }

    
    function append(let collection): {
        _checkIsCollection(collection);

        let result = ___STRING_63___;

        let arr = Array.new(collection);

        for let i = ___NUMBER_5___; i < arr.length(); i = i + ___NUMBER_6___: {
            result = result + arr.at(i);
        }

        return String.new(result);
    }

    function toLower(): {
        return String.new(_native(___STRING_64___, _str(_value)));
    }

    function toUpper(): {
        return String.new(_native(___STRING_65___, _str(_value)));
    }

    function isString(let value): {
        return _native(___STRING_66___, value);
    }

    function isPrimitive(let value): {
        return _native(___STRING_67___, value);
    }

    function split(let pattern): {
        return Array.new(_native(___STRING_68___, _str(_value), pattern));
    }

    function join(let strArr, let pattern): {
        if (Array.isCollection(strArr) == false) && (Array.isArray(strArr) == false): {
            Throw.exception(___STRING_69___);
        }   


        if Array.isArray(strArr) == true: {
            strArr = strArr.getCollection();
        }

        return String.new(_native(___STRING_70___, strArr, pattern));
    }

    
    function length(): {
        return _native(___STRING_71___, _str(_value));
    }

    
    function isNullOrEmpty(let str): {
        if (String.isPrimitive(str) == false) && (String.isString(str) == false): {
            Throw.exception(___STRING_72___);
        }

        if String.isString(str): {
            str = str.str();
        }

        return _native(___STRING_73___, _str(str));
    }
    
    
    function isNullOrWhitespace(let str): {
        if (String.isPrimitive(str) == false) && (String.isString(str) == false): {
            Throw.exception(___STRING_74___);
        }

        if String.isString(str): {
            str = str.str();
        }

        return _native(___STRING_75___, _str(str));
    }

    
    function trim(): {
        return String.new(_native(___STRING_76___, _str(_value)));
    }

    
    function trimStart(): {
        return String.new(_native(___STRING_77___, _str(_value)));
    }

    
    function trimEnd(): {
        return String.new(_native(___STRING_78___, _str(_value)));
    }

    
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception(___STRING_79___);
        }

        if Number.isNumber(length) == false: {
            Throw.exception(___STRING_80___);
        }

        if length() <= length: {
            Throw.exception(___STRING_81___);
        }

        return String.new(_native(___STRING_82___, _str(_value), startPos, length));
    }
}

#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\datetime\time.ql
class Time: {
    function wait(let millisec): {
        if Number.isNumber(millisec) == false: {
            Throw.parseException(___STRING_84___);
        }

        millisec = Parser.asInt(millisec);

        _native(___STRING_85___, millisec);
    }
}

#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\parser.ql
class Parser: {
    static function asInt(let object): {
        return _native(___STRING_87___, object);
    }

    static function asFloat(let object): {
        return _native(___STRING_88___, object);
    }

    static function asString(let object): {
        return _native(___STRING_89___, object);
    }
    
    static function asNumber(let object): {
        return _native(___STRING_90___, object);
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\regex.ql


class Regex: {
    function replace(let input, let pattern, let replacement): {
        return _native(___STRING_92___, input, pattern, replacement);
    }

    function match(let input, let pattern): {
        return _native(___STRING_93___, input, pattern);
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\base\throw.ql

class Throw: {
    
    function exception(let message): {
        _native(___STRING_95___, _str(message));
    }

    
    function nonImplementException(): {
        exception(___STRING_96___);
    }

    
    function parseException(let error): {
        exception(___STRING_97___ + error);
    }
}

#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\filesystem\directory.ql


class Directory: {

    
    
    function exists(let path): {
        return _native(___STRING_99___, path);
    }

    
    function create(let path): {
        if exists(path) == true: {
            Throw.exception(___STRING_100___);
        }

        _native(___STRING_101___, path);
    }

    
    function remove(let path): {
        if exists(path) == false: {
            Throw.exception(___STRING_102___);
        }
        
        _native(___STRING_103___, path, true);
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\filesystem\file.ql


class File: {
    
    
    function exists(let path): {
        return _native(___STRING_105___, path);
    }

    
    function setContent(let path, let content): {
        if exists(path) == false: {
            Throw.exception(___STRING_106___ + path + ___STRING_107___);
        }

        _native(___STRING_108___, path, content);
    }

    
    function appendContent(let path, let content): {
        if exists(path) == false: {
            Throw.exception(___STRING_109___ + path + ___STRING_110___);
        }

        _native(___STRING_111___, path, content);
    }

    
    
    function getContent(let path): {
        if exists(path) == false: {
            Throw.exception(___STRING_112___ + path + ___STRING_113___);
        }

        return String.new(_native(___STRING_114___, path));
    }

    
    function create(let path): {
        _native(___STRING_115___, path);
    }

    
    function remove(let path): {
        if exists(path) == false: {
            Throw.exception(___STRING_116___ + path + ___STRING_117___);
        }

        _native(___STRING_118___, path);
    }
}
#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\filesystem\path.ql


class Path: {
    
    
    
    function combine(let arr): {
        if (Array.isCollection(arr) == false) && (Array.isArray(arr) == false): {
            Throw.exception(___STRING_120___);
        }

        if Array.isArray(arr) == true: {
            arr = arr.getCollection();
        }

        return String.new(_native(___STRING_121___, arr));
    }
    
    
    
    
    
    
}

#FILE D:\Projects\Console\Qlang\bin\Debug\net9.0\lib\special\basetest.ql


class BaseTest: {
    function runTest(): {
        _stringTest();

        _numberTest();

        _arrayTest();

        _dictionaryTest();

        _objectTest();

        _regexTest();

        _vectorTest();
    }

    private function printHeader(let str): {
        Console.setForeColor(___STRING_123___);
        Console.println(___STRING_124___ + str + ___STRING_125___);
        Console.resetColors();
    }

    
    private function _numberTest(): {
        let a = ___NUMBER_7___;
        let b = ___NUMBER_8___;

        printHeader(___STRING_126___);
        Console.println(___STRING_127___ + a + ___STRING_128___ + Number.isNumber(a));
        Console.println(___STRING_129___ + b + ___STRING_130___ + Number.isNumber(b) + ___STRING_131___);

        Console.println(a + ___STRING_132___ + b + ___STRING_133___ + (a + b));
        Console.println(a + ___STRING_134___ + b + ___STRING_135___ + (a - b));
        Console.println(a + ___STRING_136___ + b + ___STRING_137___ + (a * b));
        Console.println(a + ___STRING_138___ + b + ___STRING_139___ + (a / b));
        Console.println(a + ___STRING_140___ + b + ___STRING_141___ + (a % b));

        Console.println(___STRING_142___ + Number.MIN_VALUE);
        Console.println(___STRING_143___ + Number.MAX_VALUE);

        Console.println(___STRING_144___ + a + ___STRING_145___ + b + ___STRING_146___ + Number.randInt(a, b));

        Console.println(___STRING_147___ + b + ___STRING_148___ + Number.toFixed(b, ___STRING_149___));
        Console.println(___STRING_150___ + a + ___STRING_151___ + Number.toFixedInt(a));
    }

    private function _arrayTest(): {
        let collection = [ ___STRING_152___, ___STRING_153___, ___STRING_154___ ];
        let array = Array.new(collection);

        printHeader(___STRING_155___);

        Console.println(___STRING_156___ + Array.new(collection).toString().str() + ___STRING_157___ + Array.isCollection(collection));
        Console.println(___STRING_158___ + array + ___STRING_159___ + array.toString().str() + ___STRING_160___ + Array.isArray(array));

        Console.println(___STRING_161___ + array.toString().str());
        Console.println(___STRING_162___ + array.contains(___STRING_163___));
        array.push(___STRING_164___);
        Console.println(___STRING_165___ + array.toString().str());
        array.setAt(___NUMBER_9___, ___STRING_166___);
        Console.println(___STRING_167___ + array.toString().str());
        Console.println(___STRING_168___ + array.at(___NUMBER_10___));
        array.insert(___NUMBER_11___, ___STRING_169___);
        Console.println(___STRING_170___ + array.toString().str());
        Console.println(___STRING_171___ + array.indexOf(___STRING_172___));
        array.removeAt(___NUMBER_12___);
        Console.println(___STRING_173___ + array.toString().str());
        Console.println(___STRING_174___ + array.length());
        array.clear();
        Console.println(___STRING_175___ + array.toString().str());


    }

    private function _dictionaryTest(): {
        let dict = Dictionary.new();

        printHeader(___STRING_176___);

        dict.set(___STRING_177___, ___STRING_178___);
        Console.println(___STRING_179___ + dict.toString().str());
        Console.println(___STRING_180___ + dict.containsKey(___STRING_181___));
        Console.println(___STRING_182___ + dict.containsValue(___STRING_183___));
        Console.println(___STRING_184___ + dict.get(___STRING_185___));
        dict.clear();
        Console.println(___STRING_186___ + dict.toString().str());
    }

    private function _objectTest(): {
        printHeader(___STRING_187___);

        Console.println(___STRING_188___ + Object.isNull(___NUMBER_13___));
        Console.println(___STRING_189___ + Object.isNull(null));
    }

    private function _stringTest(): {
        let str = String.new(___STRING_190___);
        let primitive = ___STRING_191___;

        printHeader(___STRING_192___);

        Console.println(___STRING_193___ + primitive + ___STRING_194___ + String.isPrimitive(primitive));
        Console.println(___STRING_195___ + str + ___STRING_196___ + str.str() + ___STRING_197___ + String.isString(str));

        Console.println(___STRING_198___ + String.append([___STRING_199___, ___STRING_200___]).str());

        Console.println(___STRING_201___ + str.str() + ___STRING_202___);
        Console.println(___STRING_203___ + str.length() + ___STRING_204___);
        Console.println(___STRING_205___ + str.trim().str() + ___STRING_206___);
        Console.println(___STRING_207___ + str.trimStart().str() + ___STRING_208___);
        Console.println(___STRING_209___ + str.trimEnd().str() + ___STRING_210___);
        Console.println(___STRING_211___ + str.toLower().str() + ___STRING_212___);
        Console.println(___STRING_213___ + str.toUpper().str() + ___STRING_214___);
        Console.println(___STRING_215___ + str.isNullOrEmpty(str) + ___STRING_216___);
        Console.println(___STRING_217___ + str.isNullOrWhitespace(str) + ___STRING_218___);
        Console.println(___STRING_219___ + str.subString(___NUMBER_14___, ___NUMBER_15___).str() + ___STRING_220___);
        Console.println(___STRING_221___ + str.split(___STRING_222___).toString().str() + ___STRING_223___);
        Console.println(___STRING_224___ + String.join(str.split(___STRING_225___), ___STRING_226___).str() + ___STRING_227___);
    }
    

    
    private function _vectorTest(): {
        let vect = Vector2.new(___NUMBER_16___, ___NUMBER_17___);

        printHeader(___STRING_228___);

        Console.println(___STRING_229___ + vect.toString().str());
        Console.println(___STRING_230___ + vect.X());
        Console.println(___STRING_231___ + vect.Y());
        Console.println(___STRING_232___ + vect.equals(Vector2.new(___NUMBER_18___, ___NUMBER_19___)));
    }
    

    private function _regexTest(): {
        printHeader(___STRING_233___);

        Console.println(___STRING_234___ + Regex.replace(___STRING_235___, ___STRING_236___, ___STRING_237___));
    }
}
