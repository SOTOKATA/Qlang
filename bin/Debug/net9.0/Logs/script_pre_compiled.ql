
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

class Array:
    private function _csharpArr(let qarray):
        return _csharp(___STRING_14___ + qarray +___STRING_15___)

    function empty():
        return _csharp(___STRING_16___)

    function at(let array, let index):
        if Number.isNumber(index) == false:
            Throw.exception(___STRING_17___)

        return _csharp(_csharpArr(array) + ___STRING_18___ + index + ___STRING_19___)

    function append(let array, let item):
        _csharp(_csharpArr(array) + ___STRING_20___)

    


class Number:
    
    private let usings = ___STRING_21___

    
    const let MIN_VALUE = ___STRING_22___
    const let MAX_VALUE = ___STRING_23___

    
    function isNumber(let var):
        return _csharp(usings + ___STRING_24___ + _str(var) + ___STRING_25___)

    
    function randInt(let min, let max):
        if min >= max:
            Throw.exception(___STRING_26___)

        
        min = toInt(min)
        max = toInt(max)

        return _csharp(usings + ___STRING_27___ + min + ___STRING_28___ + max + ___STRING_29___)

    
    function toInt(let float):
        return toFixed(float, ___STRING_30___)

    
    function toFixed(let number, let pattern):
        return _csharp(usings + number + ___STRING_31___ + _str(pattern) + ___STRING_32___)


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


function main():
    let array = Array.empty()

    array = Array.append(array, ___STRING_48___)

    Console.println(Array.at(array, ___NUMBER_14___))
