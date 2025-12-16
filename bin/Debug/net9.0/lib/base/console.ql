include "$lib/base"

// Class to make operations with console
class Console: {
    // Print text to console
    function print(let message): {
        if (Object.isSimplify(message) == false):
            message = message.toString();

        _native("lib.console.write", _str(message));
    }

    // Print text to console with new line
    function println(let message = ""): {
        if (Object.isSimplify(message) == false):
            message = message.toString();

        _native("lib.console.write", _str(message + "\n"));
    }

    // Print Verbatim text to console
    function printVerbatim(let message): {
        if (Object.isSimplify(message) == false):
            message = message.toString();

        _native("lib.console.write", message);
    }

    // Print Verbatim text to console with new line
    function printlnVerbatim(let message = ""): {
        if (Object.isSimplify(message) == false):
            message = message.toString();

        _native("lib.console.write", message + _str("\n"));
    }

    // Read line from console
    function readln(): {
        return String.new(_native("lib.console.read"));
    }

    function readkey(let intercept = false): {
        return String.new(_native("lib.console.key", intercept));
    }

    function isKeyAvailable(): {
        return _native("lib.console.key_available");
    }

    function cursorVisible(const visible): {
        _native("lib.console.cursor_visible", visible);
    }

    // Clear console
    function clear(): {
        _native("lib.console.clear");
    }

    // Set cursor position in console
    function setCursorPosition(let x, let y): {
        if Number.isNumber(x) == false: {
            Throw.exception("Object is not a number");
        }

        if Number.isNumber(y) == false: {
            Throw.exception("Object is not a number");
        }

        x = Parser.asInt(x);
        y = Parser.asInt(y);


        _native("lib.console.cursor_position", x, y);
    }

    // Set foreground color for console
    function setForeColor(let color): {
        color = String.getPrimitive(color);

        _native("lib.console.foreground", color);
    }
    
    // Set background color for console
    function setBackColor(let color): {
        color = String.getPrimitive(color);

        _native("lib.console.background", color);
    }
    // Set default colors for console
    function resetColors(): {
        _native("lib.console.reset_color");
    }

    function width(): 
        return _native("lib.console.width");

    function height(): 
        return _native("lib.console.height");
}