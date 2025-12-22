include "$lib/base"
include "$lib/gui"

function main(): {
    Window.create(800, 450, "Window");

    while (Window.shouldClose() == false): {
        Draw.begin(Color.new(200, 0, 100));

        Draw.text("Hello World!", 0, 0, 20, Color.new(0, 0, 0));

        Draw.end();
    }

    Window.close();
}