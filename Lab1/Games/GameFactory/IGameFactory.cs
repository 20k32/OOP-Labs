using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games.GameFactory
{
    internal interface IGameFactory
    {
        Game CreateStandradGame();
        Game CreateRatingToWinnerGame();
        Game CreateTrainingGame();
    }
}
