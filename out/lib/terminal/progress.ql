import "import"

namespace terminal: {
    class Progress: {
        private let _currentValue;
        private let _size;
        private let _position;

        function new(const<Number> size): {
            _currentValue = 0.0;
            _size = std::math.max(size, 6);
            _position = std::console.getCursorPosition();
            std::console.println("Initial Position: " + _position.x + " " + _position.y);
        }

        private function _progress(): {
            std::console.cursorVisible(false);

            const blockCount = _currentValue / 100.0 * _size - 2;
            const semiBlockCount = std::math.round(blockCount / 2);

            let emptyDots = "";
            if blockCount + 2 < _size:
                const emptyDots = new String(".", _size - blockCount - 2); 

            std::console.setCursorPosition(_position.x, _position.y);
            if semiBlockCount > 0:
                std::console.richPrint("[" + new String("#", blockCount) + emptyDots + "]");
            else: std::console.richPrint("[]");

            std::console.println();
            std::console.cursorVisible(true);
        }

        private function<Number> _toNormal(const<Number> value): {
            return std::math.min(std::math.max(value, 0.0), 100.0);
        }

        function add(const<Number> value): {
            _currentValue += _toNormal(value);

            _progress();
        }

        function reduce(const<Number> value): {
            _currentValue -= _toNormal(value);

            _progress();
        }

        function set(const<Number> value): {
            _currentValue = _toNormal(value);

            _progress();
        }

    }
}