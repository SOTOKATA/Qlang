namespace terminal: {
    private function _input(const message, const errorMessage = "", const funcValidator): {
        let input;
        let isNot = true;

        while isNot: {
            std::console::richPrint(message);
            input = std::console::readln();
            
            isNot = funcValidator(input);

            if isNot: 
                std::console::richPrint(errorMessage);
        }

        return input;
    }

    function<Number> inputNumber(const message, const errorMessage = ""): {
        return _input(message, errorMessage, function(const i): Object.isNull(<Number>i););
    } 

    function<String> inputFromRange(const<String> message, const<Collection> range, const<String> errorMessage = ""): {
        const func = function(const i): {
            return range.any(function(const item): return item == i;) == false;
        };
        return _input(message, errorMessage, func);
    }
}