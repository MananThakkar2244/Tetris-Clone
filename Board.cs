using System.Diagnostics;
using System.Runtime.CompilerServices;

class Board : Block
{
    private int[,] board = new int[20, 10];
    private int holdPiece = -1;
    private int ghostRow = 0;
    private object lockObj = new object();
    Random random = new Random();
    private ScoreManager score = new ScoreManager();
    private Keybinds playerkeybinds;
    public Board(Keybinds keybinds)
    {
        playerkeybinds = keybinds;
        Thread fallingThread = new Thread(startFalling);
        giveRandomPiece();
        spawnPiece();
        placePiece();
        drawBoard();
        fallingThread.Start();
        pieceMovement();
    }
    public void drawBoard()
    {
        lock (lockObj)
        {
            Console.Clear();
            Console.WriteLine("============");
            for (int i = 0; i < 20; i++)
            {
                Console.Write("|");
                for (int j = 0; j < 10; j++)
                {
                    if (board[i, j] == 0)
                    {
                        Console.Write("-");
                    }
                    else
                    {
                        ConsoleColor color = shapeColor[board[i, j] - 1];
                        Console.ForegroundColor = color;
                        Console.Write("█");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("============");
            Console.WriteLine("Score: " + score.score);
            Console.WriteLine("Level: " + score.level);
            Console.WriteLine("HighScore: " + score.highScore);
            showNextPiece();
            showHeldPiece();
            showGhostPiece();
        }
    }
    public void spawnPiece()
    {
        currentPiece = nextPiece;
        nextPiece = random.Next(0, 7);
        row = 0;
        col = 4;
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
            {
                if (board[row + i, col + j] != 0 && shapes[currentPiece][i, j] == 1)
                {
                    Console.WriteLine("GAME OVERRR!!!");
                    score.saveHighscore();
                    Environment.Exit(0);
                }
            }
        }
        canHold = true;
    }
    public void placePiece()
    {
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
            {
                if (shapes[currentPiece][i, j] == 1)
                {
                    board[row + i, col + j] = currentPiece + 1;
                }
            }
        }
    }
    public void clearPiece()
    {
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
            {
                if (shapes[currentPiece][i, j] == 1)
                {
                    board[row + i, col + j] = 0;
                }
            }
        }
    }
    public void pieceMovement()
    {
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            clearPiece();
            if (key.Key == playerkeybinds.MoveLeft && canMoveLeft())
            {
                col--;
            }
            else if (key.Key == playerkeybinds.MoveRight && canMoveRight())
            {
                col++;
            }
            else if (key.Key == playerkeybinds.Rotate)
            {
                rotatePiece();
            }
            else if (key.Key == playerkeybinds.SoftDrop)
            {
                if (canMoveDown())
                    row++;
                else
                {
                    placePiece();
                    int lines = clearLine();
                    score.updateLevel(lines);
                    score.updateScore(lines);
                    spawnPiece();
                    placePiece();
                }
            }
            else if (key.Key == playerkeybinds.HardDrop)
            {
                while (canMoveDown())
                {
                    row++;
                }
                placePiece();
                int lines = clearLine();
                score.updateLevel(lines);
                score.updateScore(lines);
                spawnPiece();
                placePiece();
                drawBoard();
                continue;
            }
            else if (key.Key == playerkeybinds.Hold)
            {
                hold();
                continue;
            }
            placePiece();
            drawBoard();
        }
    }
    public void startFalling()
    {
        while (true)
        {
            Thread.Sleep(score.fallspeed);
            clearPiece();
            if (canMoveDown())
            {
                row++;
                placePiece();
                drawBoard();
            }
            else
            {
                placePiece();
                int lines = clearLine();
                score.updateLevel(lines);
                score.updateScore(lines);
                spawnPiece();
                placePiece();
                drawBoard();
            }
        }
    }
    public bool canMoveDown()
    {
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            if (row + i + 1 < 20)
            {
                for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
                {
                    if (board[row + i + 1, col + j] != 0 && shapes[currentPiece][i, j] == 1)
                    {
                        return false;
                    }
                }
            }
        }
        if (row + shapes[currentPiece].GetLength(0) >= 20)
        {
            return false;
        }
        return true;
    }
    public bool canMoveLeft()
    {
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
            {
                if (shapes[currentPiece][i, j] == 1 && (col + j - 1 < 0 || board[row + i, col + j - 1] != 0))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public bool canMoveRight()
    {
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
            {
                if (shapes[currentPiece][i, j] == 1 && (col + j + 1 > 9 || board[row + i, col + j + 1] != 0))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void rotatePiece()
    {
        int maxRow = shapes[currentPiece].GetLength(0) - 1;
        int[,] newShape = new int[shapes[currentPiece].GetLength(1), shapes[currentPiece].GetLength(0)];
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
            {
                newShape[j, maxRow - i] = shapes[currentPiece][i, j];
            }
        }
        shapes[currentPiece] = newShape;
    }
    public int clearLine()
    {
        int clearedLines = 0;
        for (int i = 0; i < 20; i++)
        {
            bool full = true;
            for (int j = 0; j < 10; j++)
            {
                if (board[i, j] == 0)
                {
                    full = false;
                }
            }
            if (full)
            {
                clearedLines++;
                for (int r = i; r > 0; r--)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        board[r, j] = board[r - 1, j];
                    }
                }
                for (int j = 0; j < 10; j++)
                {
                    board[0, j] = 0;
                }
            }
        }
        return clearedLines;
    }
    public void giveRandomPiece()
    {
        currentPiece = random.Next(0, 7);
        nextPiece = random.Next(0, 7);
    }
    public void hold()
    {
        if (canHold == false)
        {
            return;
        }
        if (holdPiece == -1)
        {
            holdPiece = currentPiece;
            spawnPiece();
            placePiece();
            drawBoard();
        }
        else
        {
            int temp = holdPiece;
            holdPiece = currentPiece;
            currentPiece = temp;
            row = 0;
            col = 4;
            placePiece();
            drawBoard();
        }
        canHold = false;
    }
    public void showNextPiece()
    {
        Console.SetCursorPosition(13, 1);
        Console.Write("Next Piece");
        for (int i = 0; i < shapes[nextPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[nextPiece].GetLength(1); j++)
            {
                if (shapes[nextPiece][i, j] == 1)
                {
                    Console.SetCursorPosition(13 + j, 2 + i);
                    ConsoleColor color = shapeColor[nextPiece];
                    Console.ForegroundColor = color;
                    Console.Write("█");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(" ");
                }
            }
        }
    }
    public void showHeldPiece()
    {
        Console.SetCursorPosition(13, 6);
        Console.Write("Held Piece");
        if (holdPiece == -1)
        {
            Console.Write(" ");
        }
        else
        {
            for (int i = 0; i < shapes[holdPiece].GetLength(0); i++)
            {
                for (int j = 0; j < shapes[holdPiece].GetLength(1); j++)
                {
                    if (shapes[holdPiece][i, j] == 1)
                    {
                        Console.SetCursorPosition(13 + j, 7 + i);
                        ConsoleColor color = shapeColor[holdPiece];
                        Console.ForegroundColor = color;
                        Console.Write("█");
                        Console.ResetColor();
                    }
                }
            }
        }
    }
    private bool canMoveDownFrom(int testRow)
    {
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            if (testRow + i + 1 < 20)
            {
                for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
                {
                    if (board[testRow + i + 1, col + j] != 0 && shapes[currentPiece][i, j] == 1)
                    {
                        return false;
                    }
                }
            }
        }
        if (testRow + shapes[currentPiece].GetLength(0) >= 20)
        {
            return false;
        }
        return true;
    }
    private void showGhostPiece()
    {
        clearPiece();
        ghostRow = row;
        while (canMoveDownFrom(ghostRow))
        {
            ghostRow++;
        }
        placePiece();
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
            {
                if (shapes[currentPiece][i, j] == 1)
                {
                    Console.SetCursorPosition(col + j + 1, ghostRow + i + 1);
                    Console.Write("░");
                }
            }
        }
    }
}