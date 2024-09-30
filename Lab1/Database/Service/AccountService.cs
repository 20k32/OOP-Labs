using Lab1.Database.DTOs;
using Lab1.Database.Repository;
using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Database.Service
{
    internal sealed class AccountService : IService<StandardModeAccount>
    {
        private readonly IAsyncRepository<GameAccountDTO> _gameRepository;

        public AccountService(IAsyncRepository<GameAccountDTO> gameRepsitory)
        {
            _gameRepository = gameRepsitory;
        }

        public Task AddEntityAsync(StandardModeAccount entity)
        {
            entity.Map(out var gameAccountDTO);
            return _gameRepository.AddAsync(gameAccountDTO);
        }

        public Task ClearDatabaseAsync() => _gameRepository.ClearAsync();

        public async IAsyncEnumerable<StandardModeAccount> GetAllEntitiesAsync()
        {
            await foreach(var item in _gameRepository.GetAllAsync())
            {
                item.Map(out var entity);
                yield return entity;
            }
        }

        public async Task<StandardModeAccount> GetEntityByUniqueIdentifierAsync(string id)
        {
            StandardModeAccount result = null;
            var entity = await _gameRepository.GetByUniqueIdentifierAsync(id);
            
            if (!string.IsNullOrWhiteSpace(entity.UserName))
            {
                entity.Map(out var tempResult);
                result = tempResult;
            }

            return result;
        }

        public Task LoadDataAsync() => _gameRepository.LoadAsync();

        public Task RemoveEntityAsync(StandardModeAccount entity)
        {
            entity.Map(out var gameAccountDTO);
            return _gameRepository.RemoveAsync(gameAccountDTO);
        }

        public Task UpdateEntityAsync(StandardModeAccount entity)
        {
            entity.Map(out var gameAccountDTO);

            return _gameRepository.UpdateAsync(gameAccountDTO);
        }
    }
}
