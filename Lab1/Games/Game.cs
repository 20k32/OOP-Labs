using Lab1.GameAccounts;
using NanoidDotNet;


namespace Lab1.Games;

internal abstract class Game
{
    private StandardModeAccount _firstPlayer;
    private StandardModeAccount _secondPlayer;

    protected const int ZERO_RATING = 0;
    protected int Rating;

    public string Id;
    public string Index;
    public bool IsCompleted { get; protected set; }
    public virtual string DisplayType => nameof(Game);

    protected Game()
    {
        IsCompleted = false;
    }

    public string FirstPlayerUserName
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_firstPlayer);
            return _firstPlayer.UserName;
        }
    }

    public string SecondPlayerUserName
    {
        get
        {
            ArgumentNullException.ThrowIfNull(_secondPlayer);
            return _secondPlayer.UserName;
        }
    }

    public void GenerateId()
    {
        Id = Nanoid.Generate();
        Index = Nanoid.Generate(Nanoid.Alphabets.NoLookAlikes, 10);
    }

    public void SetPlayers(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer)
    {
        ArgumentNullException.ThrowIfNull(firstPlayer);
        ArgumentNullException.ThrowIfNull(secondPlayer);

        _firstPlayer = firstPlayer;
        _secondPlayer = secondPlayer;
    }

    public void FirstPlayerWin()
    {
        ArgumentNullException.ThrowIfNull(_firstPlayer);
        ArgumentNullException.ThrowIfNull(_secondPlayer);
        ArgumentOutOfRangeException.ThrowIfEqual(IsCompleted, true);

        _firstPlayer.OnWinGame(this);
        _secondPlayer.OnLooseGame(this);

        var winRating = GetWinRating();
        var looseRating = GetLooseRating();

        _firstPlayer.Log(new(_secondPlayer.UserName, winRating, Index, DisplayType));
        _secondPlayer.Log(new(_firstPlayer.UserName, -looseRating, Index, DisplayType));

        IsCompleted = true;
    }

    public void FirstPlayerLoose()
    {
        ArgumentNullException.ThrowIfNull(_firstPlayer);
        ArgumentNullException.ThrowIfNull(_secondPlayer);
        ArgumentOutOfRangeException.ThrowIfEqual(IsCompleted, true);

        _firstPlayer.OnLooseGame(this);
        _secondPlayer.OnWinGame(this);

        var winRating = GetWinRating();
        var looseRating = GetLooseRating();

        _secondPlayer.Log(new(_firstPlayer.UserName, winRating, Index, DisplayType));
        _firstPlayer.Log(new(_secondPlayer.UserName, -looseRating, Index, DisplayType));

        IsCompleted = true;
    }

    public abstract void RerollRating();
    public virtual int GetWinRating() => Rating;
    public virtual int GetLooseRating() => Rating;

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
