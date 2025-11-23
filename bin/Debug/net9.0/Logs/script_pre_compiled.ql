
class Array: {
    private let _value = _csharp(___STRING_0___);

    
    function new(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parseException(___STRING_1___);
        }
        _value = collection;
    }

    function isArray(let collection): {
        return _csharp(collection + ___STRING_2___);
    }

    
    function push(let item): {
        _value = _csharp(
            ___STRING_3___ + _value +
            ___STRING_4___ + ___STRING_5___ + _str(item) + ___STRING_6___ +
            ___STRING_7___
            );
    }

    
    function clear(): {
        _value = _csharp(___STRING_8___);
    }

    
    function at(let index): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_9___);
        }

        return _csharp(_value + ___STRING_10___ + index + ___STRING_11___);
    }

    
    function setAt(let index, let item): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_12___);
        }

        _value = _csharp(
            ___STRING_13___ + _value +
            ___STRING_14___ + ___STRING_15___ + index + ___STRING_16___ + _str(item) +
            ___STRING_17___
            );
    }

    
    function removeAt(let index): {
        index = Number.toInt(index);

        if Number.isNumber(index) == false: {
            Throw.exception(___STRING_18___);
        }

        _value = _csharp(
            ___STRING_19___ + _value +
            ___STRING_20___ + index + ___STRING_21___ +
            ___STRING_22___
            );
    }

    
    function length(): {
        return _csharp(
            ___STRING_23___ + _value +
            ___STRING_24___ +
            ___STRING_25___
            );
    }
}


class Number: {
    
    
    
    
    

    
    private let usings = ___STRING_26___;

    
    const let MIN_VALUE = ___STRING_27___;
    const let MAX_VALUE = ___STRING_28___;

    
    function isNumber(let var): {
        return _csharp(usings + ___STRING_29___ + _str(var) + ___STRING_30___);
    }

    
    function randInt(let min, let max): {
        if (isNumber(min) == false) || (isNumber(max) == false): {
            Throw.exception(___STRING_31___);
        }

        
        min = toInt(min);
        max = toInt(max);

        if min >= max: {
            Throw.exception(___STRING_32___);
        }

        return _csharp(usings + ___STRING_33___ + min + ___STRING_34___ + max + ___STRING_35___);
    }

    
    function toInt(let float): {
        return toFixed(float, ___STRING_36___);
    }

    
    function toFixed(let number, let pattern): {
        return _csharp(usings + ___STRING_37___ + number + ___STRING_38___ + _str(pattern) + ___STRING_39___);
    }
}

class String: {
    private let _value = ___STRING_40___;

    function new(let input): {
        _value = input;
    }

    function toString(): {
        return _value;
    }

    
    private function getStr(let str): {
        return ___STRING_41___ + _str(str) + ___STRING_42___;
    }

    
    function append(let collection): {
        if Array.isArray(collection) == false: {
            Throw.parceException(___STRING_43___);
        }

        let result = ___STRING_44___;

        let arr = Array.new(collection);

        for let i = ___NUMBER_0___; i < arr.length(); i = i + ___NUMBER_1___: {
            result = result + arr.at(i);
        }

        return result;
    }

    
    function length(): {
        return _csharp(getStr(_value) + ___STRING_45___);
    }

    
    function isNullOrEmpty(let str): {
        return _csharp(___STRING_46___ + _str(str) + ___STRING_47___);
    }

    
    function isNullOrWhiteSpace(let str): {
        return _csharp(___STRING_48___ + _str(str) + ___STRING_49___);
    }

    
    function trim(): {
        return _csharp(getStr(str) + ___STRING_50___);
    }

    
    function trimStart(): {
        return _csharp(getStr(_value) + ___STRING_51___);
    }

    
    function trimEnd(): {
        return _csharp(getStr(_value) + ___STRING_52___);
    }

    
    function subString(let startPos, let length): {
        if Number.isNumber(startPos) == false: {
            Throw.exception(___STRING_53___);
        }

        if Number.isNumber(length) == false: {
            Throw.exception(___STRING_54___);
        }

        return _csharp(getStr(_value) + ___STRING_55___ + startPos + ___STRING_56___ + length + ___STRING_57___);
    }
}


class Console: {
    private let usings = ___STRING_58___;
    private let defFColor = ___STRING_59___;
    private let defBColor = ___STRING_60___;

    
    function print(let message): {
        _csharp(usings + ___STRING_61___ + _str(message) + ___STRING_62___);
    }

    
    function println(let message): {
        print(message + ___STRING_63___);
    }

    
    function readln(): {
        return _csharp(usings + ___STRING_64___);
    }

    
    function clear(): {
        _csharp(usings + ___STRING_65___);
    }

    
    function setCursorPosition(let x, let y): {
        _csharp(usings + ___STRING_66___ + x + ___STRING_67___ + y + ___STRING_68___);
    }

    
    function setForeColor(let color): {
        let line1 = ___STRING_69___ + _str(color) + ___STRING_70___;
        let line2 = ___STRING_71___;
        _csharp(usings + line1 + line2);
    }
    
    
    function setBackColor(let color): {
        let line1 = ___STRING_72___ + _str(color) + ___STRING_73___;
        let line2 = ___STRING_74___;
        _csharp(usings + line1 + line2);
    }
    
    function resetColors(): {
        setForeColor(defFColor);
        setBackColor(defBColor);
    }
}

class Math: {
    
    private function throwException(let num): {
        if Number.isNumber(num) == false: {
            Throw.exception(___STRING_75___ + num + ___STRING_76___);
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
            Throw.exception(___STRING_77___);
        }
        return num / num2;
    }
}
class Object: {
    function toString(): {
        return ___STRING_78___;
    }
}

