namespace ClickBoxin;

public class Player
{
    public string Name { get; set; }
    public int Score { get; set; }
    public int Dmg { get; set; }
    public int Stage { get; set; }
    public int Ultra { get; set; }
    public int DmgMulti { get; set; }
    public int Tickets { get; set; }
    public DateTime LastRewardClaimDate { get; set; }
    public int DailyLoginStreak { get; set; }
    public int Restarts { get; set; }

    public Player()
    {
        Name = "";
        Score = 0;
        Dmg = 1;
        Stage = 1;
        Ultra = 0;
        DmgMulti = 1;
        Tickets = 0;
        LastRewardClaimDate = DateTime.MinValue;
        DailyLoginStreak = 0;
        Restarts = 0;
    }

    public bool HasClaimedRewardToday()
    {
        return LastRewardClaimDate.Date == DateTime.Today;
    }

    public void Restart()
    {
        Stage = 1;
        Score = 0;
        Dmg = 1;
        Restarts++;
        Ultra += (Score/10000) * Stage * Dmg;
    }
    
}
