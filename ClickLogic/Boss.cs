namespace ClickBoxin;

public class Boss
{
    public int Stage { get; set; }
    public int Cost { get; set; }
    public int Health { get; set; }
    public int Time { get; set; }
    public int Score { get; set; }
    
    public Boss(int stage, int cost,  int health, int time, int score)
    {
        Stage = stage;
        Cost = cost;
        Health = health;
        Time = time;
        Score = score;
    }
}