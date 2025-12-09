// Class to make operations with console
class Console: {
    // Print text to console
    function print(let message): {
        message = String.getPrimitive(message);

        _native("cmd_write", _str(message));
    }

    // Print text to console with new line
    function println(let message): {
        message = String.getPrimitive(message);

        _native("cmd_write", _str(message + "\n"));
    }

    // Print Verbatim text to console
    function printVerbatim(let message): {
        message = String.getPrimitive(message);

        _native("cmd_write", message);
    }

    // Print Verbatim text to console with new line
    function printlnVerbatim(let message): {
        message = String.getPrimitive(message);

        _native("cmd_write", message + _str("\n"));
    }

    // Read line from console
    function readln(): {
        return String.new(_native("cmd_read"));
    }

    function readkey(let intercept): {
        return String.new(_native("cmd_key", intercept));
    }

    function isKeyAvailable(): {
        return _native("cmd_key_available");
    }

    function cursorVisible(let visisble): {
        _native("cmd_cursor_visible", visible);
    }

    // Clear console
    function clear(): {
        _native("cmd_clear");
    }

    // Set cursor position in console
    function setCursorPosition(let x, let y): {
        if Number.isNumber(x) == false: {
            Throw.exception("Object is not a number");
        }

        if Number.isNumber(y) == false: {
            Throw.exception("Object is not a number");
        }

        _native("cmd_cursor_position", x, y);
    }

    // Set foreground color for console
    function setForeColor(let color): {
        color = String.getPrimitive(color);

        _native("cmd_fcolor", color);
    }
    
    // Set background color for console
    function setBackColor(let color): {
        color = String.getPrimitive(color);

        _native("cmd_bcolor", color);
    }
    // Set default colors for console
    function resetColors(): {
        _native("cmd_reset_colors");
    }
}