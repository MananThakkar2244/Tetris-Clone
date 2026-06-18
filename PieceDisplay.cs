class PieceDisplay
{
    private Board b;
    private PieceLogic piecelogic;
    public PieceDisplay(Board board)
    {
        b = board;
        piecelogic = new PieceLogic(b);
    }
    public void showNextPiece()
    {
        Console.SetCursorPosition(13, 1);
        Console.Write("Next Piece");
        for (int i = 0; i < b.shapes[b.nextPiece].GetLength(0); i++)
        {
            for (int j = 0; j < b.shapes[b.nextPiece].GetLength(1); j++)
            {
                if (b.shapes[b.nextPiece][i, j] == 1)
                {
                    Console.SetCursorPosition(13 + j, 2 + i);
                    ConsoleColor color = b.shapeColor[b.nextPiece];
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
        if (b.holdPiece == -1)
        {
            Console.Write(" ");
        }
        else
        {
            for (int i = 0; i < b.shapes[b.holdPiece].GetLength(0); i++)
            {
                for (int j = 0; j < b.shapes[b.holdPiece].GetLength(1); j++)
                {
                    if (b.shapes[b.holdPiece][i, j] == 1)
                    {
                        Console.SetCursorPosition(13 + j, 7 + i);
                        ConsoleColor color = b.shapeColor[b.holdPiece];
                        Console.ForegroundColor = color;
                        Console.Write("█");
                        Console.ResetColor();
                    }
                }
            }
        }
    }
    public void showGhostPiece()
    {
        piecelogic.clearPiece();
        b.ghostRow = b.row;
        while (b.canMoveDownFrom(b.ghostRow))
        {
            b.ghostRow++;
        }
        piecelogic.placePiece();
        for (int i = 0; i < b.shapes[b.currentPiece].GetLength(0); i++)
        {
            for (int j = 0; j < b.shapes[b.currentPiece].GetLength(1); j++)
            {
                if (b.shapes[b.currentPiece][i, j] == 1)
                {
                    Console.SetCursorPosition(b.col + j + 1, b.ghostRow + i + 1);
                    Console.Write("░");
                }
            }
        }
    }
}