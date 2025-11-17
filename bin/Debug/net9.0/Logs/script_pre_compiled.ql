
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
        if isNumber(min) == true && isNumber(max) == false && min < ___NUMBER_0___:
            Throw.exception(___STRING_7___)

        if min >= max:
            Throw.exception(___STRING_8___)

        
        min = toInt(min)
        max = toInt(max)

        return _csharp(usings + ___STRING_9___ + min + ___STRING_10___ + max + ___STRING_11___)

    
    function toInt(let float):
        return toFixed(float, ___STRING_12___)

    
    function toFixed(let number, let pattern):
        return _csharp(usings + ___STRING_13___ + number + ___STRING_14___ + _str(pattern) + ___STRING_15___)

function main():
    Number.randInt(___NUMBER_8___-___NUMBER_9___, ___STRING_16___)
    
    
    
    
    

    
    
    
    
    
    

