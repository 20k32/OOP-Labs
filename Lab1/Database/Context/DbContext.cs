using Lab1.Database.Context;
using Lab1.Database.DTOs;

namespace Lab1.Database;

internal sealed class DbContext : IAsyncDbContext
{
    private readonly AsyncDbSet<GameAccountDTO> _accountsDbSet;
    public IAsyncDbSet<GameAccountDTO> AccountsDbSet { get => _accountsDbSet; }

    private readonly AsyncDbSet<GameDTO> _gameDbSet;
    public IAsyncDbSet<GameDTO> GamesDbSet { get => _gameDbSet; }

    public DbContext()
    {
        _accountsDbSet = new GameAccountDbSet();
        _gameDbSet = new();
    }
}
