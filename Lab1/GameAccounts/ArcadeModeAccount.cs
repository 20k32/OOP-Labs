using Lab1.Games.Logging;
using Lab1.Shared;

namespace Lab1.GameAccounts;

internal sealed class ArcadeModeAccount : StandardModeAccount
{
    public ArcadeModeAccount()
    { }

    public ArcadeModeAccount(string userName, int rating = 1, uint gamesCount = 0) : base(userName, rating, gamesCount)
    {
        _displayType = Resolver.AccountTypes.ArcadeModeAccount.BaseName;
    }

    public ArcadeModeAccount(string userName, IEnumerable<GameHistoryUnit> games, int rating = 1, uint gamesCount = 0) : base(userName, games, rating, gamesCount)
    {
        _displayType = Resolver.AccountTypes.ArcadeModeAccount.BaseName;
    }

    protected override int CalculateLooseRating(int rawRating)
    {
        int resultRating = rawRating;

        try
        {
            checked 
            {
                resultRating = resultRating / 2 + resultRating % 2; 
            };
        }
        catch (OverflowException)
        { }

        return resultRating;
    }
}
