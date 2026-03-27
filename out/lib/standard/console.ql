import "$lib/standard"
import "$lib/core"
import "$lib/meta"

// Class to make operations with console
namespace std:  {
    const console = new Console();
    private class Console: {
        private function<String> getStr(message): {
            if (typeof(message)).startsWith("~object"):
                return new Dictionary(message).toString();

            if !object.isSimplify(message):
                return message.toString();

            return str(message);
        }

        // Print text to console
        function print(let message): {
            message = getStr(message);

            _native("std", "console", "write", _str(message));
        }

        // Print text to console with new line
        function println(let message = ""): {
            message = getStr(message);

            _native("std", "console", "write", _str(message + "\n"));
        }

        // Print Verbatim text to console
        function printVerbatim(let message): {
            message = getStr(message);

            _native("std", "console", "write", message);
        }

        // Print Verbatim text to console with new line
        function printlnVerbatim(let message = ""): {
            message = getStr(message);

            _native("std", "console", "write", message + _str("\n"));
        }

        // Read line from console
        function<String> readln(): {
            return new String(_native("std", "console", "read"));
        }

        function<String> readkey(<Boolean> intercept = false): {
            return new String(_native("std", "console", "key", intercept));
        }

        function<String> isKeyAvailable(): {
            return _native("std", "console", "key_available");
        }

        function cursorVisible(<Boolean> visible): {
            _native("std", "console", "cursor_visible", visible);
        }

        // Clear console
        function clear(): {
            _native("std", "console", "clear");
        }

        // Set cursor position in console
        function setCursorPosition(let<Number> x, let<Number> y): {
            x = math.round(x);
            y = math.round(y);

            if (x < 0 || x >= console.width()) || (y < 0 || y >= console.height()):
                throw.message("Size is out of range.");

            _native("std", "console", "cursor_position", x, y);
        }

        function getCursorPosition(): {
            const x = _native("std", "console", "get_current_x");
            const y = _native("std", "console", "get_current_y");

            return { const x = x, const y = y };
        }

        // Set foreground color for console
        function setForeColor(let<String> color):
            _native("std", "console", "foreground", color);
        
        // Set background color for console
        function setBackColor(let<String> color):
            _native("std", "console", "background", color);

        // Get all console colors
        function<Collection> getColors() => _native("std", "console", "colors");

        // Set default colors for console
        function resetColors():
            _native("std", "console", "reset_color");

        function width() => _native("std", "console", "width");

        function height() => _native("std", "console", "height");


        function richPrint(<String> message):
           richConsole.richPrint(message);

        function richTest(): richConsole.richTest();
    }
}