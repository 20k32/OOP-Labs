using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games;

internal sealed class RatingToWinnerGame : Game
{
    private const int WINNER_GAME_RATING_LOWER_BOUND = 10;
    private const int WINNER_GAME_RATING_UPPER_BOUND = 100;

    public override int GetLooseRating() => ZERO_RATING;

    public override void RerollRating() 
        => Rating = Random.Shared.Next(WINNER_GAME_RATING_LOWER_BOUND, WINNER_GAME_RATING_UPPER_BOUND);

    public override string DisplayType => "Rating to winner game";
}
