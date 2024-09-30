using Lab1.Database.Context;
using Lab1.Database.DTOs;
using Lab1.Games.Logging;

namespace Lab1.Database.Repository;

internal sealed class GameAccountRepository : IAsyncRepository<GameAccountDTO>
{
    private readonly IAsyncDbContext _dbContext;

    public GameAccountRepository(IAsyncDbContext context)
        => _dbContext = context;

    public Task AddAsync(GameAccountDTO entity) 
        => _dbContext.AccountsDbSet.AddOneAsync(entity);

    public Task ClearAsync() => _dbContext.AccountsDbSet.ClearAsync();

    public IAsyncEnumerable<GameAccountDTO> GetAllAsync() 
        => _dbContext.AccountsDbSet.GetAllAsync();

    public Task<GameAccountDTO> GetByUniqueIdentifierAsync(string name)
    {
        var accountDto = new GameAccountDTO(name, default, default, Enumerable.Empty<GameHistoryUnit>());

        return _dbContext.AccountsDbSet.GetOneAsync(accountDto);
    }

    public Task LoadAsync() => _dbContext.AccountsDbSet.InitAsync();

    public Task RemoveAsync(GameAccountDTO entity) 
        => _dbContext.AccountsDbSet.RemoveOneAsync(entity);

    public Task UpdateAsync(GameAccountDTO entity) 
        => _dbContext.AccountsDbSet.UpdateOneAsync(entity);
}
