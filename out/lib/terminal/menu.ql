import "import"

namespace terminal: {
    function menu(const params): {
        if (meta::cls::hasVariable(params, "items") == false):
            Throw.message("Undefined items object");

        if (meta::cls::hasVariable(params, "title")):
            std::Console.richPrint($"[color=yellow]{params.title}[/color]\n");

        const items = params.items;
        const len = items.length();

        for let i = 0; i < len; i++:
            std::Console.richPrint($"[color=green]{i + 1}.[/color] {items.at(i).at(0)}\n");

        const rangeStr = boolCase(len == 1, "1", $"{1} - {len}");

        let selected = "";

        const range = items.getIndexes().select(function(const i): return i + 1;);

        selected = inputFromRange($"Enter {rangeStr}: ", range, "");

        const func = items.at(std::Parser.asNumber(selected) - 1).at(1);

        if typeof(func) != "~function":
            std::Throw.message("Cannot call non function type: " + Object.toString(typeof(func)));

        func();
    }
}