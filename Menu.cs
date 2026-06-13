class Menu
{
    private string tetris = "Tetris";
    private string line = "========================";
    private int selectedIndex = 0;
    private string[] options = { "New Game", "Settings", "Exit" };
    Keybinds k = new Keybinds();

    public void drawMenu()
    {
        while (true)
        {
            Console.CursorVisible = false;
            Console.Clear();
            int totalWidth = 24;
            Console.WriteLine(line);
            Console.WriteLine(tetris.PadLeft(((totalWidth - tetris.Length) / 2) + tetris.Length));
            Console.WriteLine(line);
            for (int i = 0; i < options.Length; i++)
            {
                string prefix = (selectedIndex == i) ? "> " : " ";
                Console.WriteLine((prefix + options[i]).PadLeft(((totalWidth - options[i].Length) / 2) + options[i].Length));
            }
            Console.WriteLine(line);
            if (!inputHandler())
            {
                break;
            }
        }
        Console.WriteLine("You selected: " + options[selectedIndex]);
    }
    private bool inputHandler()
    {
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.UpArrow)
        {
            selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
        }
        else if (key.Key == ConsoleKey.DownArrow)
        {
            selectedIndex = (selectedIndex + 1) % options.Length;
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            if (selectedIndex == 0)
            {
                Board b = new Board(k);
                b.drawBoard();
                return false;
            }
            else if (selectedIndex == 1)
            {
                Settings s = new Settings(k);
                s.openSettings();
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}