
class Throw:
    
    function exception(let message):
        _csharp(___STRING_0___ + _str(message) + ___STRING_1___)

class Console: 
    private let usings = ___STRING_2___
    private let defFColor = ___STRING_3___
    private let defBColor = ___STRING_4___

    
    function print(let message): 
        _csharp(usings + ___STRING_5___ + _str(message) + ___STRING_6___)
    
    
    function println(let message):
        print(message + ___STRING_7___)

    
    function readln():
        return _csharp(usings + ___STRING_8___)

    
    function clear():
        _csharp(usings + ___STRING_9___)
    
    
    function setCursorPosition(let x, let y):
        _csharp(usings + ___STRING_10___ + x + ___STRING_11___ + y + ___STRING_12___)

    
    function setForeColor(let color):
        let line1 = ___STRING_13___ + _str(color) + ___STRING_14___
        let line2 = ___STRING_15___
        _csharp(usings + line1 + line2)
    
    
    function setBackColor(let color):
        let line1 = ___STRING_16___ + _str(color) + ___STRING_17___
        let line2 = ___STRING_18___
        _csharp(usings + line1 + line2)

    
    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)



class Number:
    
    private let usings = ___STRING_19___

    
    const let MIN_VALUE = ___STRING_20___
    const let MAX_VALUE = ___STRING_21___

    
    function isNumber(let var):
        return _csharp(usings + ___STRING_22___ + _str(var) + ___STRING_23___)

    
    function randInt(let min, let max):
        if isNumber(min) == false || isNumber(max) == false:
            Throw.exception(___STRING_24___)

        
        min = toInt(min)
        max = toInt(max)

        if min >= max:
            Throw.exception(___STRING_25___)

        return _csharp(usings + ___STRING_26___ + min + ___STRING_27___ + max + ___STRING_28___)

    
    function toInt(let float):
        return toFixed(float, ___STRING_29___)

    
    function toFixed(let number, let pattern):
        return _csharp(usings + ___STRING_30___ + number + ___STRING_31___ + _str(pattern) + ___STRING_32___)


class String:
    
    private function getStr(let str):
        return ___STRING_33___ + _str(str) + ___STRING_34___

    
    function append(let str, let str2):
        return str + str2

    
    function getLength(let str):
        return _csharp(getStr(str) + ___STRING_35___)

    
    function isNullOrEmpty(let str):
        return _csharp(___STRING_36___ + _str(str) + ___STRING_37___)

    
    function isNullOrWhiteSpace(let str):
        return _csharp(___STRING_38___ + _str(str) + ___STRING_39___)

    
    function trim(let str):
        return _csharp(getStr(str) + ___STRING_40___)

    
    function trimStart(let str):
        return _csharp(getStr(str) + ___STRING_41___)

    
    function trimEnd(let str):
        return _csharp(getStr(str) + ___STRING_42___)

    
    function subString(let str, let startPos, let length):
        if Number.isNumber(startPos) == false:
            Throw.exception(___STRING_43___)

        if Number.isNumber(length) == false:
            Throw.exception(___STRING_44___)

        return _csharp(getStr(str) + ___STRING_45___ + startPos + ___STRING_46___ + length + ___STRING_47___)



class Math:
    
    private function throwException(let num):
        if Number.isNumber(num) == false:
            Throw.exception(___STRING_48___ + num + ___STRING_49___)

    const let PI = ___NUMBER_7___

    
    function abs(let num):
        throwException(num)

        return ___NUMBER_8___ - num

    
    function sum(let num, let num2):
        throwException(num)
        throwException(num2)

        return num + num2

    
    function sub(let num, let num2):
        throwException(num)
        throwException(num2)

        return num - num2
        
    
    function mult(let num, let num2):
        throwException(num)
        throwException(num2)

        return num * num2

    
    function div(let num, let num2):
        throwException(num)
        throwException(num2)

        if num2 == ___NUMBER_9___:
            Throw.exception(___STRING_50___)
        return num / num2

