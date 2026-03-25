// Base exception
class Exception extends Object: {
    let message = "";
    let printStackTrace = false;

    function new(<String> message, <Boolean> printStackTrace): {
        this.message = message;
        this.printStackTrace = printStackTrace;
    }

    function<String> toString() => `Exception: {_str(message)}`;
}