class Throw: {
    
    function exception(let message): {
        _csharp(___STRING_79___ + _str(message) + ___STRING_80___);
    }

    
    function nonImplementException(): {
        exception(___STRING_81___);
    }

    
    function parseException(let error): {
        exception(___STRING_82___ + error);
    }
}


class LanguageDemo: {
    function run(): {

        Console.println(___STRING_83___);
        Console.println(___STRING_84___);
        Console.println(___STRING_85___);
        Console.println(___STRING_86___);
        
        
        do_while (String.isNullOrWhiteSpace(choice) == true) || (Number.isNumber(choice) == false): {
            Console.print(___STRING_87___);
            let choice = Console.readln();

            if String.isNullOrWhiteSpace(choice) == true: {
                printException(___STRING_88___);
            }

            if Number.isNumber(choice) == false: {
                printException(___STRING_89___);
            }
        }

        if choice == ___STRING_90___: {
            numericDemo();
        }
        else if choice == ___STRING_91___: {
            stringDemo();
        }
        else if choice == ___STRING_92___: {
            circleDemo();
        }

        Console.println(___STRING_93___);
    }

    function stringDemo(): {
        Console.println(___STRING_94___);

        do_while String.isNullOrWhiteSpace(input) == true: { 
            Console.print(___STRING_95___);
            let input = Console.readln();

            if String.isNullOrWhiteSpace(input) == true: {
                printException(___STRING_96___);
            }
        }
        
        Console.println(___STRING_97___ + String.getLength(input));
        Console.println(___STRING_98___ + String.trim(input) + ___STRING_99___);
        Console.println(___STRING_100___ + String.trimStart(input) + ___STRING_101___);
        Console.println(___STRING_102___ + String.trimEnd(input) + ___STRING_103___);
        
        if String.getLength(input) > ___NUMBER_5___: {
            Console.println(___STRING_104___ + String.subString(input, ___NUMBER_6___, ___NUMBER_7___) + ___STRING_105___);
        }
    }

    function numericDemo(): {
        Console.println(___STRING_106___);

        let num1 = getNumberFromConsole(___STRING_107___);
        let num2 = getNumberFromConsole(___STRING_108___);

        Console.println(___STRING_109___ + num1);
        Console.println(___STRING_110___ + num2);

        Console.println(___STRING_111___);
        Console.println(___STRING_112___ + Math.sum(num1, num2));
        Console.println(___STRING_113___ + Math.sub(num1, num2));
        Console.println(___STRING_114___ + Math.mult(num1, num2));

        if num2 != ___NUMBER_8___: {
            Console.println(___STRING_115___ + Number.toFixed(Math.div(num1, num2), ___STRING_116___));
        }
        else: {
            printException(___STRING_117___);
        }

        if num1 < num2: {
            Console.println(___STRING_118___ + num1 + ___STRING_119___ + num2 + ___STRING_120___ + Number.randInt(num1, num2));
        }
        else: {
            printException(___STRING_121___);
        }

        for let i = num1 - ___NUMBER_9___; i < num1; i = i + ___NUMBER_10___: {
            Console.println(___STRING_122___ + i);
        }

        Console.println(___STRING_123___);
        Console.println(___STRING_124___ + Number.toInt(num1));
        Console.println(___STRING_125___ + Number.toInt(num2));

        Console.println(___STRING_126___ + Number.toFixed(num1, ___STRING_127___));
        Console.println(___STRING_128___ + Number.toFixed(num2, ___STRING_129___));

        Console.println(___STRING_130___ + Math.PI);

        Console.println(___STRING_131___ + Number.MIN_VALUE);
        Console.println(___STRING_132___ + Number.MAX_VALUE);
    }

    function circleDemo(): {
        Console.println(___STRING_133___);

        Console.println(___STRING_134___);
        Console.print(___STRING_135___);
        
        let randNum = ___NUMBER_11___-___NUMBER_12___;
        
        while randNum != ___NUMBER_13___: {
            randNum = Number.randInt(___NUMBER_14___, ___NUMBER_15___);
            Console.print(randNum + ___STRING_136___);
        }

        Console.println(___STRING_137___);
        Console.println(___STRING_138___);

        acceptContinue();

        Console.println(___STRING_139___);
        do_while input != ___STRING_140___: {
            Console.print(___STRING_141___);
            let input = Console.readln();
        }
        Console.println(___STRING_142___);

        acceptContinue();

        Console.println(___STRING_143___);
        
        for let i = ___NUMBER_16___; i < ___NUMBER_17___; i = i + ___NUMBER_18___: {
            Console.println(___STRING_144___ + i);
        }
    }

    function acceptContinue(): {
        Console.println(___STRING_145___);
        Console.readln();
    }

    function printException(let msg): {
        Console.setForeColor(___STRING_146___);
        Console.println(msg);
        Console.resetColors();
    }

    function getNumberFromConsole(let msg): {
        let num = ___STRING_147___;

        do_while Number.isNumber(num) == false: {
            Console.print(msg);
            num = Console.readln();

            if Number.isNumber(num) == false: {
                printException(___STRING_148___);
            }
        }

        return num;
    }
}

function main(): {
    let arr = Array.new([___STRING_149___, ___NUMBER_19___]);
    
    for let i = ___NUMBER_20___; i < ___NUMBER_21___; i = i + ___NUMBER_22___: {
        Console.println(arr.at(i));
    }
    Console.println(String.append([___STRING_150___, ___STRING_151___, ___STRING_152___]));
}
