using Lab1.Database.Service;
using Lab1.GameAccounts;

namespace Lab1.Handlers;

internal abstract class HandlerBase
{
    protected IService<StandardModeAccount> AccountService;

    protected Action<string> write;
    public event Action<string> Write
    {
        add => write += value;
        remove => write -= value;
    }

    protected Func<string> read;
    public event Func<string> Read
    {
        add => read += value;
        remove => read -= value;
    }

    
    protected Action clearArea;
    public event Action ClearArea
    {
        add => clearArea += value;
        remove => clearArea -= value;
    }

    protected void TrowExceptionIfDelegatesAreNull()
    {
        ArgumentNullException.ThrowIfNull(write);
        ArgumentNullException.ThrowIfNull(read);
        ArgumentNullException.ThrowIfNull(clearArea);
    }

    protected string SubmitPlayerName(int index)
    {
        string playerName = string.Empty;
        do
        {
            write($"\nEnter name for {index} player: ");
            playerName = read();
            clearArea();

        } while (string.IsNullOrWhiteSpace(playerName));

        return playerName;
    }

    public abstract Task HandleAsync();

    protected HandlerBase()
    {
        AccountService = IocContainer.GetService<IService<StandardModeAccount>>();
    }
}
