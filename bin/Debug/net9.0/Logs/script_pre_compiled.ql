
class Throw:
    
    function exception(let message):
        _csharp(___STRING_0___ + _str(message) + ___STRING_1___)

    function nonImplementException():
        exception(___STRING_2___)
class Vector2:
    let x = ___NUMBER_0___
    let y = ___NUMBER_1___

    function zero():
        return Vector2.new(___NUMBER_2___, ___NUMBER_3___)

    function one():
        return Vector2.new(___NUMBER_4___, ___NUMBER_5___)

    function set(let posX, let posY):
        x = posX
        y = posY

    function toString():
        return ___STRING_3___ + x + ___STRING_4___ + y + ___STRING_5___ 

class Console: 
    private let usings = ___STRING_6___
    private let defFColor = ___STRING_7___
    private let defBColor = ___STRING_8___

    
    function print(let message): 
        _csharp(usings + ___STRING_9___ + _str(message) + ___STRING_10___)
    
    
    function println(let message):
        print(message + ___STRING_11___)

    
    function readln():
        return _csharp(usings + ___STRING_12___)

    
    function clear():
        _csharp(usings + ___STRING_13___)
    
    
    function setCursorPosition(let x, let y):
        _csharp(usings + ___STRING_14___ + x + ___STRING_15___ + y + ___STRING_16___)

    
    function setForeColor(let color):
        let line1 = ___STRING_17___ + _str(color) + ___STRING_18___
        let line2 = ___STRING_19___
        _csharp(usings + line1 + line2)
    
    
    function setBackColor(let color):
        let line1 = ___STRING_20___ + _str(color) + ___STRING_21___
        let line2 = ___STRING_22___
        _csharp(usings + line1 + line2)

    
    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)


class String:
    private let _value = ___STRING_23___

    function new(let input):
        _value = input

    
    private function getStr(let str):
        return ___STRING_24___ + _str(str) + ___STRING_25___

    
    function append(let str):
        return _value + str

    
    function length():
        return _csharp(getStr(_value) + ___STRING_26___)

    
    function isNullOrEmpty(let str):
        return _csharp(___STRING_27___ + _str(str) + ___STRING_28___)

    
    function isNullOrWhiteSpace(let str):
        return _csharp(___STRING_29___ + _str(str) + ___STRING_30___)

    
    function trim():
        return _csharp(getStr(str) + ___STRING_31___)

    
    function trimStart():
        return _csharp(getStr(_value) + ___STRING_32___)

    
    function trimEnd():
        return _csharp(getStr(_value) + ___STRING_33___)

    
    function subString(let startPos, let length):
        if Number.isNumber(startPos) == false:
            Throw.exception(___STRING_34___)

        if Number.isNumber(length) == false:
            Throw.exception(___STRING_35___)

        return _csharp(getStr(_value) + ___STRING_36___ + startPos + ___STRING_37___ + length + ___STRING_38___)

function main():
    let vect = Vector2.new()
    let name = String.new(___STRING_39___)

    Console.println(name)
    Console.println(name.length())
    
    vect.set(___NUMBER_6___, ___NUMBER_7___)

    Console.println(___STRING_40___ + vect.toString())
    Console.println(___STRING_41___ + Vector2.toString())
