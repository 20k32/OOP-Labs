using Lab1.GameAccounts;
using Lab1.Games.Logging;
using NanoidDotNet;


namespace Lab1.Games;

internal abstract class Game
{
    protected const int ZERO_RATING = 0;
    protected int Rating;

    public StandardModeAccount FirstPlayer;
    public StandardModeAccount SecondPlayer;
    public string Id;
    public string Index;

    public void SetPlayers(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer)
    {
        FirstPlayer = firstPlayer;
        SecondPlayer = secondPlayer;
    }

    public virtual string DisplayType => nameof(Game);

    public void GenerateId()
    {
        Id = Nanoid.Generate();
        Index = Nanoid.Generate(Nanoid.Alphabets.NoLookAlikes, 10);
    }

    public abstract void RerollRating();
    public virtual int GetWinRating() => Rating;
    public virtual int GetLooseRating() => Rating;

    public abstract void FirstPlayerWin();
    public abstract void FirstPlayerLoose();

    public sealed override bool Equals(object obj)
    {
        bool result = false;
        if (obj is Game game)
        {
            result = Id.Equals(game.Id);
        }

        return result;
    }

    public sealed override int GetHashCode() => Id.GetHashCode();
}
