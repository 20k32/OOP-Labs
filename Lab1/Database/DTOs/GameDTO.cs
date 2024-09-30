using Lab1.Games;
using Lab1.Games.GameFactory;
using NanoidDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Database.DTOs;

internal readonly struct GameDTO : IMappable<Game>
{
    public string Id { get; }
    public int Rating { get; }
    public string GameType { get; }

    public GameDTO(string id, int rating, string gameType)
        => (Id, Rating, GameType) = (id, rating, gameType);

    public GameDTO()
    {
        Id = Nanoid.Generate();
    }

    public void Map(out Game entity)
    {
        if (GameType == GameTypes.StandardGame.BaseName)
        {
            entity = new StandardGame()
            {
                Id = Id
            };
        }
        else if (GameType == GameTypes.RatingToWinnerGame.BaseName)
        {
            entity = new RatingToWinnerGame()
            {
                Id = Id
            };
        }
        else if (GameType == GameTypes.TrainingGame.BaseName)
        {
            entity = new TrainingGame()
            {
                Id = Id
            };
        }
        else
        {
            entity = null;
        }

        entity?.SetRating(Rating);
    }

    public override bool Equals([NotNullWhen(true)] object obj)
    {
        bool result = false;

        if(obj is GameDTO gameDto)
        {
            result = Id == gameDto.Id;
        }

        return result;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
