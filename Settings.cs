class Settings
{
    private Keybinds keybinds;
    private string[] options = { "Move Left", "Move Right", "Rotate", "Soft Drop", "Hard Drop", "Hold", "Back" };
    private int selectedIndex = 0;
    private string line = "========================";

    public Settings(Keybinds k)
    {
        keybinds = k;
    }
    public void openSettings()
    {
        while (true)
        {
            ConsoleKey[] keys = { keybinds.MoveLeft, keybinds.MoveRight, keybinds.Rotate,
                      keybinds.SoftDrop, keybinds.HardDrop, keybinds.Hold };
            int totalWidth = 24;
            Console.CursorVisible = false;
            Console.Clear();
            Console.WriteLine(line);
            Console.WriteLine("Settings".PadLeft(((totalWidth - "Settings".Length) / 2) + "Settings".Length));
            Console.WriteLine(line);
            for (int i = 0; i < options.Length - 1; i++)
            {
                string prefix = (selectedIndex == i) ? "> " : " ";
                Console.Write((prefix + options[i]).PadLeft(((totalWidth - options[i].Length) / 12) + options[i].Length));
                Console.WriteLine(" [" + keys[i] + "]");
            }
            string backPrefix = (selectedIndex == options.Length - 1) ? "> " : "  ";
            Console.WriteLine(backPrefix + "Back");
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
                Console.WriteLine("Press the key you want to bind...");
                ConsoleKeyInfo newKey = Console.ReadKey(true);
                if (isDuplicate(newKey.Key))
                {
                    Console.WriteLine("Key already in use!");
                    Thread.Sleep(1500);
                }
                else
                {
                    keybinds.MoveLeft = newKey.Key;
                }
            }
            else if (selectedIndex == 1)
            {
                Console.WriteLine("Press the key you want to bind...");
                ConsoleKeyInfo newKey = Console.ReadKey(true);
                if (isDuplicate(newKey.Key))
                {
                    Console.WriteLine("Key already in use!");
                    Thread.Sleep(1500);
                }
                else
                {
                    keybinds.MoveRight = newKey.Key;
                }
            }
            else if (selectedIndex == 2)
            {
                Console.WriteLine("Press the key you want to bind...");
                ConsoleKeyInfo newKey = Console.ReadKey(true);
                if (isDuplicate(newKey.Key))
                {
                    Console.WriteLine("Key already in use!");
                    Thread.Sleep(1500);
                }
                else
                {
                    keybinds.Rotate = newKey.Key;
                }
            }
            else if (selectedIndex == 3)
            {
                Console.WriteLine("Press the key you want to bind...");
                ConsoleKeyInfo newKey = Console.ReadKey(true);
                if (isDuplicate(newKey.Key))
                {
                    Console.WriteLine("Key already in use!");
                    Thread.Sleep(1500);
                }
                else
                {
                    keybinds.SoftDrop = newKey.Key;
                }
            }
            else if (selectedIndex == 4)
            {
                Console.WriteLine("Press the key you want to bind...");
                ConsoleKeyInfo newKey = Console.ReadKey(true);
                if (isDuplicate(newKey.Key))
                {
                    Console.WriteLine("Key already in use!");
                    Thread.Sleep(1500);
                }
                else
                {
                    keybinds.HardDrop = newKey.Key;
                }
            }
            else if (selectedIndex == 5)
            {
                Console.WriteLine("Press the key you want to bind...");
                ConsoleKeyInfo newKey = Console.ReadKey(true);
                if (isDuplicate(newKey.Key))
                {
                    Console.WriteLine("Key already in use!");
                    Thread.Sleep(1500);
                }
                else
                {
                    keybinds.Hold = newKey.Key;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private bool isDuplicate(ConsoleKey newKey)
    {
        ConsoleKey[] allKeys = { keybinds.MoveLeft, keybinds.MoveRight, keybinds.Rotate,
                         keybinds.SoftDrop, keybinds.HardDrop, keybinds.Hold };
        for (int i = 0; i < options.Length - 1; i++)
        {
            if (newKey == allKeys[i])
            {
                return true;
            }
        }
        return false;
    }
}