using Lab1.Database.DTOs;
using Lab1.GameAccounts;
using Lab1.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Database.Context;

internal interface IAsyncDbContext
{
    IAsyncDbSet<GameAccountDTO> AccountsDbSet { get; }
    IAsyncDbSet<GameDTO> GamesDbSet { get; }
}
