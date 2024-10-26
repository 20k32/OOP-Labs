using Lab1.Commands.Core;
using Lab1.Handlers;

namespace Lab1.Commands;

internal sealed class AddPlayerCommand : IAsyncCommand
{
    CreationAccountHandler handler;

    public AddPlayerCommand()
    {
        handler = IocContainer.GetService<CreationAccountHandler>();
    }

    public Task ExecuteAsync() => handler.HandleAsync();

    public CommandInfo GetInfo()
    {
        var commandName = nameof(AddPlayerCommand);
        var commandDescription = "This is command for adding new player to db.";

        var info = new CommandInfo(commandName, commandDescription);

        return info;
    }
}
