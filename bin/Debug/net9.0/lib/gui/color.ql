class Color: {
    let h;
    let s;
    let v;

    function new(const<Number> h, const<Number> s, const<Number> v): {
        this.h = h;
        this.s = s;
        this.v = v;
    }

    function get():
        return _native("gui.window.get_hsv_color", h, s, v);
}