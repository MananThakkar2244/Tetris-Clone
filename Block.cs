class Block
{
    private int[,] I_shape =
    {
        {1,1,1,1},
    };
    private int[,] O_shape =
    {
        {1,1},
        {1,1}
    };
    private int[,] T_shape =
    {
        {1,1,1},
        {0,1,0},
    };
    private int[,] L_shape =
    {
        {1,0},
        {1,0},
        {1,1}
    };
    private int[,] J_shape =
    {
        {0,1},
        {0,1},
        {1,1}
    };
    private int[,] S_shape =
    {
        {0,1,1},
        {1,1,0}
    };
    private int[,] Z_shape =
    {
        {1,1,0},
        {0,1,1}
    };
    public ConsoleColor[] shapeColor = { ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta,
                                            ConsoleColor.Green, ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.DarkYellow};
    public int row = 0;
    public int col = 0;
    public int currentPiece = 0;
    public int nextPiece;
    public bool canHold = true;
    public List<int[,]> shapes;

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