using Lab1.Database.DTOs;
using Lab1.Database.Repository;
using Lab1.Games;

namespace Lab1.Database.Service;

internal sealed class GameService : IService<Game>
{
    private readonly IAsyncRepository<GameDTO> _gameRepository;

    public GameService(IAsyncRepository<GameDTO> gameRepsitory)
    {
        _gameRepository = gameRepsitory;
    }

    public Task AddEntityAsync(Game entity)
    {
        entity.Map(out var gameDto);
        return _gameRepository.AddAsync(gameDto);
    }

    public Task ClearDatabaseAsync()
        => _gameRepository.ClearAsync();

    public async IAsyncEnumerable<Game> GetAllEntitiesAsync()
    {
        await foreach (var item in _gameRepository.GetAllAsync())
        {
            item.Map(out var gameEntity);
            yield return gameEntity;
        }
    }

    public async Task<Game> GetEntityByUniqueIdentifierAsync(string id)
    {
        var entity = await _gameRepository.GetByUniqueIdentifierAsync(id);
        entity.Map(out var gameEntity);
        
        return gameEntity;
    }

    public Task LoadDataAsync() => _gameRepository.LoadAsync();

    public async Task RemoveEntityAsync(Game entity)
    {
        entity.Map(out var gameDto);
        await _gameRepository.RemoveAsync(gameDto);
    }

    public async Task UpdateEntityAsync(Game entity)
    {
        entity.Map(out var gameDto);
        await _gameRepository.UpdateAsync(gameDto);
    }
}
