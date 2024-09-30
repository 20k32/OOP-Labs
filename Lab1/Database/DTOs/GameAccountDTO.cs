using Lab1.GameAccounts;
using Lab1.Games;
using Lab1.Games.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace Lab1.Database.DTOs;

internal readonly struct GameAccountDTO : IMappable<StandardModeAccount>
{
    public string UserName { get; }
    public int Rating { get; }
    public string AccountType { get; }
    public LinkedList<GameHistoryUnit> History { get; }

    public GameAccountDTO(string userName, int rating, string accountType, IEnumerable<GameHistoryUnit> history)
    {
        UserName = userName;
        Rating = rating;
        History = new();
        AccountType = accountType;
        foreach(var item in history)
        {
            History.AddLast(new GameHistoryUnit(item.OpponentName, 
                item.GainedRating,
                item.Index,
                item.GameType));
        }
    }

    public void Map(out StandardModeAccount account)
    {
        var enumerableHistory = History.AsEnumerable();

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
        History.AddLast(gameHistorUnit);
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
