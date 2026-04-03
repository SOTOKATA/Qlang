import "$lib/standard"
import "$lib/core/linq"
using std;

namespace linq: {
    class ArraySource extends linq::Enumerable: {
        private let<Array|null> _arr;
        private let<Number> _index = -1;

        function new(<Array|Collection> arr):
            _arr = new Array(arr);

        function new(a, b):
            throw.message("Cannot instantiate ArraySource with Enumerable params.");

        function next(): {
            if _index + 1 < _arr.length: {
                _index++;
                return _arr.at(_index);
            }

            return null;
        }
    }
}