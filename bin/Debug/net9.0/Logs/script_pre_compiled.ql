
class Throw:
    
    function exception(let message):
        _csharp(___STRING_0___ + _str(message) + ___STRING_1___)



class Number:
    
    private let usings = ___STRING_2___

    
    const let MIN_VALUE = ___STRING_3___
    const let MAX_VALUE = ___STRING_4___

    
    function isNumber(let var):
        return _csharp(usings + ___STRING_5___ + _str(var) + ___STRING_6___)

    
    function randInt(let min, let max):
        if min >= max:
            Throw.exception(___STRING_7___)

        
        min = toInt(min)
        max = toInt(max)

        return _csharp(usings + ___STRING_8___ + min + ___STRING_9___ + max + ___STRING_10___)

    
    function toInt(let float):
        return toFixed(float, ___STRING_11___)

    
    function toFixed(let number, let pattern):
        return _csharp(usings + number + ___STRING_12___ + _str(pattern) + ___STRING_13___)


class String:
    
    private function getStr(let str):
        return ___STRING_14___ + _str(str) + ___STRING_15___

    
    function append(let str, let str2):
        return str + str2

    
    function getLength(let str):
        return _csharp(getStr(str) + ___STRING_16___)

    
    function isNullOrEmpty(let str):
        return _csharp(___STRING_17___ + _str(str) + ___STRING_18___)

    
    function isNullOrWhiteSpace(let str):
        return _csharp(___STRING_19___ + _str(str) + ___STRING_20___)

    
    function trim(let str):
        return _csharp(getStr(str) + ___STRING_21___)

    
    function trimStart(let str):
        return _csharp(getStr(str) + ___STRING_22___)

    
    function trimEnd(let str):
        return _csharp(getStr(str) + ___STRING_23___)

    
    function subString(let str, let startPos, let length):
        if Number.isNumber(startPos) == false:
            Throw.exception(___STRING_24___)

        if Number.isNumber(length) == false:
            Throw.exception(___STRING_25___)

        return _csharp(getStr(str) + ___STRING_26___ + startPos + ___STRING_27___ + length + ___STRING_28___)



class Math:
    
    private function throwException(let num):
        if Number.isNumber(num) == false:
            Throw.exception(___STRING_29___ + num + ___STRING_30___)

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
            Throw.exception(___STRING_31___)
        return num / num2

class Console: 
    private let usings = ___STRING_32___
    private let defFColor = ___STRING_33___
    private let defBColor = ___STRING_34___

    
    function print(let message): 
        _csharp(usings + ___STRING_35___ + _str(message) + ___STRING_36___)
    
    
    function println(let message):
        print(message + ___STRING_37___)

    
    function readln():
        return _csharp(usings + ___STRING_38___)

    
    function clear():
        _csharp(usings + ___STRING_39___)
    
    
    function setCursorPosition(let x, let y):
        _csharp(usings + ___STRING_40___ + x + ___STRING_41___ + y + ___STRING_42___)

    
    function setForeColor(let color):
        let line1 = ___STRING_43___ + _str(color) + ___STRING_44___
        let line2 = ___STRING_45___
        _csharp(usings + line1 + line2)
    
    
    function setBackColor(let color):
        let line1 = ___STRING_46___ + _str(color) + ___STRING_47___
        let line2 = ___STRING_48___
        _csharp(usings + line1 + line2)

    
    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)
class File:
    private let usings = ___STRING_49___

    function exists(let message):   
        return _csharp(usings + ___STRING_50___ + _str(message) + ___STRING_51___)
class Path:
    private let usings = ___STRING_52___
    function getExtension(let path):
        return _csharp(usings + ___STRING_53___ + _str(path) + ___STRING_54___)

    function combine(let first, let second):
        return _csharp(usings + ___STRING_55___ + _str(first) + ___STRING_56___ + _str(second) + ___STRING_57___)

    function getDirSeparator():
        return _csharp(usings + ___STRING_58___)
        
    function getAltDirSeparator():
        return _csharp(usings + ___STRING_59___)


function printException(let message):
    Console.setForeColor(___STRING_60___)
    Console.println(message)
    Console.resetColors()

function main():
    let pathExample = Path.combine(___STRING_61___, ___STRING_62___)

    printException(___STRING_63___)
    for let i = ___NUMBER_10___; i < ___NUMBER_11___; i = i + ___NUMBER_12___:
        Console.println(___STRING_64___ + i)

    Console.println(___STRING_65___ + pathExample)
    Console.println(___STRING_66___ + Path.getExtension(pathExample))
    Console.println(___STRING_67___ + Path.getDirSeparator())
    Console.println(___STRING_68___ + Path.getAltDirSeparator())
