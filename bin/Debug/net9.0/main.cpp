function main():
    Term.println("{While} demo:\n")

    $num1 = 0

    Term.println("To end while cyrcle type 'Esc'")
    while $num1 < 10:
        $tblock = 0
        while $tblock < $num1:
            $tblock = Math.sum($tblock, 1)
            Term.print("\t")

        $num1 = Math.sum(1, $num1)
        Term.println("Number: " + $num1)

    Term.println("Execution ended")

function ask($message):
    Term.print($message)
    return Term.read()