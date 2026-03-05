namespace std: {
    private const richConsole = new RichConsole();
    private class RichConsole: {
        function richTest(): {
            const keywords = ["bold", "dim", "italic", "underline", "blink", "rapid_blink", "reverse", "hidden", "strike"];

            const colors = std::console.getColors();

            let biggestLength = 0;
            keywords.forEach(function(const keyword): {
                biggestLength = std::math.max(biggestLength, keyword.length());
            });

            colors.forEach(function(const color): {
                biggestLength = std::math.max(biggestLength, color.length());
            });

            std::console.println("\nRich tag test:");
            std::console.println(new String("-", 10));

            keywords.forEach(function(const keyword): {
                const spaces = new String(" ") * std::math.max(0, biggestLength - keyword.length());

                richPrint(`{keyword}:{spaces.toString()} [{keyword}]Hello, World![/{keyword}]\n`);
            });

            std::console.println();

            colors.forEach(function(const keyword): {
                const spaces = new String(" ") * std::math.max(0, biggestLength - keyword.length());

                richPrint(`{keyword}:{spaces.toString()} [color={keyword}]Hello, World![/color]\n`);
            });


            std::console.println(new String("-", 10) + "\n");
        }

        function richPrint(const<String> message): {
            const length = message.length();
            
            let isCode = false;
            let isValue = false;
            let keyword = "";
            let value = "";
            let text = "";
            
            for let i = 0; i < length; i++: {
                const el = message.charAt(i);

                switch el: {
                    case "[": {
                        isCode = true;

                        if text != "": {
                            std::console.print(text);
                            text = "";
                        }
                    }
                    case "]": {
                        isCode = false;
                        isValue = false;
                    }
                    case "=": if isCode: isValue = true;
                    default: {
                        if isCode == false:
                            text += el;
                        else if isValue:
                            value += el;
                        else: keyword += el;
                    }
                }

                if keyword != "" && isCode == false: {
                    useCode(keyword, value);

                    keyword = "";
                    value = "";
                }
            }

            if text != "": {
                std::console.print(text);
                text = "";
            }
        }

        private function useCode(let<String> keyword, const<String> value): {
            if keyword == "":
                throw.message("Keyword cannot be empty.");

            const isClose = keyword.charAt(0) == "/";

            if isClose:
                keyword = keyword.subString(1, keyword.length() - 1);

            switch keyword: {
                case "color": {
                    if isClose: 
                        std::console.resetColors();
                    else: std::console.setForeColor(value);
                }
                case "bold":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[1m"));
                case "dim":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[2m"));
                case "italic":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[3m"));
                case "underline":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[4m"));
                case "blink":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[5m"));
                case "rapid_blink":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[6m"));
                case "reverse":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[7m"));
                case "hidden":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[8m"));
                case "strike":
                    std::console.print(boolCase(isClose, "\u001b[0m", "\u001b[9m"));
                default: {
                    if value != "": 
                        std::console.print(`[{keyword}={value}]`);
                    else: std::console.print("[" + boolCase(isClose, "/", "") + `{keyword}]`);
                }
            }
        }
    }
}