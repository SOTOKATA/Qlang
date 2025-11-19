
class Throw:
    
    function exception(let message):
        _csharp(___STRING_0___ + _str(message) + ___STRING_1___)

    function nonImplementException():
        exception(___STRING_2___)


class String:
    
    private function getStr(let str):
        return ___STRING_3___ + _str(str) + ___STRING_4___

    
    function append(let str, let str2):
        return str + str2

    
    function getLength(let str):
        return _csharp(getStr(str) + ___STRING_5___)

    
    function isNullOrEmpty(let str):
        return _csharp(___STRING_6___ + _str(str) + ___STRING_7___)

    
    function isNullOrWhiteSpace(let str):
        return _csharp(___STRING_8___ + _str(str) + ___STRING_9___)

    
    function trim(let str):
        return _csharp(getStr(str) + ___STRING_10___)

    
    function trimStart(let str):
        return _csharp(getStr(str) + ___STRING_11___)

    
    function trimEnd(let str):
        return _csharp(getStr(str) + ___STRING_12___)

    
    function subString(let str, let startPos, let length):
        if Number.isNumber(startPos) == false:
            Throw.exception(___STRING_13___)

        if Number.isNumber(length) == false:
            Throw.exception(___STRING_14___)

        return _csharp(getStr(str) + ___STRING_15___ + startPos + ___STRING_16___ + length + ___STRING_17___)

class Console: 
    private let usings = ___STRING_18___
    private let defFColor = ___STRING_19___
    private let defBColor = ___STRING_20___

    
    function print(let message): 
        _csharp(usings + ___STRING_21___ + _str(message) + ___STRING_22___)
    
    
    function println(let message):
        print(message + ___STRING_23___)

    
    function readln():
        return _csharp(usings + ___STRING_24___)

    
    function clear():
        _csharp(usings + ___STRING_25___)
    
    
    function setCursorPosition(let x, let y):
        _csharp(usings + ___STRING_26___ + x + ___STRING_27___ + y + ___STRING_28___)

    
    function setForeColor(let color):
        let line1 = ___STRING_29___ + _str(color) + ___STRING_30___
        let line2 = ___STRING_31___
        _csharp(usings + line1 + line2)
    
    
    function setBackColor(let color):
        let line1 = ___STRING_32___ + _str(color) + ___STRING_33___
        let line2 = ___STRING_34___
        _csharp(usings + line1 + line2)

    
    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)


class Number:
    
    
    
    
    

    private let isUsr = false

    
    private let usings = ___STRING_35___

    
    const let MIN_VALUE = ___STRING_36___
    const let MAX_VALUE = ___STRING_37___

    
    function isNumber(let var):
        return _csharp(usings + ___STRING_38___ + _str(var) + ___STRING_39___)

    
    function randInt(let min, let max):
        if (isNumber(min) == false) || (isNumber(max) == false):
            Throw.exception(___STRING_40___)

        
        min = toInt(min)
        max = toInt(max)

        if min >= max:
            Throw.exception(___STRING_41___)

        return _csharp(usings + ___STRING_42___ + min + ___STRING_43___ + max + ___STRING_44___)

    
    function toInt(let float):
        return toFixed(float, ___STRING_45___)

    
    function toFixed(let number, let pattern):
        return _csharp(usings + ___STRING_46___ + number + ___STRING_47___ + _str(pattern) + ___STRING_48___)
function main():
    let numberClass = Number.new()

    Console.println(numberClass.isNumber(___NUMBER_7___))
