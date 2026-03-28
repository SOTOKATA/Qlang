import "$lib/standard"
import "$lib/core/linq"
using std;

namespace linq: {
    class SelectEnumerable extends linq::Enumerable: {
        function next(): {
            const item = _source.next();

            if item is not null:
                return _predicate(item);

            return null;
        }
    }
}