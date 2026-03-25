import "$lib/standard"
import "$lib/core/linq"
using std;

namespace linq: {
    class WhereEnumerable extends linq::Enumerable: {
        function next():
            while true: {
                let item = _source.next();
                
                if typeof(item) == "null": 
                    return null;

                if _predicate(item):
                    return item;
            }
    }
}