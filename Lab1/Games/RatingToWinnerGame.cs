using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games;

internal sealed class RatingToWinnerGame : Game
{

    private RatingToWinnerGame() : base()
    { }

    private RatingToWinnerGame(string id) : base(id) { } 

    public override int GetLooseRating() => GameRules.ZERO_RATING;

    public override void RerollRating() 
        => Rating = GameRules.GenerateRatingForRatingToWinnerGame();

    public override string DisplayType => GameTypes.RatingToWinnerGame.BaseName;
}
