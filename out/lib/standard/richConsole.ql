namespace std: {
    private class RichConsole: {
        function richTest(): {
            const keywords = ["bold", "dim", "italic", "underline", "blink", "rapid_blink", "reverse", "hidden", "strike"];

            const colors = std::Console.getColors();

            let biggestLength = 0;
            keywords.forEach(function(const keyword): {
                biggestLength = std::Math.max(biggestLength, keyword.length());
            });

            colors.forEach(function(const color): {
                biggestLength = std::Math.max(biggestLength, color.length());
            });

            std::Console.println("\nRich tag test:");
            std::Console.println(new String("-", 10));

            keywords.forEach(function(const keyword): {
                const spaces = new String(" ") * std::Math.max(0, biggestLength - keyword.length());

                richPrint(`{keyword}:{spaces.toString()} [{keyword}]Hello, World![/{keyword}]\n`);
            });

            std::Console.println();

            colors.forEach(function(const keyword): {
                const spaces = new String(" ") * std::Math.max(0, biggestLength - keyword.length());

                richPrint(`{keyword}:{spaces.toString()} [color={keyword}]Hello, World![/color]\n`);
            });


            std::Console.println(new String("-", 10) + "\n");
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
                            std::Console.print(text);
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
                std::Console.print(text);
                text = "";
            }
        }

        private function useCode(let<String> keyword, const<String> value): {
            if keyword == "":
                Throw.message("Keyword cannot be empty.");

            const isClose = keyword.charAt(0) == "/";

            if isClose:
                keyword = keyword.subString(1, keyword.length() - 1);

            switch keyword: {
                case "color": {
                    if isClose: 
                        std::Console.resetColors();
                    else: std::Console.setForeColor(value);
                }
                case "bold":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[1m"));
                case "dim":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[2m"));
                case "italic":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[3m"));
                case "underline":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[4m"));
                case "blink":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[5m"));
                case "rapid_blink":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[6m"));
                case "reverse":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[7m"));
                case "hidden":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[8m"));
                case "strike":
                    std::Console.print(boolCase(isClose, "\u001b[0m", "\u001b[9m"));
                default: {
                    if value != "": 
                        std::Console.print(`[{keyword}={value}]`);
                    else: std::Console.print("[" + boolCase(isClose, "/", "") + `{keyword}]`);
                }
            }
        }
    }
}