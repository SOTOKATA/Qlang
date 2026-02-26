namespace terminal: {
    function menu(const params): {
        if (meta::cls::hasVariable(params, "items") == false):
            Throw.message("Undefined items object");

        if (meta::cls::hasVariable(params, "title")):
            std::console::richPrint($"[color=yellow]{params.title}[/color]\n");

        const items = params.items;
        const len = items.length();

        for let i = 0; i < len; i++:
            std::console::richPrint($"[color=green]{i + 1}.[/color] {items.at(i).at(0)}\n");

        const rangeStr = boolCase(len == 1, "1", $"{1} - {len}");

        let selected = "";

        const range = items.getIndexes().select(function(const i): return i + 1;);

        while range.firstOrDefault(function(const item): return item == selected;) == null: {

            std::console::richPrint($"Enter [color=green]{rangeStr}[/color]: ");
            selected = std::console::readln();
        }

        const func = items.at(std::Parser.asNumber(selected) - 1).at(1);

        func();
    }
}