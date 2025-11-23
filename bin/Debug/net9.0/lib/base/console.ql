// Class to make operations with console
class Console: {
    private let usings = "using System; ";
    private let defFColor = "gray";
    private let defBColor = "black";

    // Print text to console
    function print(let message): {
        _native("cmd_write", _str(message));
        // _csharp(usings + "Console.Write(" + _str_csharp(message) + ")");
    }

    // Print text to console with new line
    function println(let message): {
        print(message + "\n");
    }

    // Read line from console
    function readln(): {
        return _native("cmd_read");
    }

    function readkey(let intercept): {
        return _native("cmd_key", intercept);
    }

    function isKeyAvailable(): {
        return _native("cmd_key_available");
    }

    function cursorVisible(let visible): {
        _native("cmd_cursor_visible", visible);
    }

    // Clear console
    function clear(): {
        _native("cmd_clear");
    }

    // Set cursor position in console
    function setCursorPosition(let x, let y): {
        _native("cmd_cursor_position", x, y);
    }

    // Set foreground color for console
    function setForeColor(let color): {
        _native("cmd_fcolor", color);
    }
    
    // Set background color for console
    function setBackColor(let color): {
        _native("cmd_bcolor", color);
    }
    // Set default colors for console
    function resetColors(): {
        setForeColor(defFColor);
        setBackColor(defBColor);
    }
}