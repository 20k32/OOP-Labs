using Lab1.GameAccounts;
using Lab1.Shared;

namespace Lab1.Games;

internal sealed class StandardGame : Game
{
    private StandardGame() : base()
    { }

    private StandardGame(string id) : base(id)
    { }

    public override void RerollRating()
        => Rating = GameRules.GenerateRatingForStandardGame();
}
