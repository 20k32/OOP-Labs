﻿using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Games;

internal sealed class TrainingGame : Game
{
    public override void RerollRating()
    {
        Rating = ZERO_RATING;
    }

    public override string DisplayType => "Training game";
}
