using Lab1.Commands.Core;
using Lab1.Handlers;

namespace Lab1.Commands;

internal sealed class GetPlayersCommand : IAsyncCommand
{
    AccountHandler handler;

    public GetPlayersCommand()
    {
        handler = IocContainer.GetService<AccountHandler>();
    }

    public Task ExecuteAsync() => handler.HandleAsync();

    public CommandInfo GetInfo()
    {
        var commandName = nameof(GetPlayersCommand);
        var commandDescription = "This is command for displaying players info.";

        var info = new CommandInfo(commandName, commandDescription);

        return info;
    }
}
