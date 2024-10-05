using Lab1.Games;
using Lab1.Games.GameFactory;
using Lab1.Mapper;
using NanoidDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Database.DTOs;

[Mappable(typeof(Game))]
internal struct GameDTO
{
    private string _id;
    public string Id => _id;

    private int _rating;
    public int Rating => _rating;

    private string _displayType;
    public string DisplayType => _displayType;

    public GameDTO(string id, int rating, string displayType)
        => (_id, _rating, _displayType) = (id, rating, displayType);

    public GameDTO()
    {
        //_id = Nanoid.Generate();
    }

    public void Map(out Game entity) => SimpleMapper.Map(this, out entity);

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

    public new Type GetType() => typeof(GameDTO);
}
