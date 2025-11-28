include "$lib/base"

class LanguageDemo: {
    function run(): {

        Console.println("<--- Menu demo --->");
        Console.println("1. Numeric");
        Console.println("2. Strings");
        Console.println("3. Circles");
        
        // TODO: { bool re-write parsing (with || and &&)
        do_while (String.isNullOrWhitespace(choice) == true) || (Number.isNumber(choice) == false): {
            Console.print("Print choice (1-3): ");
            let choice = Console.readln();

            if String.isNullOrWhitespace(choice) == true: {
                printException("You print empty string!");
            }

            if Number.isNumber(choice) == false: {
                printException("You print not number!");
            }
        }

        if choice == "1": {
            numericDemo();
        }
        else if choice == "2": {
            stringDemo();
        }
        else if choice == "3": {
            circleDemo();
        }

        Console.println("\nCurrent presentation ended");
    }

    function stringDemo(): {
        Console.println("");

        do_while String.isNullOrWhitespace(input) == true: { 
            Console.print("Print anything: ");
            let input = Console.readln();

            if String.isNullOrWhitespace(input) == true: {
                printException("You must to print anything!");
            }
        }

        input = String.new(input);
        
        Console.println("\nInput length: " + input.length());
        Console.println("Input trim: '" + input.trim() + "'");
        Console.println("Input trim start: '" + input.trimStart() + "'");
        Console.println("Input trim end: '" + input.trimEnd() + "'");
        
        if input.length() > 2: {
            Console.println("Input first two characters: '" + input.subString(0, 2) + "'");
        }
    }

    function numericDemo(): {
        Console.println("\nWelcome to numeric demo!");

        let num1 = getNumberFromConsole("Write first number: ");
        let num2 = getNumberFromConsole("Write second number: ");

        Console.println("\nFirst number: " + num1);
        Console.println("Second number: " + num2);

        Console.println("\n<--- Math --->");
        Console.println("Sum: " + (num1 + num2));
        Console.println("Substration: " + (num1 - num2));
        Console.println("Multiplication: " + (num1 * num2));

        if num2 != 0: {
            Console.println("Division: " + Number.toFixed((num1 / num2), "0.00"));
        }
        else: {
            printException("Division: Can't divide by 0!");
        }

        if num1 < num2: {
            Console.println("Random number (range: " + num1 + "-" + num2 + "): " + Number.randInt(num1, num2));
        }
        else: {
            printException("Random number: Error, first number can't be more than or equal second");
        }

        for let i = num1 - 10; i < num1; i = i + 1: {
            Console.println("Index: " + i);
        }

        Console.println("\n<--- Number class --->");
        Console.println("First number as integer: " + Number.toFixedInt(num1));
        Console.println("Second number as integer: " + Number.toFixedInt(num2));

        Console.println("\nFirst number with pattern '0.00': " + Number.toFixed(num1, "0.00"));
        Console.println("Second number with pattern '0.00': " + Number.toFixed(num2, "0.00"));

        Console.println("\nMinimum number value: " + Number.MIN_VALUE);
        Console.println("Maximum number value: " + Number.MAX_VALUE);
    }

    function circleDemo(): {
        Console.println("\nWelcome to circles demo!");

        Console.println("\nWhile example (while random number is not 0): ");
        Console.print("Numbers: ");
        
        let randNum = 0-1;
        
        while randNum != 0: {
            randNum = Number.randInt(0, 3);
            Console.print(randNum + ", ");
        }

        Console.println("Ocurre 0!");
        Console.println("You exit from while.");

        acceptContinue();

        Console.println("\ndo-While example: ");
        do_while input != "0": {
            Console.print("Type '0' to break do-While: ");
            let input = Console.readln();
        }
        Console.println("You exit from do_while.");

        acceptContinue();

        Console.println("For example (0-10): ");
        
        for let i = 0; i < 10; i = i + 1: {
            Console.println("Index: " + i);
        }
    }

    function acceptContinue(): {
        Console.println("\nPress enter to continue");
        Console.readkey();
    }

    function printException(let msg): {
        Console.setForeColor("red");
        Console.println(msg);
        Console.resetColors();
    }

    function getNumberFromConsole(let msg): {
        let num = "";

        do_while Number.isNumber(num) == false: {
            Console.print(msg);
            num = Console.readln();

            if Number.isNumber(num) == false: {
                printException("Input is not number!");
            }
        }

        return Parser.asNumber(num);
    }
}