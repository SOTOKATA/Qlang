namespace std: {
    private const richConsole = new RichConsole();
    private class RichConsole: {
        function richTest(): {
            const keywords = ["bold", "dim", "italic", "underline", "blink", "rapid_blink", "reverse", "hidden", "strike"];

            const colors = std::console.getColors();

            let biggestLength = math.max(keywords.select(fn(x) => x.length()).max(), colors.select(fn(x) => x.length()).max());

            std::console.println("\nRich tag test:");
            std::console.println(new String("-", 10));

            keywords.forEach(function(keyword): {
                const spaces = new String(" ") * std::math.max(0, biggestLength - keyword.length());

                richPrint(`{keyword}:{spaces.toString()} [{keyword}]Hello, World![/{keyword}]\n`);
            });

            std::console.println();

            colors.forEach(function(keyword): {
                const spaces = new String(" ") * std::math.max(0, biggestLength - keyword.length());

                richPrint(`{keyword}:{spaces.toString()} [color={keyword}]Hello, World![/color]\n`);
            });


            std::console.println(new String("-", 10) + "\n");
        }

        function richPrint(<String> message): {
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
        }

        private function useCode(let<String> keyword, const<String> value): {
            if keyword == "":
                throw.message("Keyword cannot be empty.");

            const isClose = keyword.charAt(0) == "/";

            if isClose: {
                keyword = keyword.subString(1, keyword.length() - 1);
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