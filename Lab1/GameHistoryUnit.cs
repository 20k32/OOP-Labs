namespace Lab1;

internal sealed class GameHistoryUnit
{
    public readonly string OpponentName;
    public readonly int GainedRating;
    public readonly string Index;


    public GameHistoryUnit(string opponentName, int gainedRating, string index) =>
        (OpponentName, GainedRating, Index) = (opponentName, gainedRating, index);

    public override string ToString() => $"{OpponentName}\t{(GainedRating > 0 ? "Win" : "Lost")}\t{GainedRating}\t{Index}";
}
