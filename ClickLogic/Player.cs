namespace ClickBoxin;

public class Player
{
    public int Score { get; set; }
    public int Stage { get; set; }
    public int Dmg { get; set; }
    public int DmgMulti { get; set; }
    public int Ultra { get; set; }
    
    public Player()
    {
        Score = 1;
        Stage = 1;
        Dmg = 1;
        DmgMulti = 1;
        Ultra = 0;
    }
}