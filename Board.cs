using System.Diagnostics;

class Board : Block
{
    private int[,] board = new int[20, 10];
    private object lockObj = new object();
    Random random = new Random();
    private ScoreManager score = new ScoreManager();
    public Board()
    {
        Thread fallingThread = new Thread(startFalling);
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
        }
    }
    public void spawnPiece()
    {
        int randomPiece = random.Next(0, 7);
        currentPiece = randomPiece;
        row = 0;
        col = 4;
        for (int i = 0; i < shapes[currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < shapes[currentPiece].GetLength(1); j++)
            {
                if (board[row + i, col + j] != 0 && shapes[currentPiece][i, j] == 1)
                {
                    Console.WriteLine("GAME OVERRR!!!");
                    Environment.Exit(0);
                }
            }
        }
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
            if (key.Key == ConsoleKey.A && canMoveLeft())
            {
                col--;
            }
            else if (key.Key == ConsoleKey.D && canMoveRight())
            {
                col++;
            }
            else if (key.Key == ConsoleKey.W)
            {
                rotatePiece();
            }
            else if (key.Key == ConsoleKey.S)
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
            else if (key.Key == ConsoleKey.Spacebar)
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
}