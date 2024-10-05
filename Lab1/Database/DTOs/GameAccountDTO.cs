using Lab1.GameAccounts;
using Lab1.Games;
using Lab1.Games.Logging;
using Lab1.Mapper;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace Lab1.Database.DTOs;

[Mappable(typeof(StandardModeAccount))]
internal struct GameAccountDTO
{
    private string _userName;
    public string UserName => _userName;

    private int _rating;
    public int Rating => _rating;

    private string _displayType;
    public string AccountType => _displayType;

    private LinkedList<GameHistoryUnit> _gameHistory;
    public LinkedList<GameHistoryUnit> GameHistory => _gameHistory;

    private uint _gamesCount;
    public uint GamesCount => _gamesCount;

    public GameAccountDTO(string userName, int rating, string accountType, IEnumerable<GameHistoryUnit> history)
    {
        _gamesCount = 0;
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
        => SimpleMapper.Map(this, out account);

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

    public new Type GetType() => typeof(GameAccountDTO);
}
