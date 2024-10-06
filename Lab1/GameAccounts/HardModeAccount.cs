using Lab1.Games;
using Lab1.Games.Logging;
using Lab1.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.GameAccounts;

internal sealed class HardModeAccount : StandardModeAccount
{
    private readonly int _winStreak;

    public HardModeAccount() : base()
    {
        _winStreak = GameRules.HARD_MODE_ACCOUNT_WIN_STREAK;
    }

    public HardModeAccount(string userName, int winStreak, int rating = 1, uint gamesCount = 0) : base(userName, rating, gamesCount)
    {
        _winStreak = winStreak;
    }

    public HardModeAccount(string userName, IEnumerable<GameHistoryUnit> games, int winStreak, int rating = 1, uint gamesCount = 0): base(userName, games, rating, gamesCount)
    { 
        _winStreak = winStreak;
    }

    protected override int CalculateWinRating(int rawRating)
    {
        int resultRating = rawRating;

        var lastMatches = _gameHistory
            .Take(_winStreak)
            .Where(match => match.GainedRating > 0);

        if(lastMatches.Count() == _winStreak)
        {
            try
            {
                checked
                {
                    resultRating = rawRating / _winStreak + rawRating % _winStreak;
                }
            }
            catch (OverflowException)
            {
                resultRating = rawRating;
            }
        }

        return resultRating;
    }
}
