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

            #std.Console.Write(_str(message));
        }

        // Print text to console with new line
        function println(let message = ""): {
            message = getStr(message);

            #std.Console.Write(_str(message + "\n"));
        }

        // Print Verbatim text to console
        function printVerbatim(let message): {
            message = getStr(message);

            #std.Console.Write(message);
        }

        // Print Verbatim text to console with new line
        function printlnVerbatim(let message = ""): {
            message = getStr(message);

            #std.Console.Write(message + _str("\n"));
        }

        // Read line from console
        function<String> readln(): {
            return new String(#std.Console.ReadLine());
        }

        function<String> readkey(<Boolean> intercept = false): {
            return new String(#std.Console.ReadKey(intercept));
        }

        function<Boolean> isKeyAvailable() => #std.Console.IsKeyAvailable();

        const cursorVisible = field(_): {
            fn get() => #std.Console.GetCursorVisible()
            fn set(<Boolean> visible): #std.Console.SetCursorVisible(visible);
        };

        // Clear console
        function clear(): #std.Console.Clear();

        // Set cursor position in console
        function setCursorPosition(let<Number> x, let<Number> y): {
            x = math.round(x);
            y = math.round(y);

            if (x < 0 || x >= console.width) || (y < 0 || y >= console.height):
                throw.message("Size is out of range.");

            #std.Console.SetCursorPosition(x, y);
        }

        function getCursorPosition(): => { x = #std.Console.GetCurrentX(), y = #std.Console.GetCurrentY() };

        const foreColor = field(_): {
            fn get() => #std.Console.GetForeground()
            fn set(<String> color): #std.Console.SetForeground(color);
        };

        const backColor = field(_): {
            fn get() => #std.Console.GetBackground()
            fn set(<String> color): #std.Console.SetBackground(color);
        };

        // Get all console colors
        function<Collection> getColors() => #std.Console.GetColors();

        // Set default colors for console
        function resetColors():
            #std.Console.ResetColors();

        const width = field(_): {
            fn get() => #std.Console.GetWidth()
            fn set(<Number> num): #std.Console.SetWidth(num);
        };

        const height = field(_): {
            fn get() => #std.Console.GetHeight()
            fn set(<Number> num): #std.Console.GetHeight(num);
        };

        function richPrint(<String> message):
           richConsole.richPrint(message);

        function richTest(): richConsole.richTest();
    }
}