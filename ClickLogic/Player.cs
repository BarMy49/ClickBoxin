namespace ClickBoxin;

public class Player
{
    public int Score { get; set; }
    public int Stage { get; set; }
    public int Dmg { get; set; }
    
    public Player(int score, int stage, int dmg)
    {
        Score = score;
        Stage = stage;
        Dmg = dmg;
    }
}