class LanguageDemo:
    function run():

        Console.println(___STRING_51___)
        Console.println(___STRING_52___)
        Console.println(___STRING_53___)
        Console.println(___STRING_54___)
        
        
        do_while String.isNullOrWhiteSpace(choice) == true || Number.isNumber(choice) == false:
            Console.print(___STRING_55___)
            let choice = Console.readln()

            if String.isNullOrWhiteSpace(choice) == true:
                printException(___STRING_56___)

            if Number.isNumber(choice) == false:
                printException(___STRING_57___)

        if choice == ___STRING_58___:
            numericDemo()
        else if choice == ___STRING_59___:
            stringDemo()
        else if choice == ___STRING_60___:
            circleDemo()

        Console.println(___STRING_61___)

    function stringDemo():
        Console.println(___STRING_62___)

        do_while String.isNullOrWhiteSpace(input) == true: 
            Console.print(___STRING_63___)
            let input = Console.readln()

            if String.isNullOrWhiteSpace(input) == true:
                printException(___STRING_64___)
        
        Console.println(___STRING_65___ + String.getLength(input))
        Console.println(___STRING_66___ + String.trim(input) + ___STRING_67___)
        Console.println(___STRING_68___ + String.trimStart(input) + ___STRING_69___)
        Console.println(___STRING_70___ + String.trimEnd(input) + ___STRING_71___)
        
        if String.getLength(input) > ___NUMBER_10___:
            Console.println(___STRING_72___ + String.subString(input, ___NUMBER_11___, ___NUMBER_12___) + ___STRING_73___)

    function numericDemo():
        Console.println(___STRING_74___)

        let num1 = getNumberFromConsole(___STRING_75___)
        let num2 = getNumberFromConsole(___STRING_76___)

        Console.println(___STRING_77___ + num1)
        Console.println(___STRING_78___ + num2)

        Console.println(___STRING_79___)
        Console.println(___STRING_80___ + Math.sum(num1, num2))
        Console.println(___STRING_81___ + Math.sub(num1, num2))
        Console.println(___STRING_82___ + Math.mult(num1, num2))

        if num2 != ___NUMBER_13___:
            Console.println(___STRING_83___ + Number.toFixed(Math.div(num1, num2), ___STRING_84___))
        else:
            printException(___STRING_85___)

        if num1 < num2:
            Console.println(___STRING_86___ + num1 + ___STRING_87___ + num2 + ___STRING_88___ + Number.randInt(num1, num2))
        else:
            printException(___STRING_89___)

        for let i = num1 - ___NUMBER_14___; i < num1; i = i + ___NUMBER_15___:
            Console.println(___STRING_90___ + i)

        Console.println(___STRING_91___)
        Console.println(___STRING_92___ + Number.toInt(num1))
        Console.println(___STRING_93___ + Number.toInt(num2))

        Console.println(___STRING_94___ + Number.toFixed(num1, ___STRING_95___))
        Console.println(___STRING_96___ + Number.toFixed(num2, ___STRING_97___))

        Console.println(___STRING_98___ + Math.PI)

        Console.println(___STRING_99___ + Number.MIN_VALUE)
        Console.println(___STRING_100___ + Number.MAX_VALUE)

    function circleDemo():
        Console.println(___STRING_101___)

        Console.println(___STRING_102___)
        Console.print(___STRING_103___)
        let randNum = ___NUMBER_16___-___NUMBER_17___
        while randNum != ___NUMBER_18___:
            randNum = Number.randInt(___NUMBER_19___, ___NUMBER_20___)
            Console.print(randNum + ___STRING_104___)
        Console.println(___STRING_105___)
        Console.println(___STRING_106___)

        acceptContinue()

        Console.println(___STRING_107___)
        do_while input != ___STRING_108___:
            Console.print(___STRING_109___)
            let input = Console.readln()
        Console.println(___STRING_110___)

        acceptContinue()

        Console.println(___STRING_111___)
        for let i = ___NUMBER_21___; i < ___NUMBER_22___; i = i + ___NUMBER_23___:
            Console.println(___STRING_112___ + i)

    function acceptContinue():
        Console.println(___STRING_113___)
        Console.readln()

    function printException(let msg):
        Console.setForeColor(___STRING_114___)
        Console.println(msg)
        Console.resetColors()

    function getNumberFromConsole(let msg):
        let num = ___STRING_115___

        do_while Number.isNumber(num) == false:
            Console.print(msg) 
            num = Console.readln()

            if Number.isNumber(num) == false:
                printException(___STRING_116___)

        return num


function main():
    LanguageDemo.run()
