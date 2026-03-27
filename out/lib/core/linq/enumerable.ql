import "$lib/standard"
import "$lib/core/linq"
using std;

namespace linq: {
    class Enumerable: {
        private let _source;
        private let _predicate;

        function new(<Enumerable|null> source, <Func|null> predicate): {
            _source = source;
            _predicate = predicate;
        }

        function next(): throw.notImplementedException();

        function<Array> toArray(): {
            const result = new Array();
            let item = this.next();

            while typeof(item) != "null": {
                result.push(item);
                item = this.next();
            }

            return result;
        }

        function<Number> index(const item): {
            let currentIndex = 0; 
            let currentElement = next(); 

            while typeof(currentElement) != "null": {
                if currentElement == item:
                    return currentIndex;

                currentElement = next();
                currentIndex++;
            };

            return -1;
        }

        function<Dictionary> toDictionary(): {
            const result = new Dictionary();

            let item = this.next();

            while typeof(item) != "null": {
                result.set(item.key, item.value);
                item = this.next();
            }

            return result;
        }

        function first(): {
            let item = this.next();

            return if typeof(item) != "null" ? item : null;
        }

        function<Boolean> any(<Func|null> predicate): {
            let item = this.next();

            while typeof(item) != "null": {
                if typeof(predicate) == "null":
                    return true;
                
                if predicate(item):
                    return true;

                item = this.next();
            }

            return false;
        }

        function<Boolean> all(<Func> predicate): {
            let item = this.next();

            while typeof(item) != "null": {
                if !predicate(item):
                    return false;

                item = this.next();
            }

            return true; 
        }

        function<Number> sum(): {
            let total = 0;
            let item = this.next();

            while typeof(item) != "null": {
                total = total + item;
                item = this.next();
            }

            return total;
        }

        function<Number|null> max(): {
            let item = this.next();
            
            if typeof(item) == "null":
                return null;

            let maxVal = item; 

            while true: {
                item = this.next();
                
                if typeof(item) == "null":
                    break;

                if item > maxVal:
                    maxVal = item;
            }

            return maxVal; 
        }

        function<Number|null> min(): {
            let item = this.next();
            
            if typeof(item) == "null":
                return null;

            let minVal = item;

            while true: {
                item = this.next();
                
                if typeof(item) == "null":
                    break;

                if item < minVal:
                    minVal = item;
            }

            return minVal; 
        }

        function where(<Func> func) => new WhereEnumerable(this, func); 
        function select(<Func> func) => new SelectEnumerable(this, func); 
    }
}