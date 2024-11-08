namespace ClickBoxin;

public class Boss
{
    public int Stage { get; set; }
    public int Cost { get; set; }
    public int Health { get; set; }
    public int Time { get; set; }
    public int Score { get; set; }
    
    public Boss(int stage, int time)
    {
        Stage = stage;
        Cost = (Stage * 1000)*Stage;
        Health = (Stage * 100)*Stage;
        Time = time;
        Score = (Stage * 1000)*Stage;
    }
}