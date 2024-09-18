using DoubleLinkedList;

namespace Lab1;

internal sealed class GameAccount
{
    private readonly DoubleLinkedList<GameHistoryUnit> _gameHistory;
    public string UserName { get; private set; }

    private int _currentRating;
    public int CurrentRating
    {
        get => _currentRating;
        set
        {
            if(value < 1 
               || _currentRating + value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            if (CurrentRating != value)
            {
                _currentRating = value;
            }
        }
    }
    public uint GamesCount { get; private set; }

    public GameAccount(string userName, int rating = 1, uint gamesCount = 0)
    {
        UserName = userName;
        CurrentRating = rating;
        GamesCount = gamesCount;
        _gameHistory = new();
    }

    public IEnumerable<GameHistoryUnit> GetHistory() => _gameHistory.ReadFromHead();

    public void OnWinGame(string opponentName, int rating)
    {
        try
        {
            checked
            {
                CurrentRating += rating;
            }
        }
        catch (OverflowException)
        {
            CurrentRating = int.MaxValue;
        }
        finally
        {
            GamesCount++;
        }
    }

    public void OnLooseGame(string opponentName, int rating)
    {
        try
        {
            CurrentRating -= rating;
        }
        catch (ArgumentOutOfRangeException)
        {
            CurrentRating = 1;
        }
        finally
        {
            GamesCount++;
        }
    }

    public void Log(GameHistoryUnit game) 
        => _gameHistory.AddToEnd(game);

    public override bool Equals(object obj)
    {
        bool result = false;

        if(obj is GameAccount account)
        {
            result = UserName.Equals(account.UserName);
        }

        return result;
    }

    public override int GetHashCode() => UserName.GetHashCode();
}
