namespace Lab1.Games.Logging;

internal sealed class GameHistoryUnit
{
    public readonly string OpponentName;
    public readonly int GainedRating;
    public readonly string Index;
    public readonly string GameType;

    public GameHistoryUnit(string opponentName, int gainedRating, string index, string gameType) =>
        (OpponentName, GainedRating, Index, GameType) = (opponentName, gainedRating, index, gameType);

    public override string ToString() => $"{OpponentName}\t{(GainedRating > 0 ? "Win" : "Lost")}\t{GainedRating}\t{Index}\t{GameType}";
    
    public override bool Equals(object obj)
    {
        bool result = false;

        if(obj is GameHistoryUnit unit)
        {
            result = Index.Equals(unit.Index);
        }

        return result;
    }

    public override int GetHashCode() => Index.GetHashCode();
}
