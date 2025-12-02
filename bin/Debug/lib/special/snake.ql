include "$lib/base"

class SnakeGame: {
    let _position;

    let _tail;

    let _tailLength;

    let _lastChar;

    let _fruitPos;

    let _width;

    let _height;

    let _score;

    let _gameOver;

    let _toWait;

    function setup(): {
        Console.clear();
        Console.cursorVisible(false);

        _tail = Array.new([]);

        _tailLength = 0;

        _score = 0;

        _gameOver = false;

        _lastChar = "";

        _width = 16;

        _height = 8;

        _position = Vector2.new(_width / 2, _height / 2);

        _fruitPos = Vector2.new(Number.randInt(1, _width), Number.randInt(1, _height));

        _toWait = 100;
    }

    function draw(): {
        Console.setCursorPosition(0, 0);

        let arrow = "";

        if _lastChar == "a": { arrow = "<"; }
        else if _lastChar == "d": { arrow = ">"; }
        else if _lastChar == "s": { arrow = "V"; }
        else if _lastChar == "w": { arrow = "^"; }
        else: { arrow = "%"; }

        Console.setForeColor("white");
        Console.println("Score: " + Number.toFixed(_score, "00") + " Speed: " + _toWait);

        for let j = 0; j < (_height + 1); j = j + 1: {
            for let i = 0; i < (_width + 1); i = i + 1: {
                Console.setCursorPosition(i, (j + 1));

                let isTail = false;
                for let k = 0; k < _tail.length(); k = k + 1: {
                    if _tail.at(k).equals(Vector2.new(i, j)): {
                        isTail = true;
                    }
                }

                if (i == _position.X()) && (j == _position.Y()): {
                    Console.setForeColor("yellow");
                    Console.print(arrow);
                } else if  _fruitPos.equals(Vector2.new(i, j)): {
                    Console.setForeColor("green");
                    Console.print("F");
                } else if (i == 0) || (j == 0) || (i == _width) || (j == _height): {
                    Console.setForeColor("gray");
                    Console.print("#");
                } else if isTail == true: {
                    Console.setForeColor("green");
                    Console.print("0");
                } else: {
                    Console.setForeColor("DarkGray");
                    Console.print(".");
                }
            }
        }
    }

    function logic(): {
        if _fruitPos.equals(_position): {
            _score = _score + 1;

            _tailLength = _tailLength + 1;

            _fruitPos = Vector2.new(Number.randInt(1, _width), Number.randInt(1, _height));
        }

        for let i = 0; i < _tail.length(); i = i + 1: {
            if _tail.at(i).equals(_position): {
                _gameOver = true;
            }
        }
     
        _tail.insert(0, _position);
        if (_tail.length() > _tailLength): {
            _tail.removeAt(_tail.length() - 1);
        }

        input();

        if _position.X() < 1: {
            _position = Vector2.new(_width - 1, _position.Y());
        } else if _position.X() > (_width - 1): {
            _position = Vector2.new(1, _position.Y());
        }

        if _position.Y() < 1: {
            _position = Vector2.new(_position.X(), _height - 1);
        } else if _position.Y() > (_height - 1): {
            _position = Vector2.new(_position.X(), 1);
        }
    }

    function input(): {
        if Console.isKeyAvailable() == false: {
            if _lastChar != "": {
                control(_lastChar);
            }  
            return "";
        }

        _lastChar = Console.readkey(true);    
        control(_lastChar);
    }

    function control(let char): {
        if char == "a": {
            _position = Vector2.new(_position.X() - 1, _position.Y());
        } else if char == "d": {
            _position = Vector2.new(_position.X() + 1, _position.Y());
        } else if char == "w": {
            _position = Vector2.new(_position.X(), _position.Y() - 1);
        } else if char == "s": {
            _position = Vector2.new(_position.X(), _position.Y() + 1);
        }
    }

    function run(): {
        setup();

        while _gameOver == false: {
            logic();
            draw();

            _toWait = 100 / (1 + (_score * 0.05));
            Time.wait(_toWait);

        }

        Console.setCursorPosition(0, _height + 3);
        Console.setForeColor("red");
        Console.println("Game Over! Your score: " + Number.toFixed(_score, "00"));
        Console.resetColors();
        Console.cursorVisible(true);

    }
}


