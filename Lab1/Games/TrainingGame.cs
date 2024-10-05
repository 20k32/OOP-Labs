using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games;

internal sealed class TrainingGame : Game
{
    private TrainingGame() : base()
    { }

    private TrainingGame(string id) : base(id) { }

    public override void RerollRating()
        => Rating = GameRules.GenerateRatingForTrainingGame();


    public override string DisplayType => GameTypes.TrainingGame.BaseName;
}
