using Lab1.GameAccounts;


namespace Lab1.Games;

internal sealed class StandardGame : Game
{

    private StandardGame() : base()
    { }

    private StandardGame(string id) : base(id)
    { }

    public override void RerollRating()
        => Rating = GameRules.GenerateRatingForStandardGame();

    public override string DisplayType => GameTypes.StandardGame.BaseName;
}
