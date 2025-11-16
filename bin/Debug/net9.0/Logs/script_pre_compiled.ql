class Throw:
    function exception(let message):
        csharp(___STRING_0___ + message + ___STRING_1___)
class Console: 
    let usings = ___STRING_2___
    let defFColor = ___STRING_3___
    let defBColor = ___STRING_4___

    function print(let message): 
        csharp(usings + ___STRING_5___ + message + ___STRING_6___)
    
    function println(let message):
        print(message + ___STRING_7___)

    function readln():
        return csharp(usings + ___STRING_8___)

    function clear():
        csharp(usings + ___STRING_9___)
    
    function setCursorPosition(let x, let y):
        csharp(usings + ___STRING_10___ + x + ___STRING_11___ + y + ___STRING_12___)

    function setForeColor(let color):
        let line1 = ___STRING_13___ + color + ___STRING_14___
        let line2 = ___STRING_15___
        csharp(usings + line1 + line2)
    
    function setBackColor(let color):
        let line1 = ___STRING_16___ + color + ___STRING_17___
        let line2 = ___STRING_18___
        csharp(usings + line1 + line2)

    function resetColors():
        setForeColor(defFColor)
        setBackColor(defBColor)

class Number:
    function getUsings():
        return usings

    private let usings = ___STRING_19___
        
    function getMinValue():
        return csharp(usings + ___STRING_20___)

    function getMaxValue():
        return csharp(usings + ___STRING_21___)

    function isNumber(let var):
        return csharp(usings + ___STRING_22___ + var + ___STRING_23___)

    function randInt(let min, let max):
        if min >= max:
            Throw.exception(___STRING_24___)
        return csharp(usings + ___STRING_25___ + min + ___STRING_26___ + max + ___STRING_27___)

    function toFixed(let number, let pattern):
        return csharp(usings + number + ___STRING_28___ + pattern + ___STRING_29___)
class String:
    
    function getStr(let str):
        return ___STRING_30___ + str + ___STRING_31___

    function append(let str, let str2):
        return str + str2

    function getLength(let str):
        return csharp(getStr(str) + ___STRING_32___)

    function isNullOrEmty(let str):
        return csharp(___STRING_33___ + str + ___STRING_34___)

    function isNullOrWhiteSpace(let str):
        return csharp(___STRING_35___ + str + ___STRING_36___)

    function trim(let str):
        return csharp(getStr(str) + ___STRING_37___)

    function trimStart(let str):
        return csharp(getStr(str) + ___STRING_38___)

    function trimEnd(let str):
        return csharp(getStr(str) + ___STRING_39___)

    function subString(let str, let startPos, let length):
        return csharp(getStr(str) + ___STRING_40___ + startPos + ___STRING_41___ + length + ___STRING_42___)

class Math:
    function throwException(let num):
        if Number.isNumber(num) == false:
            Throw.exception(___STRING_43___ + num + ___STRING_44___)

    function getPI():
        return ___NUMBER_0___

    function abs(let num):
        throwException(num)
        return ___NUMBER_1___ - num

    function sum(let num, let num2):
        throwException(num)
        return num + num2

    function sub(let num, let num2):
        throwException(num)
        return num - num2
        
    function mult(let num, let num2):
        throwException(num)
        return num * num2

    function div(let num, let num2):
        throwException(num)
        throwException(num2)
        if num2 == ___NUMBER_2___:
            Throw.exception(___STRING_45___)
        return num / num2

function main():
    Console.println(___STRING_46___)
    Console.println(___STRING_47___)

    Console.print(___STRING_48___)
    let choice = Console.readln()

    if choice == ___NUMBER_3___:
        numericDemo()
    else if choice == ___NUMBER_4___:
        stringDemo()

function stringDemo():
    return ___STRING_49___

function numericDemo():
    Console.println(___STRING_50___)

    let num1 = getNumberFromConsole(___STRING_51___)
    let num2 = getNumberFromConsole(___STRING_52___)

    Console.println(___STRING_53___ + num1)
    Console.println(___STRING_54___ + num2)

    Console.println(___STRING_55___)
    Console.println(___STRING_56___ + Math.sum(num1, num2))
    Console.println(___STRING_57___ + Math.sub(num1, num2))
    Console.println(___STRING_58___ + Math.mult(num1, num2))

    let res = Number.toFixed(Math.div(num1, num2), ___STRING_59___)

    if num2 != ___NUMBER_5___:
        Console.println(___STRING_60___ + res)
    else:
        Console.println(___STRING_61___)

    if num1 < num2:
        Console.println(___STRING_62___ + num1 + ___STRING_63___ + num2 + ___STRING_64___ + Number.randInt(num1, num2))
    else:
        Console.println(___STRING_65___)

function printException(let msg):
    Console.setForeColor(___STRING_66___)
    Console.println(msg)
    Console.resetColors()

function getNumberFromConsole(let msg):
    let num = ___STRING_67___

    do_while Number.isNumber(num) == false:
        Console.print(msg) 
        num = Console.readln()

        if Number.isNumber(num) == false:
            printException(___STRING_68___)

    return num
