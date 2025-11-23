include "@lib/base"

class SnakeGame: {
    let X = 0;
    let Y = 0;

    let tailX = Array.new([]);
    let tailY = Array.new([]);
    let tailLength = 0;

    let lastChar = "";

    let fruitX = 1;
    let fruitY = 1;

    let width = 16;
    let height = 8;

    let score = 0;

    let gameOver = false;

    function setup(): {
        Console.clear();
        Console.cursorVisible(false);

        fruitX = Number.randInt(1, width);
        fruitY = Number.randInt(1, height);
    }

    function draw(): {
        Console.setCursorPosition(0, 0);
        Console.println("Score: " + score);

        for let j = 0; j < (height + 1); j = j + 1: {
            for let i = 0; i < (width + 1); i = i + 1: {
                
                let isTailPosition = false;

                for let tX = 0; tX < tailX.length(); tX = tX + 1: {
                    let x = tailX.at(tX);

                    for let tY = 0; tY < tailY.length(); tY = tY + 1: {
                        let y = tailY.at(tY);

                        if (x == i) && (y == j): {
                            isTailPosition = true;
                        }
                    }
                }

                Console.setCursorPosition(i, (j + 1));

                if (i == X) && (j == Y): {
                    Console.print("%");
                } else if (i == fruitX) && (j == fruitY): {
                    Console.print("F");
                } else if (i == 0) || (j == 0) || (i == width) || (j == height): {
                    Console.print("#");
                } else if isTailPosition == true: {
                    Console.print("0");
                } else: {
                    Console.print(".");
                }
            }
        }
    }

    function logic(): {
        if (X == fruitX) && (Y == fruitY): {
            score = score + 1;

            tailLength = tailLength + 1;

            fruitX = Number.randInt(1, width);
            fruitY = Number.randInt(1, height);
        }
     
        tailX.insert(0, X);
        tailY.insert(0, Y);
        if tailX.length() > tailLength: {
            tailX.removeAt(tailX.length() - 1);
            tailY.removeAt(tailX.length() - 1);
        }

        input();

        if X < 1: {
            X = width - 1;
        } else if X > (width - 1): {
            X = 1;
        }

        if Y < 1: {
            Y = height - 1;
        } else if Y > (height - 1): {
            Y = 1;
        }
    }

    function input(): {
        if Console.isKeyAvailable() == false: {
            if lastChar != "": {
                control(lastChar);
            }  
            return "";
        }

        lastChar = Console.readkey(true);    
    }

    function control(let char): {
        if char == "a": {
            X = X - 1;
        } else if char == "d": {
            X = X + 1;
        } else if char == "w": {
            Y = Y - 1;
        } else if char == "s": {
            Y = Y + 1;
        }
    }

    function run(): {
        setup();

        while gameOver == false: {
            logic();
            draw();

            Time.wait(200);
        }
    }
}