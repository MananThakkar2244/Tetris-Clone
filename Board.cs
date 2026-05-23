class Board : Block
{
    private int[,] board = new int[20, 10];
    Random random = new Random();
    public Board()
    {
        spawnPiece();
        placePiece();
        drawBoard();
        pieceMovement();
    }
    public void drawBoard()
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
    }
    public void spawnPiece()
    {
        int randomPiece = random.Next(0, 7);
        currentPiece = randomPiece;
        row = 0;
        col = 4;
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
            if (key.Key == ConsoleKey.A)
            {
                clearPiece();
                col--;
                placePiece();
                drawBoard();
            }
            else if (key.Key == ConsoleKey.D)
            {
                clearPiece();
                col++;
                placePiece();
                drawBoard();
            }
        }
    }
}