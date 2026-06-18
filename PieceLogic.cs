class PieceLogic
{
    Board b;
    public PieceLogic(Board board)
    {
        b = board;
    }
    public void spawnPiece()
    {
        b.currentPiece = b.nextPiece;
        b.nextPiece = b.random.Next(0, 7);
        b.row = 0;
        b.col = 4;
        for (int i = 0; i < b.shapes[b.currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < b.shapes[b.currentPiece].GetLength(1); j++)
            {
                if (b.board[b.row + i, b.col + j] != 0 && b.shapes[b.currentPiece][i, j] == 1)
                {
                    Console.WriteLine("GAME OVERRR!!!");
                    b.score.saveHighscore();
                    Environment.Exit(0);
                }
            }
        }
        b.canHold = true;
    }
    public void placePiece()
    {
        for (int i = 0; i < b.shapes[b.currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < b.shapes[b.currentPiece].GetLength(1); j++)
            {
                if (b.shapes[b.currentPiece][i, j] == 1)
                {
                    b.board[b.row + i, b.col + j] = b.currentPiece + 1;
                }
            }
        }
    }
    public void clearPiece()
    {
        for (int i = 0; i < b.shapes[b.currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < b.shapes[b.currentPiece].GetLength(1); j++)
            {
                if (b.shapes[b.currentPiece][i, j] == 1)
                {
                    b.board[b.row + i, b.col + j] = 0;
                }
            }
        }
    }
    public void rotatePiece()
    {
        int maxRow = b.shapes[b.currentPiece].GetLength(0) - 1;
        int[,] newShape = new int[b.shapes[b.currentPiece].GetLength(1), b.shapes[b.currentPiece].GetLength(0)];
        for (int i = 0; i < b.shapes[b.currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < b.shapes[b.currentPiece].GetLength(1); j++)
            {
                newShape[j, maxRow - i] = b.shapes[b.currentPiece][i, j];
            }
        }
        b.shapes[b.currentPiece] = newShape;
    }
    public void hold()
    {
        if (b.canHold == false)
        {
            return;
        }
        if (b.holdPiece == -1)
        {
            b.holdPiece = b.currentPiece;
            spawnPiece();
            placePiece();
            b.drawBoard();
        }
        else
        {
            int temp = b.holdPiece;
            b.holdPiece = b.currentPiece;
            b.currentPiece = temp;
            b.row = 0;
            b.col = 4;
            placePiece();
            b.drawBoard();
        }
        b.canHold = false;
    }
    public void giveRandomPiece()
    {
        b.currentPiece = b.random.Next(0, 7);
        b.nextPiece = b.random.Next(0, 7);
    }
}