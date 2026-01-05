
class Draw: {
    function begin(const<Color> color): {
        _native("gui.window.begin_drawing");
        _native("gui.window.clear_background", color.get());
    }

    function text(const<String> text, const<Number> x, const<Number> y, const<Number> fontSize, const<Color> color): {
        _native("gui.window.draw_text", text, x, y, fontSize, color.get());
    }

    function end():
        _native("gui.window.end_drawing");
}