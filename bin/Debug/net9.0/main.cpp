function main():
    $console = Console.new()
    $console.WriteLine("--- test program ver.0.1 ---")
    Console.Write("Write name: ")

    $name = Console.ReadLine()

    Console.WriteLine("Hello, " + $name + "!")

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