function main():
    Console.Write("Write number: ")
    
    $name = Console.ReadLine()

    Console.WriteLine(math.square($name))
    
class Console:
    function Write($message):
        Term.print($message)
    
    function WriteLine($message):
        Write($message + "\n")

    function ReadLine():
        return Term.read()

class math:
    function square($num):
        return Math.mult($num, $num)