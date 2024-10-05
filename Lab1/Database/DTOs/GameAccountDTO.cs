using Lab1.GameAccounts;
using Lab1.Games;
using Lab1.Games.Logging;
using Lab1.Mapper;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace Lab1.Database.DTOs;

[Mappable(typeof(StandardModeAccount))]
internal struct GameAccountDTO : IMappable<StandardModeAccount>
{
    private string _userName;

    public string UserName => _userName;

    private int _rating;

    public int Rating => _rating;

    private string _displayType;

    public string AccountType => _displayType;

    private LinkedList<GameHistoryUnit> _gameHistory;

    public LinkedList<GameHistoryUnit> GameHistory => _gameHistory;

    public GameAccountDTO(string userName, int rating, string accountType, IEnumerable<GameHistoryUnit> history)
    {
        _userName = userName;
        _rating = rating;
        _gameHistory = new();
        _displayType = accountType;

        foreach(var item in history)
        {
            GameHistory.AddLast(new GameHistoryUnit(item.OpponentName, 
                item.GainedRating,
                item.Index,
                item.GameType));
        }
    }

    public void Map(out StandardModeAccount account)
    {
        var enumerableHistory = GameHistory.AsEnumerable();

        if (AccountType == AccountTypes.StandardModeAccount.BaseName)
        {
            account = new StandardModeAccount(UserName, enumerableHistory, Rating);
        }
        else if (AccountType == AccountTypes.HardModeAccount.BaseName)
        {
            account = new HardModeAccount(UserName, enumerableHistory, GameRules.HARD_MODE_ACCOUNT_WIN_STREAK, Rating);
        }
        else if (AccountType == AccountTypes.ArcadeModeAccount.BaseName)
        {
            account = new ArcadeModeAccount(UserName, enumerableHistory, Rating);
        }

        else account = null;
    }

    public void AddGameToHistory(GameHistoryUnit gameHistorUnit)
    {
        GameHistory.AddLast(gameHistorUnit);
    }

    public override bool Equals(object obj)
    {
        bool result = false;

        if(obj is GameAccountDTO dto)
        {
            result = dto.UserName == UserName;
        }

        return result;
    }

    public override int GetHashCode() => UserName.GetHashCode();
}
