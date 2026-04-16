namespace std: {
    private const richConsole = new RichConsole();
    private class RichConsole: {
        function richPrint(<String> message): {
            if message == "":
                return;
            
            std::console.cursorVisible(false);

            let currentIndex = 0;
            const len = message.length;
            let<String> msg = message;

            while currentIndex < len: {
                if currentIndex >= len:
                    break;

                const text = msg.trimStart("[");

                currentIndex += text.length;

                std::console.print(text);

                msg = msg.subString(0, text.length - 1);
                
                console.println("INDEX: " + msg);

                if msg.length > 0 && msg.charAt(0) == "[": {
                    const index = msg.indexOf("]");
                    const code = msg.subString(index, msg.length - index).split("=");

                    msg = msg.subString(0, index);

                    useCode(code.at(0), if code.length >= 2 ? code.at(1) : "");
                }
            }
        }

        function old_richPrint(<String> message): {
            std::console.cursorVisible(false);

            const len = message.length;
            
            let isCode = false;
            let isValue = false;
            let keyword = "";
            let value = "";
            let text = "";
            
            for let i = 0; i < len; i++: {
                const el = message.charAt(i);

                switch el: {
                    case "[": {
                        isCode = true;

                        if text != "": {
                            console.print(text);
                            text = "";
                        }
                    }
                    case "]": {
                        isCode = false;
                        isValue = false;
                    }
                    case "=": if isCode: isValue = true;
                    default: {
                        if !isCode:
                            text += el;
                        else if isValue:
                            value += el;
                        else: keyword += el;
                    }
                }

                if keyword != "" && !isCode: {
                    useCode(keyword, value);

                    keyword = "";
                    value = "";
                }
            }

            if text != "": {
                std::console.print(text);
                text = "";
            }
    
            std::console.cursorVisible(true);
        }

        private function useCode(let<String> keyword, const<String> value): {
            if keyword == "":
                throw.message("Keyword cannot be empty.");

            const isClose = keyword.charAt(0) == "/";

            if isClose: {
                keyword = keyword.subString(1, keyword.length - 1);
                std::console.print("\u001b[0m");
                return;
            }

            if keyword == "color": {
                if isClose: 
                    std::console.resetColors();
                else: std::console.setForeColor(value);

                return;
            }

            std::console.print(switch keyword: {
                "bold" => "\u001b[1m",
                "dim" => "\u001b[2m",
                "italic" => "\u001b[3m",
                "underline" => "\u001b[4m",
                "blink" => "\u001b[5m",
                "rapid_blink" => "\u001b[6m",
                "reverse" => "\u001b[7m",
                "hidden" => "\u001b[8m",
                "strike" => "\u001b[9m",
                "new_line" => "\n",
                "tab" => "\t",
                default => if value != "" ? `[{keyword}={value}]` : `[{if isClose ? "/" : ""}{keyword}]`
            });
        }
    }
}