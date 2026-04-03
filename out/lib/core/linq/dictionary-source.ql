import "$lib/standard"
import "$lib/core/linq"
using std;

namespace linq: {
    class DictionarySource extends linq::Enumerable: {
        private let<Dictionary> _dict = new Dictionary();
        private let<Number> _index = -1;

        function new(<Dictionary> dict):
            _dict = dict;

        function new(a, b):
            throw.message("Cannot instantiate DictionarySource with Enumerable params.");

        function next(): {
            const keys = _dict.getKeys();
            const values = _dict.getValues();
            const len = keys.length;

            if _index + 1 < len: {
                _index++;
                return { key = keys.at(_index), value = values.at(_index) };
            }

            return null;
        }
    }
}