using Lab1.Database.Context;
using Lab1.Database.DTOs;
using Lab1.Games;

namespace Lab1.Database.Repository;

internal sealed class GameRepository : IAsyncRepository<GameDTO>
{
    private readonly IAsyncDbContext _dbContext;

    public GameRepository(IAsyncDbContext context) 
        => _dbContext = context;

    public Task LoadDataAsync()
        => _dbContext.GamesDbSet.InitAsync();

    public Task AddAsync(GameDTO entity) 
        => _dbContext.GamesDbSet.AddOneAsync(entity);

    public Task ClearAsync() 
        => _dbContext.GamesDbSet.ClearAsync();

    public Task<GameDTO> GetByUniqueIdentifierAsync(string id)
    {
        var gameDto = new GameDTO(id,
            default(int),
            string.Empty);

        return _dbContext.GamesDbSet.GetOneAsync(gameDto);
    }

    public Task RemoveAsync(GameDTO entity) 
        => _dbContext.GamesDbSet.RemoveOneAsync(entity);

    public IAsyncEnumerable<GameDTO> GetAllAsync() 
        => _dbContext.GamesDbSet.GetAllAsync();

    public Task UpdateAsync(GameDTO entity) => _dbContext.GamesDbSet.UpdateOneAsync(entity);

    public Task LoadAsync() => _dbContext.GamesDbSet.InitAsync();
}
