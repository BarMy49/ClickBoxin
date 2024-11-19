namespace ClickBoxin;

public class Farm
{
    public int Lvl { get; set; }
    public int ScorePP { get; set; }
    public int TimeInterval { get; set; }
    public int InitialCost { get; set; }
    public int Cost { get; set; }
    public int TimeCost { get; set; }

    public Farm(int timeInterval, int inicost, int timecost)
    {
        Lvl = 0;
        ScorePP = 0;
        TimeInterval = timeInterval;
        InitialCost = inicost;
        Cost = inicost;
        TimeCost = timecost;
    }
    
    public void Upgrade()
    {
        Lvl++;
        ScorePP = 10*Lvl;
        Cost += Cost/2;
    }
    public void Restart()
    {
        Lvl = 0;
        Cost = InitialCost;
    }
    public void UpgradeTime()
    {
        TimeInterval -= 1;
        TimeCost *= 2;
    }
    
}