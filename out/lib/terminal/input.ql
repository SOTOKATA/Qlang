import "import"

namespace terminal: {
    private function<String> _input(const message, const errorMessage = "", const funcValidator): {
        let input;
        let isNot = true;

        while isNot: {
            std::console.richPrint(message);
            input = std::console.readln();
            
            isNot = funcValidator(input);

            if isNot: 
                std::console.richPrint(errorMessage);
        }

        return input;
    }

    function<Number> inputNumber(const message, const errorMessage = ""): {
        return <Number>_input(message, errorMessage, function(const i): object.isNull(<Number>i););
    } 

    function<String> inputFromRange(const<String> message, let range, const<String> errorMessage = ""): {
        const rangeType = typeof(range);

        if rangeType == "Array":
            range = range.getCollection();
        else if rangeType != "Collection":
            throw.message("Undefined array type");

        const func = function(const i): {
            return range.any(function(const item): return item == i;) == false;
        };
        
        return _input(message, errorMessage, func);
    }
}