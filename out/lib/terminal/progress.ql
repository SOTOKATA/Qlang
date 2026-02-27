import "import"

namespace terminal: {
    function progress(const progressFunction, let<Number> size = 10): {
        if typeof(progressFunction) != "~function":
            std::Throw.message("Cannot use non function type: " + Object.toString(typeof(progressFunc)));

        size = std::Math.max(6, size);

        let current = 0;

        const position = std::Console.getCursorPosition();

        std::Console.cursorVisible(false);

        while current < 100.0: {
            current = std::Math.min(100.0, progressFunction(current));
            const blockCount = current / 100.0 * size - 2;
            const semiBlockCount = std::Math.round(blockCount / 2); 
            
            const currentPosition = std::Console.getCursorPosition();

            let emptyDots = "";
            if blockCount + 2 < size:
                const emptyDots = String.new(".", size - blockCount - 2); 

            std::Console.setCursorPosition(position.x, position.y);
//█
            if semiBlockCount > 0:
                std::Console.richPrint("[" + String.new("#", blockCount) + emptyDots + "]");
            else: std::Console.richPrint("[]");
        }

        std::Console.cursorVisible(true);
    }
}