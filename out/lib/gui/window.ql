native "D:/Projects/Console/Qlang/GUI/bin/Debug/net9.0/GUI"

class Window: {
    private let _name;
    private let _width;
    private let _height;

    function new(const<Number> width, const<Number> height, const<String> name): {
        _name = name;
        _width = width;
        _height = height;
    }

    function create(const<Number> width, const<Number> height, const<String> name): {
        const window = Window.new(width, height, name);
        _native("gui.window.init", width, height, name);
        return window;
    }

    function close():
        _native("gui.window.close");
    
    function shouldClose():
        return _native("gui.window.should_close");
}