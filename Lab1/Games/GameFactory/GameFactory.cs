namespace Lab1.Games.GameFactory;

internal static class GameFactory
{
    private static void SetupGame(Game game)
    {
        game.RerollRating();
    }

    public static Game CreateStandradGame()
    {
        var result = (StandardGame)Activator.CreateInstance(typeof(StandardGame), true);
        SetupGame(result);
        return result;
    }


    public static Game CreateTrainingGame()
    {
        var result = (TrainingGame)Activator.CreateInstance(typeof(TrainingGame), true);
        SetupGame(result);
        return result;
    }


    public static Game CreateRatingToWinnerGame()
    {
        var result = (RatingToWinnerGame)Activator.CreateInstance(typeof(RatingToWinnerGame), true);
        SetupGame(result);
        return result;
    }
}
