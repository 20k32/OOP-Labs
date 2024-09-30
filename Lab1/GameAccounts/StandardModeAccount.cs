using DoubleLinkedList;
using Lab1.Database.DTOs;
using Lab1.Games;
using Lab1.Games.Logging;

namespace Lab1.GameAccounts;

internal class StandardModeAccount : IMappable<GameAccountDTO>
{
    protected readonly DoubleLinkedList<GameHistoryUnit> GameHistory;

    private int _currentRating;
    
    public string UserName { get; private set; }
    
    public int CurrentRating
    {
        get => _currentRating;
        private set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);

            if (CurrentRating != value)
            {
                _currentRating = value;
            }
        }
    }

    public uint GamesCount { get; private set; }
    
    public IEnumerable<GameHistoryUnit> GetHistory() => GameHistory.ReadFromHead();

    public StandardModeAccount(string userName, int rating = 1, uint gamesCount = 0)
    {
        UserName = userName;
        CurrentRating = rating;
        GamesCount = gamesCount;
        GameHistory = new();
    }

    public StandardModeAccount(string userName, IEnumerable<GameHistoryUnit> games, int rating = 1, uint gamesCount = 0)
    {
        UserName = userName;
        CurrentRating = rating;
        GamesCount = gamesCount;
        GameHistory = new();

        if(games is not null)
        {
            foreach (var item in games)
            {
                GameHistory.AddToEnd(item);
            }
        }
    } 
    
    protected virtual int CalculateWinRating(int rawRating) => rawRating;
    
    protected virtual int CalculateLooseRating(int rawRating) => rawRating;
    
    public void OnLooseGame(Game game)
    {
        var rawRating = game.GetLooseRating();
        var tempRating = CalculateLooseRating(rawRating);

        try
        {
            CurrentRating -= tempRating;
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

    public void OnWinGame(Game game)
    {
        var rawRating = game.GetWinRating();
        var tempRating = CalculateWinRating(rawRating);
        try
        {
            checked
            {
                CurrentRating += tempRating;
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

    public void Log(GameHistoryUnit gameHistory) => GameHistory.AddToEnd(gameHistory);

    public override bool Equals(object obj)
    {
        bool result = false;

        if(obj is StandardModeAccount account)
        {
            result = UserName.Equals(account.UserName);
        }

        return result;
    }

    public override int GetHashCode() => UserName.GetHashCode();

    public void Map(out GameAccountDTO entity)
    {
        entity = new(UserName, CurrentRating, DisplayType, GameHistory.ReadFromHead());
    }

    public virtual string DisplayType => AccountTypes.StandardModeAccount.BaseName;

    public override string ToString()
    {
        return $"[u:{UserName} r:{CurrentRating} t:{DisplayType}]";
    }

    public void SetRating(int rating) => CurrentRating = rating;
}
