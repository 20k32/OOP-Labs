using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games
{
    internal static class GameTypes
    {
        public static readonly TypeUnit StandardGame = new(0, "Standard");
        public static readonly TypeUnit RatingToWinnerGame = new(1, "Rating to winner");
        public static readonly TypeUnit TrainingGame = new(2, "Training");
    }
}
