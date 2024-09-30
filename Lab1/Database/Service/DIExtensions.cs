using Lab1.Database.DTOs;
using Lab1.Database.Repository;
using Lab1.GameAccounts;
using Lab1.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Database.Service;

internal static class DIExtensions
{
    public static void ConfigureService()
    {
        var gameAccountRepository = IocContainer.GetService<IAsyncRepository<GameAccountDTO>>();
        var gameRepository = IocContainer.GetService<IAsyncRepository<GameDTO>>();

        IService<Game> gameService = new GameService(gameRepository);
        IService<StandardModeAccount> gameAccountService = new AccountService(gameAccountRepository);

        IocContainer.AddService(gameService);
        IocContainer.AddService(gameAccountService);
    }
}
