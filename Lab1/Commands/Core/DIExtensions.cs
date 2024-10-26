namespace Lab1.Commands.Core;

internal static class DIExtensions
{
    public static void RegisterCommands()
    {
        var addPlayer = new AddPlayerCommand();
        var getPlayers = new GetPlayersCommand();
        var getPlayerStats = new GetPlayerStatsCommand();
        var playGame = new PlayGameCommand();

        IocContainer.AddService(addPlayer);
        IocContainer.AddService(getPlayers);
        IocContainer.AddService(getPlayerStats);
        IocContainer.AddService(playGame);
    }
}
