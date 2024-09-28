using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games.GameFactory;

internal interface IGamePool
{
    Game CreateStandradGame(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer);
    Game CreateRatingToWinnerGame(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer);
    Game CreateTrainingGame(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer);
}
