include "$lib/base"
include "$lib/filesystem"

class T: {
let field;
let currentPiece;
let posX;
let posY;
let score;
let gameOver;
let pieces;

// Game constants
const FIELD_WIDTH = 10;
const FIELD_HEIGHT = 20;

function initGame(): {
    // Initialize field
    field = Array.new([]);
    for let y = 0; y < FIELD_HEIGHT; y = y + 1: {
        let row = Array.new([]);
        for let x = 0; x < FIELD_WIDTH; x = x + 1: {
            row.push(0);
        }
        field.push(row);
    }

    // Initialize pieces
    pieces = Array.new([]);

    // I-piece (line)
    let piece1 = Array.new([]);
    piece1.push(Array.new([1, 1, 1, 1]));
    pieces.push(piece1);

    // O-piece (square)
    let piece2 = Array.new([]);
    piece2.push(Array.new([1, 1]));
    piece2.push(Array.new([1, 1]));
    pieces.push(piece2);

    // T-piece
    let piece3 = Array.new([]);
    piece3.push(Array.new([0, 1, 0]));
    piece3.push(Array.new([1, 1, 1]));
    pieces.push(piece3);

    // L-piece
    let piece4 = Array.new([]);
    piece4.push(Array.new([1, 0]));
    piece4.push(Array.new([1, 0]));
    piece4.push(Array.new([1, 1]));
    pieces.push(piece4);

    score = 0;
    gameOver = false;
    spawnNewPiece();
}

function spawnNewPiece(): {
    let pieceIndex = Number.randInt(0, pieces.length());
    if pieceIndex >= pieces.length(): {
        pieceIndex = 0;
    }

    // Copy piece
    currentPiece = Array.new([]);
    let original = pieces.at(pieceIndex);
    for let i = 0; i < original.length(); i = i + 1: {
        let row = original.at(i);
        let newRow = Array.new([]);
        for let j = 0; j < row.length(); j = j + 1: {
            newRow.push(row.at(j));
        }
        currentPiece.push(newRow);
    }

    // FIXED: Use proper integer division
    posX = Number.toFixedInt(FIELD_WIDTH / 2) - 1;
    posY = 0;

    if canPlace() == false: {
        gameOver = true;
    }
}

// FIXED: Cleaned up canPlace function - return now works correctly
function canPlace(): {
    for let y = 0; y < currentPiece.length(); y = y + 1: {
        let row = currentPiece.at(y);
        for let x = 0; x < row.length(); x = x + 1: {
            if row.at(x) == 1: {
                let fieldX = posX + x;
                let fieldY = posY + y;

                // Check boundaries (including top boundary fieldY < 0)
                if (fieldX < 0) || (fieldX >= FIELD_WIDTH) || (fieldY >= FIELD_HEIGHT): {
                    return false;
                }

                // Check collision with existing pieces
                // Allow pieces to be partially above the field (fieldY < 0)
                if (fieldY >= 0) && (fieldY < FIELD_HEIGHT): {
                    let fieldRow = field.at(fieldY);
                    if fieldRow.at(fieldX) == 1: {
                        return false;
                    }
                }
            }
        }
    }

    return true;
}

function lockPiece(): {
    for let y = 0; y < currentPiece.length(); y = y + 1: {
        let row = currentPiece.at(y);
        for let x = 0; x < row.length(); x = x + 1: {
            if row.at(x) == 1: {
                let fieldX = posX + x;
                let fieldY = posY + y;

                if fieldY >= 0: {
                    let fieldRow = field.at(fieldY);
                    fieldRow.setAt(fieldX, 1);
                }
            }
        }
    }

    checkLines();
    spawnNewPiece();
}

function checkLines(): {
    for let y = FIELD_HEIGHT - 1; y >= 0; y = y - 1: {
        let row = field.at(y);
        let complete = true;

        for let x = 0; x < FIELD_WIDTH; x = x + 1: {
            if row.at(x) == 0: {
                complete = false;
            }
        }

        if complete == true: {
            field.removeAt(y);
            let newRow = Array.new([]);
            for let x = 0; x < FIELD_WIDTH; x = x + 1: {
                newRow.push(0);
            }
            field.insert(0, newRow);
            score = score + 100;
            y = y + 1;
        }
    }
}

function moveLeft(): {
    posX = posX - 1;
    if canPlace() == false: {
        posX = posX + 1;
    }
}

function moveRight(): {
    posX = posX + 1;
    if canPlace() == false: {
        posX = posX - 1;
    }
}

// FIXED: Cleaned up moveDown function - removed debug output
function moveDown(): {
    posY = posY + 1;

    if canPlace() == false: {
        posY = posY - 1;
        lockPiece();
    } else: {
        score = score + 1;
    }
}

function handleInput(): {
    if Console.isKeyAvailable() == false: {
        return;
    }

    let key = Console.readkey(true);

    if gameOver: {
        if key == "r": {
            initGame();
        }
        return;
    }

    if key == "a": {
        moveLeft();
    } else if key == "d": {
        moveRight();
    } else if key == "s": {
        moveDown();
    } else if key == "q": {
        gameOver = true;
    }
}

function draw(): {
    Console.clear();  // FIXED: Uncommented clear() for proper rendering

    // Top border
    Console.setCursorPosition(0, 0);
    Console.print("+");
    for let x = 0; x < FIELD_WIDTH; x = x + 1: {
        Console.print("-");
    }
    Console.println("+");

    // Game field
    for let y = 0; y < FIELD_HEIGHT; y = y + 1: {
        Console.print("|");
        let row = field.at(y);

        for let x = 0; x < FIELD_WIDTH; x = x + 1: {
            let hasPiece = false;

            // Check current piece
            for let py = 0; py < currentPiece.length(); py = py + 1: {
                let pieceRow = currentPiece.at(py);
                for let px = 0; px < pieceRow.length(); px = px + 1: {
                    if pieceRow.at(px) == 1: {
                        if (posX + px) == x && (posY + py) == y: {
                            hasPiece = true;
                        }
                    }
                }
            }

            if hasPiece: {
                Console.print("O");
            } else if row.at(x) == 1: {
                Console.print("#");
            } else: {
                Console.print(" ");
            }
        }
        Console.println("|");
    }

    // Bottom border
    Console.print("+");
    for let x = 0; x < FIELD_WIDTH; x = x + 1: {
        Console.print("-");
    }
    Console.println("+");

    // UI - clean interface without debug info
    Console.println("");
    Console.println("TETRIS - Score: " + score);
    Console.println("Controls: A/D=Move, S=Drop, Q=Quit");

    if gameOver: {
        Console.println("GAME OVER! Press R to restart");
    }
}

function run(): {
    Console.println("Starting Tetris Game...");
    Console.println("Loading...");
    Console.println("Starting Tetris...");
    Console.cursorVisible(false);

    initGame();

    let dropTimer = 0;
    let dropSpeed = 500;

    while true: {
        handleInput();

        if gameOver == false: {
            dropTimer = dropTimer + 50;
            if dropTimer >= dropSpeed: {
                moveDown();
                dropTimer = 0;
            }
        }
        draw();
        Time.wait(50);
    }

    Console.cursorVisible(true);
}
}

function main(): {
    Console.clear();
    T.run();
}
