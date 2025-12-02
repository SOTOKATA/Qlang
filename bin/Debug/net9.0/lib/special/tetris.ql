include "$lib/base"

class TetrisGame: {
    let _width;
    let _height;
    let _board;

    let _pieces;
    let _currentIndex;
    let _currentRotation;
    let _currentPos;

    let _score;
    let _gameOver;
    let _dropCounter;
    let _dropDelay;

    function setup(): {
        Console.clear();
        Console.cursorVisible(false);

        _width = 10;
        _height = 20;

        _board = Array.new([]);
        for let y = 0; y < _height; y = y + 1: {
            let row = Array.new([]);
            for let x = 0; x < _width; x = x + 1: {
                row.push(0);
            }
            _board.push(row);
        }

        _pieces = Array.new([]);
        // Define pieces as rotations of Vector2 offsets (x,y)
        // I
        _pieces.push(Array.new([ Array.new([ Vector2.new(0,1), Vector2.new(1,1), Vector2.new(2,1), Vector2.new(3,1) ]), Array.new([ Vector2.new(2,0), Vector2.new(2,1), Vector2.new(2,2), Vector2.new(2,3) ]) ]));
        // O
        _pieces.push(Array.new([ Array.new([ Vector2.new(1,0), Vector2.new(2,0), Vector2.new(1,1), Vector2.new(2,1) ]) ]));
        // T
        _pieces.push(Array.new([ Array.new([ Vector2.new(1,0), Vector2.new(0,1), Vector2.new(1,1), Vector2.new(2,1) ]), Array.new([ Vector2.new(1,0), Vector2.new(1,1), Vector2.new(2,1), Vector2.new(1,2) ]), Array.new([ Vector2.new(0,1), Vector2.new(1,1), Vector2.new(2,1), Vector2.new(1,2) ]), Array.new([ Vector2.new(1,0), Vector2.new(0,1), Vector2.new(1,1), Vector2.new(1,2) ]) ]));
        // L
        _pieces.push(Array.new([ Array.new([ Vector2.new(2,0), Vector2.new(0,1), Vector2.new(1,1), Vector2.new(2,1) ]), Array.new([ Vector2.new(1,0), Vector2.new(1,1), Vector2.new(1,2), Vector2.new(2,2) ]), Array.new([ Vector2.new(0,1), Vector2.new(1,1), Vector2.new(2,1), Vector2.new(0,2) ]), Array.new([ Vector2.new(0,0), Vector2.new(1,0), Vector2.new(1,1), Vector2.new(1,2) ]) ]));
        // J
        _pieces.push(Array.new([ Array.new([ Vector2.new(0,0), Vector2.new(0,1), Vector2.new(1,1), Vector2.new(2,1) ]), Array.new([ Vector2.new(1,0), Vector2.new(2,0), Vector2.new(1,1), Vector2.new(1,2) ]), Array.new([ Vector2.new(0,1), Vector2.new(1,1), Vector2.new(2,1), Vector2.new(2,2) ]), Array.new([ Vector2.new(1,0), Vector2.new(1,1), Vector2.new(0,2), Vector2.new(1,2) ]) ]));
        // S
        _pieces.push(Array.new([ Array.new([ Vector2.new(1,0), Vector2.new(2,0), Vector2.new(0,1), Vector2.new(1,1) ]), Array.new([ Vector2.new(1,0), Vector2.new(1,1), Vector2.new(2,1), Vector2.new(2,2) ]) ]));
        // Z
        _pieces.push(Array.new([ Array.new([ Vector2.new(0,0), Vector2.new(1,0), Vector2.new(1,1), Vector2.new(2,1) ]), Array.new([ Vector2.new(2,0), Vector2.new(1,1), Vector2.new(2,1), Vector2.new(1,2) ]) ]));

        _score = 0;
        _gameOver = false;
        _dropCounter = 0;
        _dropDelay = 500; // ms

        spawnPiece();
    }

    function spawnPiece(): {
        _currentIndex = Number.randInt(0, 7);
        _currentRotation = 0;
        _currentPos = Vector2.new(3, 0);

        if collides(_currentPos.X(), _currentPos.Y(), _currentRotation): {
            _gameOver = true;
        }
    }

    function collides(let px, let py, let rotation): {
        let rotations = _pieces.at(_currentIndex);
        rotation = Parser.asInt(rotation);
        if rotation < 0: { rotation = 0; }
        rotation = rotation % rotations.length();
        let shape = rotations.at(rotation);
        for let i = 0; i < shape.length(); i = i + 1: {
            let cell = shape.at(i);
            let x = px + cell.X();
            let y = py + cell.Y();
            if (x < 0) || (x >= _width): {
                return true;
            }
            if (y >= _height): {
                return true;
            }
            if (y >= 0): {
                if _board.at(y).at(x) != 0: {
                    return true;
                }
            }
        }
        return false;
    }

