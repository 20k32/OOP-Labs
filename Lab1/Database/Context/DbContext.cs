using Lab1.Database.Context;
using Lab1.Database.DTOs;

namespace Lab1.Database;

internal sealed class DbContext : IAsyncDbContext
{
    private readonly GameAccountDbSet _accountsDbSet;
    public IAsyncDbSet<GameAccountDTO> AccountsDbSet { get => _accountsDbSet; }

    private readonly GameDbSet _gameDbSet;
    public IAsyncDbSet<GameDTO> GamesDbSet { get => _gameDbSet; }

    public DbContext()
    {
        _accountsDbSet = new();
        _gameDbSet = new();
    }
}
