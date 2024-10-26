using Lab1.Commands.Core;
using Lab1.Handlers;

namespace Lab1.Commands;

internal sealed class GetPlayerStatsCommand : IAsyncCommand
{
    StatsHandler handler;

    public GetPlayerStatsCommand()
    {
        handler = IocContainer.GetService<StatsHandler>();
    }

    public Task ExecuteAsync() => handler.HandleAsync();

    public CommandInfo GetInfo()
    {
        var commandName = nameof(GetPlayerStatsCommand);
        var commandDescription = "This is command for displaying detailed player's info.";

        var info = new CommandInfo(commandName, commandDescription);
    
        return info;
    }
}
