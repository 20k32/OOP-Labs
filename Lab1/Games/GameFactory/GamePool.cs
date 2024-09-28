using Lab1.GameAccounts;
namespace Lab1.Games.GameFactory;

internal class GamePool : IGamePool
{
    private readonly Game _standardGame;
    private readonly Game _ratingToWinnerGame;
    private readonly Game _trainingGame;

    public GamePool()
    {
        _standardGame = new StandardGame();
        _trainingGame = new TrainingGame();
        _ratingToWinnerGame = new RatingToWinnerGame();
    }

    private static Game SetupGame(Game game, StandardModeAccount firstPlayer, StandardModeAccount secondPlayer)
    {
        game.GenerateId();
        game.SetPlayers(firstPlayer, secondPlayer);
        game.RerollRating();
        return game;
    }

    public Game CreateRatingToWinnerGame(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer)
        => SetupGame(_ratingToWinnerGame, firstPlayer, secondPlayer);

    public Game CreateStandradGame(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer) 
        => SetupGame(_standardGame, firstPlayer, secondPlayer);

    public Game CreateTrainingGame(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer)
        => SetupGame(_trainingGame, firstPlayer, secondPlayer);
}
