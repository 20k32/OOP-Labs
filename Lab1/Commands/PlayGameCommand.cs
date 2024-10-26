using Lab1.Commands.Core;
using Lab1.Handlers;

namespace Lab1.Commands;

internal sealed class PlayGameCommand : IAsyncCommand
{
    GameHandler handler;

    public PlayGameCommand()
    {
        handler = IocContainer.GetService<GameHandler>();
    }

    public Task ExecuteAsync() => handler.HandleAsync();

    public CommandInfo GetInfo()
    {
        var commandName = nameof(PlayGameCommand);
        var commandDescription = "This is command for simmulating game between two players.";

        var info = new CommandInfo(commandName, commandDescription);

        return info;
    }
}
