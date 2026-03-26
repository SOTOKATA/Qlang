namespace std: {
    function wait(let<Number> millisec): {
        millisec = std::parser.asInt(millisec);

        _native("std", "dateTime", "wait", millisec);
    }
}