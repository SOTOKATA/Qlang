
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
        if min >= max:
            Throw.exception(___STRING_24___)

        
        min = toInt(min)
        max = toInt(max)

        return _csharp(usings + ___STRING_25___ + min + ___STRING_26___ + max + ___STRING_27___)

    
    function toInt(let float):
        return toFixed(float, ___STRING_28___)

    
    function toFixed(let number, let pattern):
        return _csharp(usings + number + ___STRING_29___ + _str(pattern) + ___STRING_30___)


class String:
    
    private function getStr(let str):
        return ___STRING_31___ + _str(str) + ___STRING_32___

    
    function append(let str, let str2):
        return str + str2

    
    function getLength(let str):
        return _csharp(getStr(str) + ___STRING_33___)

    
    function isNullOrEmpty(let str):
        return _csharp(___STRING_34___ + _str(str) + ___STRING_35___)

    
    function isNullOrWhiteSpace(let str):
        return _csharp(___STRING_36___ + _str(str) + ___STRING_37___)

    
    function trim(let str):
        return _csharp(getStr(str) + ___STRING_38___)

    
    function trimStart(let str):
        return _csharp(getStr(str) + ___STRING_39___)

    
    function trimEnd(let str):
        return _csharp(getStr(str) + ___STRING_40___)

    
    function subString(let str, let startPos, let length):
        if Number.isNumber(startPos) == false:
            Throw.exception(___STRING_41___)

        if Number.isNumber(length) == false:
            Throw.exception(___STRING_42___)

        return _csharp(getStr(str) + ___STRING_43___ + startPos + ___STRING_44___ + length + ___STRING_45___)


class Math:
    
    private function throwException(let num):
        if Number.isNumber(num) == false:
            Throw.exception(___STRING_46___ + num + ___STRING_47___)

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
            Throw.exception(___STRING_48___)
        return num / num2

function main():
    Console.println(___STRING_49___)
    Console.println(___STRING_50___)
    Console.println(___STRING_51___)
    
    
    do_while String.isNullOrWhiteSpace(choice) == true || Number.isNumber(choice) == false:
        Console.print(___STRING_52___)
        let choice = Console.readln()

        if String.isNullOrWhiteSpace(choice) == true:
            printException(___STRING_53___)

        if Number.isNumber(choice) == false:
            printException(___STRING_54___)

    if choice == ___STRING_55___:
        numericDemo()
    else if choice == ___STRING_56___:
        stringDemo()

    Console.println(___STRING_57___)

function stringDemo():
    Console.println(___STRING_58___)

    do_while String.isNullOrWhiteSpace(input) == true: 
        Console.print(___STRING_59___)
        let input = Console.readln()

        if String.isNullOrWhiteSpace(input) == true:
            printException(___STRING_60___)
     
    Console.println(___STRING_61___ + String.getLength(input))
    Console.println(___STRING_62___ + String.trim(input) + ___STRING_63___)
    Console.println(___STRING_64___ + String.trimStart(input) + ___STRING_65___)
    Console.println(___STRING_66___ + String.trimEnd(input) + ___STRING_67___)
    
    if String.getLength(input) > ___NUMBER_10___:
        Console.println(___STRING_68___ + String.subString(input, ___NUMBER_11___, ___NUMBER_12___) + ___STRING_69___)

function numericDemo():
    Console.println(___STRING_70___)

    let num1 = getNumberFromConsole(___STRING_71___)
    let num2 = getNumberFromConsole(___STRING_72___)

    Console.println(___STRING_73___ + num1)
    Console.println(___STRING_74___ + num2)

    Console.println(___STRING_75___)
    Console.println(___STRING_76___ + Math.sum(num1, num2))
    Console.println(___STRING_77___ + Math.sub(num1, num2))
    Console.println(___STRING_78___ + Math.mult(num1, num2))

    if num2 != ___NUMBER_13___:
        Console.println(___STRING_79___ + Number.toFixed(Math.div(num1, num2), ___STRING_80___))
    else:
        printException(___STRING_81___)

    if num1 < num2:
        Console.println(___STRING_82___ + num1 + ___STRING_83___ + num2 + ___STRING_84___ + Number.randInt(num1, num2))
    else:
        printException(___STRING_85___)

    Console.println(___STRING_86___)
    Console.println(___STRING_87___ + Number.toInt(num1))
    Console.println(___STRING_88___ + Number.toInt(num2))

    Console.println(___STRING_89___ + Number.toFixed(num1, ___STRING_90___))
    Console.println(___STRING_91___ + Number.toFixed(num2, ___STRING_92___))

    Console.println(___STRING_93___ + Math.PI)

    Console.println(___STRING_94___ + Number.MIN_VALUE)
    Console.println(___STRING_95___ + Number.MAX_VALUE)


function printException(let msg):
    Console.setForeColor(___STRING_96___)
    Console.println(msg)
    Console.resetColors()

function getNumberFromConsole(let msg):
    let num = ___STRING_97___

    do_while Number.isNumber(num) == false:
        Console.print(msg) 
        num = Console.readln()

        if Number.isNumber(num) == false:
            printException(___STRING_98___)

    return num
