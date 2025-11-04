function main():
    $num1 = this.ask("Type first number: ")

    $num2 = this.ask("Type second number: ")

    $operator = this.ask("Type operator: ")

    if $operator == "+":
        $result = Math.sum($num1, $num2)
        Term.println($num1, $operator, $num2, "=", $result)
    else if $operator == "-":
        $result = Math.sub($num1, $num2)
        Term.println($num1, $operator, $num2, "=", $result)
    else if $operator == "*":
        $result = Math.mult($num1, $num2)
        Term.println($num1, $operator, $num2, "=", $result)
    else if $operator == "/":
        if $num2 == "0":
            Term.println("Error: can't divide by 0")
        else:
            $result = Math.div($num1, $num2)
            Term.println($num1, $operator, $num2, "=", $result)
    else:
        Term.println("Error: operator is not identificated!")

function ask($message):
    Term.print($message)
    return Term.read()