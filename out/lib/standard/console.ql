import "$lib/standard"
import "$lib/core"
import "$lib/meta"

// Class to make operations with console
namespace std:  {
    class Console: {
        private function getStr(const message): {
            if (Object.isNull(message)):
                return "null";
                
            if (Object.isSimplify(message) == false): {
                if (meta::Meta.getFunctionListOf(message).contains("toString")): 
                    return message.toString();
                else: 
                    return str(message);
            }

            return message;
        }

        // Print text to console
        function print(let message): {
            message = getStr(message);

            _native("std.console.write", _str(message));
        }

        // Print text to console with new line
        function println(let message = ""): {
            message = getStr(message);

            _native("std.console.write", _str(message + "\n"));
        }

        // Print Verbatim text to console
        function printVerbatim(let message): {
            message = getStr(message);

            _native("std.console.write", message);
        }

        // Print Verbatim text to console with new line
        function printlnVerbatim(let message = ""): {
            message = getStr(message);

            _native("std.console.write", message + _str("\n"));
        }

        // Read line from console
        function readln(): {
            return String.new(_native("std.console.read"));
        }

        function readkey(const<Boolean> intercept = false): {
            return String.new(_native("std.console.key", intercept));
        }

        function isKeyAvailable(): {
            return _native("std.console.key_available");
        }

        function cursorVisible(const<Boolean> visible): {
            _native("std.console.cursor_visible", visible);
        }

        // Clear console
        function clear(): {
            _native("std.console.clear");
        }

        // Set cursor position in console
        function setCursorPosition(let<Number> x, let<Number> y): {
            x = Parser.asInt(x);
            y = Parser.asInt(y);

            _native("std.console.cursor_position", x, y);
        }

        // Set foreground color for console
        function setForeColor(let<String> color):
            _native("std.console.foreground", color);
        
        // Set background color for console
        function setBackColor(let<String> color):
            _native("std.console.background", color);

        // Set default colors for console
        function resetColors():
            _native("std.console.reset_color");

        function width(): 
            return _native("std.console.width");

        function height(): 
            return _native("std.console.height");
    }
}