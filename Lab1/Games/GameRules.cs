using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games
{
    internal static class GameRules
    {
        private const int WINNER_GAME_RATING_LOWER_BOUND = 50;
        private const int WINNER_GAME_RATING_UPPER_BOUND = 200;

        private const int STANDARD_GAME_RATING_LOWER_BOUND = 10;
        private const int STANDARD_GAME_RATING_UPPER_BOUND = 100;

        public const int ZERO_RATING = 0;
        public const int HARD_MODE_ACCOUNT_WIN_STREAK = 5;

        public static int GenerateRatingForRatingToWinnerGame() 
            => Random.Shared.Next(WINNER_GAME_RATING_LOWER_BOUND, WINNER_GAME_RATING_UPPER_BOUND);

        public static int GenerateRatingForStandardGame()
             => Random.Shared.Next(STANDARD_GAME_RATING_LOWER_BOUND, STANDARD_GAME_RATING_UPPER_BOUND);

        public static int GenerateRatingForTrainingGame()
            => ZERO_RATING;
    }
}
