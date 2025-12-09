include "$lib/base"
include "$lib/filesystem"

class BaseTest: {
    function runTest(): {
        _stringTest();

        _numberTest();

        _mathTest();

        _arrayTest();

        _dictionaryTest();

        _objectTest();

        _regexTest();

        _vectorTest();

        _pathTest();

        _fileTest();
    }

    function _mathTest(): {
        let a = 5.5;
        let b = 1.2;

        printHeader("math");
    
        Console.println("Work with numbers: a = " + a + ", b = " + b);
        Console.println("\nMax: " + Math.max(a, b));
        Console.println("Min: " + Math.min(a, b));
        Console.println("Abs of a (" + a + "): " + Math.abs(a));
    }

    function _pathTest(): {
        let path = ".\\file.com";

        if File.exists(path) == false: {
            File.create(path);
        }

        printHeader("path");

        Console.println("Work with path: '" + path + "'");
        Console.println("\nExists: " + Path.exists(path));

        Console.println("\nExtension: " + Path.getExtension(path));
        Console.println("Has extension: " + Path.hasExtension(path));
        Console.println("Change extension (to '.ql'): " + Path.changeExtension(path, ".ql"));
        
        Console.println("\nFile name (without extension): " + Path.getFileName(path));
        Console.println("Full file name: " + Path.getFullFileName(path));

        File.remove(path);
    }

    function _fileTest(): {
        let path = ".\\file.com";

        printHeader("file");


        if File.exists(path) == false: {
            File.create(path);
            Console.println("File created.");
        } else: {
            Console.println("File already created.");

        }

        File.setContent(path, "EXAMPLE_OF_FILE");

        Console.println("\nWork with path: '" + path + "'");

        Console.println("\nExists: " + File.exists(path));

        Console.println("\nContent: '" + File.getContent(path).str() + "'");

        File.setContent(path, "Hello ");
        Console.println("Set content 'Hello ': '" + File.getContent(path).str() + "'");

        File.appendContent(path, "World!");
        Console.println("Append content 'World!': '" + File.getContent(path).str() + "'");

        File.remove(path);
        Console.println("\nFile removed.");

    }

    private function printHeader(let str): {
        Console.setForeColor("yellow");
        Console.println("\n--- " + str + " test ---\n");
        Console.resetColors();
    }

    // Types test
    private function _numberTest(): {
        let a = 5.5;
        let b = 10;

        printHeader("number");
        Console.println("a = " + a + ", isNumber: " + Number.isNumber(a));
        Console.println("b = " + b + ", isNumber: " + Number.isNumber(b) + "\n");

        Console.println(a + " + " + b + ": " + (a + b));
        Console.println(a + " - " + b + ": " + (a - b));
        Console.println(a + " * " + b + ": " + (a * b));
        Console.println(a + " / " + b + ": " + (a / b));
        Console.println(a + " % " + b + ": " + (a % b));

        Console.println("\nMin value: " + Number.MIN_VALUE);
        Console.println("Max value: " + Number.MAX_VALUE);

        Console.println("\nrandInt (" + a + "-" + b + "): " + Number.randInt(a, b));

        Console.println("\ntoFixed (" + b + ", pattern: 00.000): " + Number.toFixed(b, "00.000"));
        Console.println("toFixedInt (" + a + "): " + Number.toFixedInt(a));
    }

    private function _arrayTest(): {
        let collection = [ "Hello", "World", "!" ];
        let array = Array.new(collection);

        printHeader("array");

        Console.println("Collection: '" + Array.new(collection).toString().str() + "', isCollection: " + Array.isCollection(collection));
        Console.println("Array: '" + array + "', '" + array.toString().str() + "', isArray: " + Array.isArray(array));

        Console.println("\nWork with array: " + array.toString().str());
        Console.println("Contains 'Hello': " + array.contains("Hello"));
        array.push("Amigo");
        Console.println("Push 'Amigo': " + array.toString().str());
        array.setAt(0, "Folder");
        Console.println("SetAt '0, Folder': " + array.toString().str());
        Console.println("At '0': " + array.at(0));
        array.insert(0, "first");
        Console.println("Insert '0, first': " + array.toString().str());
        Console.println("IndexOf 'first': " + array.indexOf("first"));
        array.removeAt(0);
        Console.println("RemoveAt '0': " + array.toString().str());
        Console.println("Length: " + array.length());
        array.clear();
        Console.println("Cleared: " + array.toString().str());


    }

    private function _dictionaryTest(): {
        let dict = Dictionary.new();

        printHeader("dictionary");

        dict.set("KeyItem", "FirstItem");
        Console.println("set 'KeyItem, FirstItem': " + dict.toString().str());
        Console.println("containsKey 'KeyItem': " + dict.containsKey("KeyItem"));
        Console.println("containsValue 'FirstItem': " + dict.containsValue("FirstItem"));
        Console.println("get 'KeyItem': " + dict.get("KeyItem"));
        dict.clear();
        Console.println("Cleared: " + dict.toString().str());
    }

    private function _objectTest(): {
        printHeader("object");

        Console.println("isNull '1': " + Object.isNull(1));
        Console.println("isNull 'null': " + Object.isNull(null));
    }

    private function _stringTest(): {
        let str = String.new(" Hello, World! ");
        let primitive = "Hello, World!";

        printHeader("string");

        Console.println("Primitive string: '" + primitive + "', is primitive: " + String.isPrimitive(primitive));
        Console.println("String class: '" + str + "', '" + str.str() + "', is string: " + String.isString(str));

        Console.println("\nHello + World = " + String.append(["Hello", "World"]).str());

        Console.println("\nWork with string class: '" + str.str() + "'");
        Console.println("length: '" + str.length() + "'");
        Console.println("trim: '" + str.trim().str() + "'");
        Console.println("trimStart: '" + str.trimStart().str() + "'");
        Console.println("trimEnd: '" + str.trimEnd().str() + "'");
        Console.println("toLower: '" + str.toLower().str() + "'");
        Console.println("toUpper: '" + str.toUpper().str() + "'");
        Console.println("isNullOrEmpty: '" + str.isNullOrEmpty(str) + "'");
        Console.println("isNullOrWhitespace: '" + str.isNullOrWhitespace(str) + "'");
        Console.println("subString: '" + str.subString(0, 5).str() + "'");
        Console.println("\nsplit: '" + str.split(" ").toString().str() + "'");
        Console.println("join: '" + String.join(str.split(" "), " ").str() + "'");
    }
    // Types test end

    // Advanced types test 
    private function _vectorTest(): {
        let vect = Vector2.new(5, 10.5);

        printHeader("vector");

        Console.println("Vector: " + vect.toString().str());
        Console.println("X: " + vect.X());
        Console.println("Y: " + vect.Y());
        Console.println("equals '5, 10.5': " + vect.equals(Vector2.new(5, 10.5)));
    }
    // Advanced types test end

    private function _regexTest(): {
        printHeader("regex");

        Console.println("replace 'input: 'Good night!', pattern: 'night', replacement: 'evening'': " + Regex.replace("Good night!", "night", "evening"));
    }
}