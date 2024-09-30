using Lab1.Database.DTOs;
using Lab1.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Database.Context;

internal sealed class GameDbSet : IAsyncDbSet<GameDTO>
{
    private LinkedList<GameDTO> _data;

    public GameDbSet()
    { }

    public async Task AddOneAsync(GameDTO entity)
    {
        if (_data is null)
        {
            return;
        }

        await Task.Delay(100);
        _data.AddLast(entity);
    }

    public async Task ClearAsync()
    {
        if (_data is null)
        {
            return;
        }

        await Task.Delay(100);
        _data.Clear();
    }

    public async IAsyncEnumerable<GameDTO> GetAllAsync()
    {
        if (_data is null)
        {
            yield break;
        }

        foreach (var item in _data)
        {
            await Task.Delay(100);
            yield return item;
        }
    }

    public async Task<GameDTO> GetOneAsync(GameDTO game)
    {
        if (_data is null)
        {
            return default;
        }

        GameDTO result = default;

        await Task.Delay(100);
        var node = _data.Find(game);

        if (node is not null)
        {
            result = node.Value;
        }

        return result;
    }

    public async Task InitAsync()
    {
        await Task.Delay(100);
        _data = new();
    }

    public async Task RemoveOneAsync(GameDTO game)
    {
        if (_data is null)
        {
            return;
        }

        await Task.Delay(100);

        var node = _data.Find(game);

        if (node is not null)
        {
            _data.Remove(node);
        }
    }

    public async Task UpdateOneAsync(GameDTO game)
    {
        if (_data is null)
        {
            return;
        }

        await Task.Delay(100);

        var node = _data.Find(game);
        if (node is not null)
        {
            _data.AddBefore(node, game);
            _data.Remove(node);
        }
    }
}