    function lockPiece(): {
        let rotations = _pieces.at(_currentIndex);
        let shape = rotations.at(_currentRotation % rotations.length());
        for let i = 0; i < shape.length(); i = i + 1: {
            let cell = shape.at(i);
            let x = _currentPos.X() + cell.X();
            let y = _currentPos.Y() + cell.Y();
            if (y >= 0): {
                _board.at(y).setAt(x, _currentIndex + 1);
            }
        }
        clearLines();
        spawnPiece();
    }

    function clearLines(): {
        let cleared = 0;
        for let y = (_height - 1); y >= 0; y = y - 1: {
            let full = true;
            for let x = 0; x < _width; x = x + 1: {
                if _board.at(y).at(x) == 0: { full = false; break; }
            }
            if full == true: {
                // remove row y
                _board.removeAt(y);
                // insert empty row at top
                let newRow = Array.new([]);
                for let x2 = 0; x2 < _width; x2 = x2 + 1: { newRow.push(0); }
                _board.insert(0, newRow);
                cleared = cleared + 1;
                y = y + 1; // re-check same y after pull-down
            }
        }
        if cleared > 0: { _score = _score + (cleared * 100); }
    }

    function draw(): {
        Console.setCursorPosition(0, 0);
        Console.setForeColor("white");
        Console.println("Score: " + Number.toFixed(_score, "0000"));

        // Draw left border
        for let y = 0; y < _height; y = y + 1: {
            Console.setCursorPosition(0, (y + 1));
            Console.setForeColor("gray");
            Console.print("#");
        }

        for let y = 0; y < _height; y = y + 1: {
            for let x = 0; x < _width; x = x + 1: {
                Console.setCursorPosition((x + 1), (y + 1));

                // check current piece overlay
                let drawCell = _board.at(y).at(x);
                if drawCell == 0: {
                    // check current falling piece
                    if isPieceAt(x, y) == true: {
                        Console.setForeColor("yellow"); Console.print("@");
                    } else: {
                        Console.setForeColor("DarkGray"); Console.print(".");
                    }
                } else: {
                    Console.setForeColor("green"); Console.print("#");
                }
            }
            // Draw right border
            Console.setCursorPosition((_width + 1), (y + 1));
            Console.setForeColor("gray");
            Console.print("#");
        }
    }

    function isPieceAt(let x, let y): {
        let rotations = _pieces.at(_currentIndex);
        let shape = rotations.at(_currentRotation % rotations.length());
        for let i = 0; i < shape.length(); i = i + 1: {
            let cell = shape.at(i);
            let cx = _currentPos.X() + cell.X();
            let cy = _currentPos.Y() + cell.Y();
            if (cx == x) && (cy == y) && (cy >= 0) && (cy < _height) && (cx >= 0) && (cx < _width): {
                return true;
            }
        }
        return false;
    }

    function input(): {
        if Console.isKeyAvailable() == false: {
            return "";
        }
        let k = Console.readkey(true);
        if k == "a": { moveLeft(); }
        else if k == "d": { moveRight(); }
        else if k == "s": { softDrop(); }
        else if k == "w": { rotate(); }
        else if k == "q": { _gameOver = true; }
    }

    function moveLeft(): {
        let np = Vector2.new(_currentPos.X() - 1, _currentPos.Y());
        if collides(np.X(), np.Y(), _currentRotation) == false: { _currentPos = np; }
    }

    function moveRight(): {
        let np = Vector2.new(_currentPos.X() + 1, _currentPos.Y());
        if collides(np.X(), np.Y(), _currentRotation) == false: { _currentPos = np; }
    }

    function softDrop(): {
        let np = Vector2.new(_currentPos.X(), _currentPos.Y() + 1);
        if collides(np.X(), np.Y(), _currentRotation) == false: { _currentPos = np; }
        else: { lockPiece(); }
    }

    function rotate(): {
        let newRot = (_currentRotation + 1);
        if collides(_currentPos.X(), _currentPos.Y(), newRot) == false: { _currentRotation = newRot; }
    }

    function run(): {
        setup();
        let last = 0;
        while _gameOver == false: {
            input();
            // gravity
            _dropCounter = _dropCounter + 50;
            if _dropCounter >= _dropDelay: {
                _dropCounter = 0;
                let np = Vector2.new(_currentPos.X(), _currentPos.Y() + 1);
                if collides(np.X(), np.Y(), _currentRotation) == false: { _currentPos = np; }
                else: { lockPiece(); }
            }

            draw();
            Time.wait(50);
        }

        Console.setCursorPosition(0, _height + 3);
        Console.setForeColor("red");
        Console.println("Game Over! Your score: " + Number.toFixed(_score, "0000"));
        Console.resetColors();
        Console.cursorVisible(true);
    }
}