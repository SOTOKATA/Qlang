import "import"

namespace terminal: {
    function<Number> menu(const params): {
        if (meta::cls::hasVariable(params, "items") == false):
            std::Throw.message("Undefined items object");

        const items = params.items;

        if items.length() == 0:
            std::Throw.message("Cannot show empty menu");

        let range = items.length();
        if (meta::cls::hasVariable(params, "range")):
            range = params.range;

        let title = "";
        if (meta::cls::hasVariable(params, "title")):
            title = `[color=yellow]{params.title}[/color]`;

        const position = std::Console.getCursorPosition();
        const maxLength = items.select(fn(const x): return x.length();).max();
        let selected = 0;

        const itemsLength = items.length();
        std::Console.cursorVisible(false);
        while true: {
            std::Console.setCursorPosition(position.x, position.y);
            std::Console.richPrint(`{title}`);

            for let i = std::Math.max(0, selected - range); i < std::Math.min(itemsLength, range + selected); i++: {
                const displayIndex = std::Math.max((i - selected), 0);
                std::Console.setCursorPosition(position.x, position.y + displayIndex + 1);
                const item = items.at(i);

                if i == selected:
                    std::Console.richPrint(`[color=green]> {i + 1}. {item}[/color]` + String.new(" ", maxLength));
                else: std::Console.richPrint((i + 1) + ". " + item + String.new(" ", maxLength));
            }

            const input = std::Console.readkey(true);
            switch (input): {
                    case "DOWNARROW": {
                        if selected + 1 >= itemsLength:
                            selected = 0;
                        else:
                            selected++;
                    }
                    case "UPARROW": {
                        if selected - 1 < 0:
                            selected = itemsLength - 1;
                        else:
                            selected--;
                    }
                    case "ENTER": {
                        std::Console.cursorVisible(true);
                        return selected;
                    }
                }
        }
    }
}