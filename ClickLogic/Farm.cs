namespace ClickBoxin;

public class Farm
{
    public int ScoreIncrement { get; set; }
    public int TimeInterval { get; set; }
    public int Cost { get; set; }

    public Farm(int scoreIncrement, int timeInterval, int cost)
    {
        ScoreIncrement = scoreIncrement;
        TimeInterval = timeInterval;
        Cost = cost;
    }
}