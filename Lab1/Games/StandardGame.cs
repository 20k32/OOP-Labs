using Lab1.GameAccounts;


namespace Lab1.Games;

internal sealed class StandardGame : Game
{

    public override void RerollRating()
        => Rating = GameRules.GenerateRatingForStandardGame();

    public override string DisplayType => GameTypes.StandardGame.BaseName;
}
