using Lab1.Database.Context;
using Lab1.Database.DTOs;

namespace Lab1.Database.Repository;

internal static class DIExtensions
{
    public static void ConfigureRepository()
    {
        IAsyncDbContext dbContext = IocContainer.GetService<IAsyncDbContext>();
        IAsyncRepository<GameDTO> gameRepository = new GameRepository(dbContext);
        IAsyncRepository<GameAccountDTO> accountRepository = new GameAccountRepository(dbContext);

        IocContainer.AddService(gameRepository);
        IocContainer.AddService(accountRepository);
    }
}
