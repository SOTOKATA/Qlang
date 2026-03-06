import "import"
using std;

namespace terminal: {
    private function evaluateMenuClass(const cls): {
        return {
            let numeration = hasOrDefault(cls, "numeration", true),
            let title = hasOrDefault(cls, "title", ""),
            let range = hasOrDefault(cls, "range", cls.items.length()),
            let selected = hasOrDefault(cls, "selected", "\t>"),
            let highlightColor = hasOrDefault(cls, "highlightColor", "green"),
            const items = cls.items
        };
    }

    private function hasOrDefault(const cls, const name, const defValue): {
        if meta::cls::hasVariable(cls, name):
            return meta::cls::getVariableValue(cls, name);
        return defValue;
    }

    function<Number> menu(let params): {
        if (meta::cls::hasVariable(params, "items") == false):
            throw.message("Undefined items object");

        if params.items.length() == 0:
            throw.message("Cannot show empty menu");

        params = evaluateMenuClass(params);

        params.title = `[color=yellow]{params.title}[/color]`;

        const position = console.getCursorPosition();
        const maxLength = params.items.select(fn(const x): return x.length();).max();
        let selected = 0;

        const itemsLength = params.items.length();
        console.cursorVisible(false);
        while true: {
            console.setCursorPosition(position.x, position.y);
            console.richPrint(`{params.title}`);

            for let i = 0; i < params.range; i++: {
                console.setCursorPosition(position.x, position.y + i + 1);
                console.print(new String(" ", maxLength) + "  " + new String(" ", <Number>`{(i + 1)}`));
            }

            console.setCursorPosition(position.x, position.y);


            for let i = math.max(0, selected - params.range); i < math.min(itemsLength, params.range + selected); i++: {
                const displayIndex = math.max((i - selected), 0);
                const num = boolCase(params.numeration == true, i + 1 + ". ", "");
                console.setCursorPosition(position.x, position.y + displayIndex + 1);
                const item = params.items.at(i);

                if i == selected:
                    console.richPrint(`[color={params.highlightColor}]{params.selected}{num}{item}[/color]` + new String(" ", maxLength));
                else: console.richPrint(num + item + new String(" ", maxLength));
            }

            const input = console.readkey(true);
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
                        console.cursorVisible(true);
                        return selected;
                    }
                }
        }
    }
}