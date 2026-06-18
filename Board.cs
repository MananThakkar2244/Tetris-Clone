class Board : Block
{
    public int[,] board = new int[20, 10];
    public int holdPiece = -1;
    public int ghostRow = 0;
    public object lockObj = new object();
    public Random random = new Random();
    public ScoreManager score = new ScoreManager();
    private Keybinds playerkeybinds;
    private PieceLogic piecelogic;
    private PieceDisplay piecedisplay;
    public Board(Keybinds keybinds)
    {
        playerkeybinds = keybinds;
        piecelogic = new PieceLogic(this);
        piecedisplay = new PieceDisplay(this);
    }
    public void startGame()
    {
        Thread fallingThread = new Thread(startFalling);
        piecelogic.giveRandomPiece();
        piecelogic.spawnPiece();
        piecelogic.placePiece();
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
            piecedisplay.showNextPiece();
            piecedisplay.showHeldPiece();
            piecedisplay.showGhostPiece();
        }
    }
    public void pieceMovement()
    {
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            piecelogic.clearPiece();
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
                piecelogic.rotatePiece();
            }
            else if (key.Key == playerkeybinds.SoftDrop)
            {
                if (canMoveDown())
                    row++;
                else
                {
                    piecelogic.placePiece();
                    int lines = clearLine();
                    score.updateLevel(lines);
                    score.updateScore(lines);
                    piecelogic.spawnPiece();
                    piecelogic.placePiece();
                }
            }
            else if (key.Key == playerkeybinds.HardDrop)
            {
                while (canMoveDown())
                {
                    row++;
                }
                piecelogic.placePiece();
                int lines = clearLine();
                score.updateLevel(lines);
                score.updateScore(lines);
                piecelogic.spawnPiece();
                piecelogic.placePiece();
                drawBoard();
                continue;
            }
            else if (key.Key == playerkeybinds.Hold)
            {
                piecelogic.hold();
                continue;
            }
            piecelogic.placePiece();
            drawBoard();
        }
    }
    public void startFalling()
    {
        while (true)
        {
            Thread.Sleep(score.fallspeed);
            piecelogic.clearPiece();
            if (canMoveDown())
            {
                row++;
                piecelogic.placePiece();
                drawBoard();
            }
            else
            {
                piecelogic.placePiece();
                int lines = clearLine();
                score.updateLevel(lines);
                score.updateScore(lines);
                piecelogic.spawnPiece();
                piecelogic.placePiece();
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
    public bool canMoveDownFrom(int testRow)
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
}