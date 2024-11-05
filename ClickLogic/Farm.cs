namespace ClickBoxin;

public class Farm
{
    public int ScoreIncrement { get; set; }
    public int TimeInterval { get; set; }
    public int InitialCost { get; set; }
    public int Cost { get; set; }

    public Farm(int scoreIncrement, int timeInterval, int inicost)
    {
        ScoreIncrement = scoreIncrement;
        TimeInterval = timeInterval;
        InitialCost = inicost;
        Cost = inicost;
    }
}