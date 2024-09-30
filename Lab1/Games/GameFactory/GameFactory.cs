using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games.GameFactory;

internal class GameFactory : IGameFactory
{
    private GameFactory()
    { }

    private static GameFactory _instance;
    public static GameFactory Instance => _instance ??= new GameFactory();

    private void SetupGame(Game game)
    {
        game.GenerateId();
        game.RerollRating();
    }

    public Game CreateRatingToWinnerGame()
    {
        var result = new RatingToWinnerGame();
        SetupGame(result);
        return result;
    }

    public Game CreateStandradGame()
    {
        var result = new StandardGame();
        SetupGame(result);
        return result;
    }

    public Game CreateTrainingGame()
    {
        var result = new TrainingGame();
        SetupGame(result);
        return result;
    }
}
