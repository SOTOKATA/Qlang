namespace std: {
    private const richConsole = new RichConsole();
    private class RichConsole: {
        function richPrint(<String> message): {
            if message == "": return;
            message = "text: [color=yellow]Yello text[/color]";
            std::console.cursorVisible(false);
            let<String> msg = message;

            let indexofw = 0;

            try: {
                while msg.length() > 0: {
                    indexofw++;
                    const openIndex = msg.indexOf("[");

                    if openIndex == -1: {
                        std::console.print(msg);
                        break; 
                    }

                    if openIndex > 0: {
                        std::console.print(msg.subString(0, openIndex));
                        // Отрезаем напечатанное
                        msg = msg.subString(openIndex, msg.length() - openIndex);
                    }

                    // Теперь msg ГАРАНТИРОВАННО начинается с "["
                    const closeIndex = msg.indexOf("]");
                    
                    if closeIndex == -1: {
                        std::console.print(msg);
                        break;
                    }

                    // ИЗВЛЕКАЕМ ТЕГ
                    // Индекс 0 это '[', индекс closeIndex это ']'
                    // Длина содержимого между ними: closeIndex - 1
                    const tagContent = msg.subString(1, closeIndex - 1);
                    
                    const parts = tagContent.split("=");
                    const keyword = parts.at(0);
                    const value = if parts.length() >= 2 ? parts.at(1) : "";

                    useCode(keyword, value);

                    // ОТРЕЗАЕМ ТЕГ
                    const nextStart = closeIndex + 1;
                    const remainingLength = msg.length() - nextStart;

                    if remainingLength <= 0: {
                        break;
                    }
                    
                    // Вот тут могла быть ошибка, если неправильно рассчитать длину
                    msg = msg.subString(nextStart, remainingLength);
                }
            }
            catch(const error): {
                console.println(`\nDebugInfo: Index of while: {indexofw}\nmsg: {msg}\nopenIndex: {openIndex}\n`);
                throw.exception(error);
            }

            std::console.cursorVisible(true);
        }

        function static_richPrint(<String> message): {
            if message == "":
                return;
            
            std::console.cursorVisible(false);

            let currentIndex = 0;
            const length = message.length();
            let<String> msg = message;

            while currentIndex < length: {
                if currentIndex >= length:
                    break;

                const text = msg.trimStart("[");

                currentIndex += text.length();

                std::console.print(text);

                msg = msg.subString(0, text.length() - 1);
                
                console.println("INDEX: " + msg);

                if msg.length() > 0 && msg.charAt(0) == "[": {
                    const index = msg.indexOf("]");
                    const code = msg.subString(index, msg.length() - index).split("=");

                    msg = msg.subString(0, index);

                    useCode(code.at(0), if code.length() >= 2 ? code.at(1) : "");
                }
            }
        }

        function old_richPrint(<String> message): {
            std::console.cursorVisible(false);

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
    
            std::console.cursorVisible(true);
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