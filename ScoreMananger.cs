using System.IO;
class ScoreManager
{
    public int score = 0;
    public int highScore;
    public int level = 1;
    private int targetNextLevel = 5;
    private int totallines = 0;
    public int fallspeed = 500;
    public ScoreManager()
    {
        if (File.Exists("HighScore.txt"))
        {
            highScore = int.Parse(File.ReadAllText("HighScore.txt"));
        }
        else
        {
            highScore = 0;
        }
    }
    public void updateLevel(int linescleared)
    {
        totallines += linescleared;
        if (totallines >= targetNextLevel && level < 10)
        {
            level++;
            totallines = 0;
            targetNextLevel = level * 5;
            fallspeed = (int)(fallspeed * 0.97);
        }
    }
    public void updateScore(int linescleared)
    {
        if (linescleared == 0)
        {
            return;
        }
        else if (linescleared == 1)
        {
            score += 100;
        }
        else if (linescleared == 2)
        {
            score += 300;
        }
        else if (linescleared == 3)
        {
            score += 500;
        }
        else
        {
            score += 800;
        }
        if (score > highScore)
        {
            highScore = score;
        }
    }
    public void saveHighscore()
    {
        if (File.Exists("HighScore.txt"))
        {
            if (score > int.Parse(File.ReadAllText("HighScore.txt")))
            {
                File.WriteAllText("HighScore.txt", score.ToString());
            }
            else
            {
                return;
            }
        }
        else
        {
            File.WriteAllText("HighScore.txt", score.ToString());
        }
    }
}