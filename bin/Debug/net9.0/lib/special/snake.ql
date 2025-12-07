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
                if (i == 0) || (j == 0) || (i == _width) || (j == _height): {
                    Console.setCursorPosition(i, (j + 1));
                    Console.setForeColor("gray");
                    Console.print("#"); 
                }
            }
        }

        Console.setCursorPosition(_position.X(), _position.Y() + 1);
        Console.setForeColor("yellow");
        Console.print(arrow);

        for let k = 0; k < _tail.length(); k = k + 1: {
            let t = _tail.at(k);
            Console.setCursorPosition(t.X(), t.Y() + 1);
            Console.setForeColor("green");
            Console.print("0");
        }

        Console.setCursorPosition(_fruitPos.X(), _fruitPos.Y() + 1);
        Console.setForeColor("red");
        Console.print("F");
    }

    function logic(): {
        if (_fruitPos.X() == _position.X()) && (_fruitPos.Y() == _position.Y()): {
            _score = _score + 1;

            _tailLength = _tailLength + 1;

            _fruitPos = Vector2.new(Number.randInt(1, _width), Number.randInt(1, _height));
        }

        for let i = 0; i < _tail.length(); i = i + 1: {
            let t = _tail.at(i);
            if (t.X() == _position.X()) && (t.Y() == _position.Y()): {
                _gameOver = true;
            }
        }
     
        _tail.insert(0, Vector2.new(_position.X(), _position.Y()));
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
            draw();
            logic();
            Console.clear();
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


