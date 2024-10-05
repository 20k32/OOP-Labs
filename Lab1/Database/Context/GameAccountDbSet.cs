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
using Lab1.Shared;

namespace Lab1.Database.Context;

internal sealed class GameAccountDbSet : AsyncDbSet<GameAccountDTO>
{
    public GameAccountDbSet() : base()
    { }

    public override async Task InitAsync()
    {
        await Task.Delay(100);
        Data = new();

        foreach (var item in Enumerable.Range('a', 'z' - 'a' + 1))
        {
            var randomIndex = Random.Shared.Next(0, 3);
            var accountName = ((char)item).ToString();
            string accountType = string.Empty;

            if (randomIndex == Resolver.AccountTypes.StandardModeAccount.AssociatedIndex)
            {
                accountType = Resolver.AccountTypes.StandardModeAccount.BaseName;
            }
            else if (randomIndex == Resolver.AccountTypes.HardModeAccount.AssociatedIndex)
            {
                accountType = Resolver.AccountTypes.HardModeAccount.BaseName;
            }
            else
            {
                accountType = Resolver.AccountTypes.ArcadeModeAccount.BaseName;
            }

            Data.AddLast(new GameAccountDTO(accountName, 1, accountType, Enumerable.Empty<GameHistoryUnit>()));
        }
    }
}
