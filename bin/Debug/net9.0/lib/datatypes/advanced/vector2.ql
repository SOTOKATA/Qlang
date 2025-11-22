class Vector2:
    let x = 0
    let y = 0

    function new(let posX, let posY):
        x = posX
        y = posY

    function zero():
        return Vector2.new(0, 0)

    function one():
        return Vector2.new(1, 1)

    function set(let posX, let posY):
        x = posX
        y = posY

    function toString():
        return "(x:" + x + ", y:" + y + ")" 