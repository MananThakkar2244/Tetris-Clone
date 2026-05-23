class Block
{
    protected int[,] I_shape =
    {
        {1},
        {1},
        {1},
        {1}
    };
    protected int[,] O_shape =
    {
        {1,1},
        {1,1}
    };
    protected int[,] T_shape =
    {
        {1,1,1},
        {0,1,0},
    };
    protected int[,] L_shape =
    {
        {1,0},
        {1,0},
        {1,1}
    };
    protected int[,] J_shape =
    {
        {0,1},
        {0,1},
        {1,1}
    };
    protected int[,] S_shape =
    {
        {0,1,1},
        {1,1,0}
    };
    protected int[,] Z_shape =
    {
        {1,1,0},
        {0,1,1}
    };
    protected ConsoleColor[] shapeColor = { ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta,
                                            ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.DarkYellow};
    protected int row = 0;
    protected int col = 0;
    protected int currentPiece = 0;
    protected List<int[,]> shapes;

    public Block()
    {
        shapes = new List<int[,]>
        {
            I_shape,
            O_shape,
            T_shape,
            L_shape,
            J_shape,
            S_shape,
            Z_shape
        };
    }
}