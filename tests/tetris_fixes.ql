include "$lib/base"
include "$lib/filesystem"

class TetrisFixed: {
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

    // ИСПРАВЛЕНО: Используем целочисленное деление
    posX = Number.toFixedInt(FIELD_WIDTH / 2) - 1;
    posY = 0;

    if canPlace() == false: {
        gameOver = true;
    }
}

// ИСПРАВЛЕННАЯ функция canPlace()
function canPlace(): {
    // Проходим по всем блокам текущей фигуры
    for let y = 0; y < currentPiece.length(); y = y + 1: {
        let row = currentPiece.at(y);
        for let x = 0; x < row.length(); x = x + 1: {
            // Проверяем только заполненные блоки фигуры
            if row.at(x) == 1: {
                let fieldX = posX + x;
                let fieldY = posY + y;

                // ИСПРАВЛЕНО: Добавлена проверка верхней границы fieldY < 0
                // Проверяем выход за границы поля
                if (fieldX < 0) || (fieldX >= FIELD_WIDTH) || (fieldY >= FIELD_HEIGHT): {
                    return false;
                }

                // Проверяем коллизию с уже размещенными блоками
                // Учитываем, что фигура может частично находиться выше поля (fieldY < 0)
                if (fieldY >= 0): {
                    let fieldRow = field.at(fieldY);
                    if fieldRow.at(fieldX) == 1: {
                        return false;
                    }
                }
            }
        }
    }

    // Если все проверки пройдены - фигуру можно разместить
    return true;
}

function lockPiece(): {
    for let y = 0; y < currentPiece.length(); y = y + 1: {
        let row = currentPiece.at(y);
        for let x = 0; x < row.length(); x = x + 1: {
            if row.at(x) == 1: {
                let fieldX = posX + x;
                let fieldY = posY + y;

                // Размещаем только блоки, которые находятся в пределах поля
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

// ИСПРАВЛЕННАЯ функция moveDown()
function moveDown(): {
    // Пытаемся сдвинуть фигуру вниз
    posY = posY + 1;

    // Проверяем, можно ли разместить фигуру в новой позиции
    if canPlace() == false: {
        // Если нельзя - откатываем позицию и фиксируем фигуру
        posY = posY - 1;
        lockPiece();
    } else: {
        // Если можно - увеличиваем счет за мягкое падение
        score = score + 1;
    }
}

// Функция для тестирования без ввода с клавиатуры
function testMoveDown(): {
    Console.println("=== Тестирование moveDown() ===");

    // Создаем тестовое поле с препятствием внизу
    field = Array.new([]);
    for let y = 0; y < FIELD_HEIGHT; y = y + 1: {
        let row = Array.new([]);
        for let x = 0; x < FIELD_WIDTH; x = x + 1: {
            // Добавляем препятствие в нижней части
            if y == FIELD_HEIGHT - 1: {
                row.push(1);
            } else: {
                row.push(0);
            }
        }
        field.push(row);
    }

    // Создаем простую тестовую фигуру (квадрат)
    currentPiece = Array.new([]);
    currentPiece.push(Array.new([1, 1]));
    currentPiece.push(Array.new([1, 1]));

    // Устанавливаем начальную позицию
    posX = 4;
    posY = FIELD_HEIGHT - 4; // Недалеко от дна

    Console.println("Начальная позиция: (" + posX + ", " + posY + ")");
    Console.println("canPlace() возвращает: " + canPlace());

    // Тестируем несколько вызовов moveDown()
    for let i = 0; i < 5; i = i + 1: {
        Console.println("\n--- Вызов moveDown() #" + (i + 1) + " ---");
        Console.println("Позиция до: (" + posX + ", " + posY + ")");

        moveDown();

        Console.println("Позиция после: (" + posX + ", " + posY + ")");
        Console.println("canPlace() теперь: " + canPlace());

        if gameOver: {
            Console.println("Игра окончена!");
            break;
        }
    }
}

// Функция для тестирования canPlace()
function testCanPlace(): {
    Console.println("=== Тестирование canPlace() ===");

    // Инициализируем пустое поле
    field = Array.new([]);
    for let y = 0; y < FIELD_HEIGHT; y = y + 1: {
        let row = Array.new([]);
        for let x = 0; x < FIELD_WIDTH; x = x + 1: {
            row.push(0);
        }
        field.push(row);
    }

    // Создаем тестовую фигуру
    currentPiece = Array.new([]);
    currentPiece.push(Array.new([1, 1]));

    // Тест 1: Нормальная позиция
    posX = 4;
    posY = 10;
    Console.println("Тест 1 - позиция (4, 10): " + canPlace());

    // Тест 2: Выход за левую границу
    posX = -1;
    posY = 10;
    Console.println("Тест 2 - позиция (-1, 10): " + canPlace());

    // Тест 3: Выход за правую границу
    posX = FIELD_WIDTH;
    posY = 10;
    Console.println("Тест 3 - позиция (" + FIELD_WIDTH + ", 10): " + canPlace());

    // Тест 4: Выход за нижнюю границу
    posX = 4;
    posY = FIELD_HEIGHT;
    Console.println("Тест 4 - позиция (4, " + FIELD_HEIGHT + "): " + canPlace());

    // Тест 5: Выше поля (должно быть OK)
    posX = 4;
    posY = -1;
    Console.println("Тест 5 - позиция (4, -1): " + canPlace());

    // Тест 6: Коллизия с заполненным блоком
    posX = 4;
    posY = 10;
    let testRow = field.at(10);
    testRow.setAt(4, 1); // Помещаем блок в позицию коллизии
    Console.println("Тест 6 - коллизия в (4, 10): " + canPlace());
}

function draw(): {
    Console.clear(); // ИСПРАВЛЕНО: Раскомментирован clear()

    // Top border
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

    // UI (без отладочной информации)
    Console.println("");
    Console.println("TETRIS - Score: " + score);
    Console.println("Position: (" + posX + ", " + posY + ")");

    if gameOver: {
        Console.println("GAME OVER!");
    }
}

// Основная функция тестирования
function runTests(): {
    Console.println("========================================");
    Console.println("    ТЕСТИРОВАНИЕ ИСПРАВЛЕННЫХ ФУНКЦИЙ   ");
    Console.println("========================================");

    testCanPlace();
    Console.println("");
    testMoveDown();

    Console.println("\n========================================");
    Console.println("         ТЕСТИРОВАНИЕ ЗАВЕРШЕНО         ");
    Console.println("========================================");
}
}

function main(): {
    Console.clear();
    TetrisFixed.runTests();
}
