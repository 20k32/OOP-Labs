using Lab1.Database.DTOs;
using Lab1.Games;
using Lab1.Games.Logging;
using Lab1.Mapper;
using Lab1.Shared;

namespace Lab1.GameAccounts;

[Mappable(typeof(GameAccountDTO))]
internal class StandardModeAccount
{
    protected LinkedList<GameHistoryUnit> _gameHistory;

    private string _userName;

    public string UserName
    {
        get => _userName;
        private set => _userName = value;
    }

    private int _rating;

    public int CurrentRating
    {
        get => _rating;
        private set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);

            if (CurrentRating != value)
            {
                _rating = value;
            }
        }
    }

    private uint _gamesCount;

    public IEnumerable<GameHistoryUnit> GetHistory() => _gameHistory;

    private void InitFields()
    {
        var unit = Resolver.ResolveAccountType(this);
        _displayType = unit.BaseName;
    }

    protected StandardModeAccount()
    {
        InitFields();
    }

    public StandardModeAccount(string userName, int rating = 1, uint gamesCount = 0)
    {
        InitFields();
        UserName = userName;
        CurrentRating = rating;
        _gamesCount = gamesCount;
        _gameHistory = new();
    }

    public StandardModeAccount(string userName, IEnumerable<GameHistoryUnit> games, int rating = 1, uint gamesCount = 0)
    {
        InitFields();
        UserName = userName;
        CurrentRating = rating;
        _gamesCount = gamesCount;
        _gameHistory = new();

        if (games is not null)
        {
            foreach (var item in games)
            {
                _gameHistory.AddLast(item);
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
            _gamesCount++;
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
            _gamesCount++;
        }
    }

    public void Log(GameHistoryUnit gameHistory) => _gameHistory.AddLast(gameHistory);

    public override bool Equals(object obj)
    {
        bool result = false;

        if (obj is StandardModeAccount account)
        {
            result = UserName.Equals(account.UserName);
        }

        return result;
    }

    public override int GetHashCode() => UserName.GetHashCode();

    public void Map(out GameAccountDTO entity) 
        => SimpleMapper.Map(this, out entity);

    protected string _displayType;
    public string DisplayType => _displayType;

    public override string ToString()
    {
        return $"[u:{UserName} r:{CurrentRating} t:{DisplayType}]";
    }

    public void SetRating(int rating) => CurrentRating = rating;
}
