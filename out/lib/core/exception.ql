// Base exception
class Exception extends Object: {
    let message = "";
    let printStackTrace = false;

    function new(const<String> message, const<Boolean> printStackTrace): {
        this.message = message;
        this.printStackTrace = printStackTrace;
    }

    function<String> toString():
        return $"Exception: {_str(message)}";
}