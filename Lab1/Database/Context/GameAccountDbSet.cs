using Lab1.Database.DTOs;
using Lab1.GameAccounts;
using Lab1.Games.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab1.Database.Context;

internal sealed class GameAccountDbSet : IAsyncDbSet<GameAccountDTO>
{
    private LinkedList<GameAccountDTO> _data;

    public GameAccountDbSet()
    {
        _data = new();
    }


    public async Task InitAsync()
    {
        await Task.Delay(100);

        foreach (var item in Enumerable.Range('a', 'z' - 'a' + 1))
        {
            var randomIndex = Random.Shared.Next(0, 3);
            var accountName = ((char)item).ToString();
            string accountType = string.Empty;

            if (randomIndex == AccountTypes.StandardModeAccount.AssociatedIndex)
            {
                accountType = AccountTypes.StandardModeAccount.BaseName;
            }
            else if (randomIndex == AccountTypes.HardModeAccount.AssociatedIndex)
            {
                accountType = AccountTypes.HardModeAccount.BaseName;
            }
            else
            {
                accountType = AccountTypes.ArcadeModeAccount.BaseName;
            }

            _data.AddLast(new GameAccountDTO(accountName, 1, accountType, Enumerable.Empty<GameHistoryUnit>()));
        }
    }

    public async IAsyncEnumerable<GameAccountDTO> GetAllAsync()
    {
        if(_data is null)
        {
            yield break;
        }

        foreach(var item in _data)
        {
            await Task.Delay(100);
            yield return item;
        }
    }

    public async Task<GameAccountDTO> GetOneAsync(GameAccountDTO account)
    {
        if (_data is null)
        {
            return default;
        }

        GameAccountDTO result = default;

        await Task.Delay(100);
        var node = _data.Find(account);

        if (node is not null)
        {
            result = node.Value;
        }

        return result;
    }

    public async Task RemoveOneAsync(GameAccountDTO account)
    {
        if (_data is null)
        {
            return;
        }

        await Task.Delay(100);

        var node = _data.Find(account);

        if (node is not null)
        {
            _data.Remove(node);
        }
    }

    public async Task UpdateOneAsync(GameAccountDTO account)
    {
        if (_data is null)
        {
            return;
        }

        await Task.Delay(100);

        GameAccountDTO itemToDelete = default;

        foreach(var item in _data)
        {
            if (item.Equals(account))
            {
                itemToDelete = item;
                break;
            }
        }
        
        if (!itemToDelete.Equals(account)) 
        {
            throw new ArgumentOutOfRangeException($"{nameof(account)} is out of range of valid values.");
        }

        _data.Remove(account);
        _data.AddLast(account);
    }

    public async Task AddOneAsync(GameAccountDTO entity)
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
}
