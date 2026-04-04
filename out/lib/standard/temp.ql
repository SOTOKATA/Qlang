namespace std: {
    function wait(let<Number> millisec): {
        millisec = std::parser.asInt(millisec);

        #std.DateTime.Wait(millisec);
    }
}