include "$lib/standard"
include "$lib/core"
include "$lib/meta"

// Class to make operations with console
namespace std:  {
    class Console: {
        private function getStr(const message): {
            if (core::Object.isNull(message)):
                return "null";
            if (core::Object.isSimplify(message) == false): {
                // if (Meta.getMethodListOf(message).contains("toString")): 
                //     return message.toString();
                // else: 
                    return str(message);
            }

            return message;
        }

        // Print text to console
        function print(let message): {
            message = getStr(message);

            _native("lib.console.write", _str(message));
        }

        // Print text to console with new line
        function println(let message = ""): {
            message = getStr(message);

            _native("lib.console.write", _str(message + "\n"));
        }

        // Print Verbatim text to console
        function printVerbatim(let message): {
            message = getStr(message);

            _native("lib.console.write", message);
        }

        // Print Verbatim text to console with new line
        function printlnVerbatim(let message = ""): {
            message = getStr(message);

            _native("lib.console.write", message + _str("\n"));
        }

        // Read line from console
        function readln(): {
            return core::String.new(_native("lib.console.read"));
        }

        function readkey(const<core::Boolean> intercept = false): {
            return core::String.new(_native("lib.console.key", intercept));
        }

        function isKeyAvailable(): {
            return _native("lib.console.key_available");
        }

        function cursorVisible(const<core::Boolean> visible): {
            _native("lib.console.cursor_visible", visible);
        }

        // Clear console
        function clear(): {
            _native("lib.console.clear");
        }

        // Set cursor position in console
        function setCursorPosition(let<core::Number> x, let<core::Number> y): {
            x = Parser.asInt(x);
            y = Parser.asInt(y);

            _native("lib.console.cursor_position", x, y);
        }

        // Set foreground color for console
        function setForeColor(let<core::String> color):
            _native("lib.console.foreground", color);
        
        // Set background color for console
        function setBackColor(let<core::String> color):
            _native("lib.console.background", color);

        // Set default colors for console
        function resetColors():
            _native("lib.console.reset_color");

        function width(): 
            return _native("lib.console.width");

        function height(): 
            return _native("lib.console.height");
    }
}