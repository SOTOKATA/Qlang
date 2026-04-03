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
            return _native("std", "console", "keyAvailable");
        }

        const cursorVisible = field(_): {
            fn get() => _native("std", "console", "getCursorVisible")
            fn set(<Boolean> visible): _native("std", "console", "setCursorVisible", visible);
        };

        // Clear console
        function clear(): {
            _native("std", "console", "clear");
        }

        // Set cursor position in console
        function setCursorPosition(let<Number> x, let<Number> y): {
            x = math.round(x);
            y = math.round(y);

            if (x < 0 || x >= console.width) || (y < 0 || y >= console.height):
                throw.message("Size is out of range.");

            _native("std", "console", "cursorPosition", x, y);
        }

        function getCursorPosition(): {
            const x = _native("std", "console", "getCurrentX");
            const y = _native("std", "console", "getCurrentY");

            return { x = x, y = y };
        }

        const foreColor = field(_): {
            fn get() => _native("std", "console", "getForeground")
            fn set(<String> color): _native("std", "console", "setForeground", color);
        };

        const backColor = field(_): {
            fn get() => _native("std", "console", "getBackground")
            fn set(<String> color): _native("std", "console", "setBackground", color);
        };

        // Get all console colors
        function<Collection> getColors() => _native("std", "console", "colors");

        // Set default colors for console
        function resetColors():
            _native("std", "console", "resetColor");

        const width = field(_): {
            fn get() => _native("std", "console", "getWidth")
            fn set(<Number> num): _native("std", "console", "setWidth", num);
        };

        const height = field(_): {
            fn get() => _native("std", "console", "getHeight")
            fn set(<Number> num): _native("std", "console", "setHeight", num);
        };

        function richPrint(<String> message):
           richConsole.richPrint(message);

        function richTest(): richConsole.richTest();
    }
}