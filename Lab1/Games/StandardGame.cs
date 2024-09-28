using Lab1.GameAccounts;


namespace Lab1.Games;

internal sealed class StandardGame : Game
{
    private const int STANDARD_GAME_RATING_LOWER_BOUND = 10;
    private const int STANDARD_GAME_RATING_UPPER_BOUND = 100;

    public override void FirstPlayerLoose()
    {
        FirstPlayer.OnLooseGame(this);
        SecondPlayer.OnWinGame(this);
    }

    public override void FirstPlayerWin()
    {
        FirstPlayer.OnWinGame(this);
        SecondPlayer.OnLooseGame(this);
    }

    public override void RerollRating()
    {
        Rating = Random.Shared.Next(STANDARD_GAME_RATING_LOWER_BOUND, STANDARD_GAME_RATING_UPPER_BOUND);
    }

    public override string DisplayType => "Standard game";
}
