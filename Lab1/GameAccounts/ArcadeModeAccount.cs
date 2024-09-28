using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.GameAccounts;

internal sealed class ArcadeModeAccount : StandardModeAccount
{
    public ArcadeModeAccount(string userName, int rating = 1, uint gamesCount = 0) : base(userName, rating, gamesCount)
    { }

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

    public override string DisplayType => "Arcade account";
}
