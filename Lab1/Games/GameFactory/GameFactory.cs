namespace Lab1.Games.GameFactory;

internal static class GameFactory
{
    private static void SetupGame(Game game)
    {
        game.RerollRating();
    }

    public static Game CreateRatingToWinnerGame(string id)
    {
        var result = (RatingToWinnerGame)Activator.CreateInstance(typeof(RatingToWinnerGame), id);
        SetupGame(result);
        return result;
    }

    public static Game CreateStandradGame(string id)
    {
        var result = (StandardGame)Activator.CreateInstance(typeof(StandardGame), id);
        SetupGame(result);
        return result;
    }

    public static Game CreateTrainingGame(string id)
    {
        var result = (TrainingGame)Activator.CreateInstance(typeof(TrainingGame), id);
        SetupGame(result);
        return result;
    }

    public static Game CreateStandradGame()
    {
        var result = Activator.CreateInstance<StandardGame>();
        SetupGame(result);
        return result;
    }


    public static Game CreateTrainingGame()
    {
        var result = Activator.CreateInstance<TrainingGame>();
        SetupGame(result);
        return result;
    }


    public static Game CreateRatingToWinnerGame()
    {
        var result = Activator.CreateInstance<RatingToWinnerGame>();
        SetupGame(result);
        return result;
    }
}
