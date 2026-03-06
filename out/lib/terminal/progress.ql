import "import"
using std;

namespace terminal: {
    class Progress: {
        private let _currentValue;
        private let _size;
        private let _position;

        function new(const<Number> size): {
            _currentValue = 0.0;
            _size = math.max(size, 6);
            _position = console.getCursorPosition();
            console.println("Initial Position: " + _position.x + " " + _position.y);
        }

        private function _progress(): {
            console.cursorVisible(false);

            const blockCount = _currentValue / 100.0 * _size - 2;
            const semiBlockCount = math.round(blockCount / 2);

            let emptyDots = "";
            if blockCount + 2 < _size:
                const emptyDots = new String(".", _size - blockCount - 2); 

            console.setCursorPosition(_position.x, _position.y);
            if semiBlockCount > 0:
                console.richPrint("[" + new String("#", blockCount) + emptyDots + "]");
            else: console.richPrint("[]");

            console.println();
            console.cursorVisible(true);
        }

        private function<Number> _toNormal(const<Number> value): {
            return math.min(math.max(value, 0.0), 100.0);
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