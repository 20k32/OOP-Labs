using Lab1.GameAccounts;
using Lab1.Games;
using Lab1.Games.GameFactory;
using Lab1.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Handlers;

internal class GameHandler : HandlerBase
{
    private StringBuilder builder;

    public GameHandler()
    {
        builder = new();
    }

    private StandardModeAccount ChooseAccoutType(string type, string userName) => type switch
    {
        "1" => new StandardModeAccount(userName),
        "2" => new HardModeAccount(userName, GameRules.HARD_MODE_ACCOUNT_WIN_STREAK),
        "3" => new ArcadeModeAccount(userName)
    };

    private Type ChooseGameType(int typeIndex)
    {
        var type = Resolver.GameTypes
            .Values
            .First(game => game.AssociatedIndex == typeIndex)
            .Type;

        return type;
    }

    private StandardModeAccount SubmitPlayerType(string userName)
    {
        builder.Clear();

        builder.AppendLine("\nEnter account type:");
        int counter = 1;

        foreach (var typeName in Resolver.AccountTypes.Values.Select(x => x.BaseName))
        {
            builder.AppendLine($"{counter++} - {typeName}");
        }

        string answer = string.Empty;
        do
        {
            write(builder.ToString());
            write("Action -> ");
            answer = read();

        } while (!int.TryParse(answer, out var intAsnwer) || intAsnwer < 1 || intAsnwer > counter);

        var choosenAccount = ChooseAccoutType(answer, userName);

        return choosenAccount;
    }

    private void OnGameCompleted(Game game)
    {
        write(game.ToString() + "\n");
    }

    private Type SubmitGameType()
    {
        builder.Clear();

        builder.Append("Enter game type:\n");
        int counter = 1;

        foreach (var typeName in Resolver.GameTypes.Values.Select(x => x.BaseName))
        {
            builder.AppendLine($"{counter++} - {typeName}");
        }

        string answer = string.Empty;
        int intAnswer = -1;
        do
        {
            clearArea();
            write(builder.ToString());
            write("Action -> ");
            answer = read();

        } while (!int.TryParse(answer, out intAnswer) || intAnswer < 1 || intAnswer > counter);

        var choosenGame = ChooseGameType(intAnswer);

        return choosenGame;
    }

    private int SubmitTimesToPlay()
    {
        string answer = string.Empty;
        int intAnswer;
        do
        {
            clearArea();
            write("\nHow many times do you wish to play?\n");
            write("Action -> ");
            answer = read();

        } while (!int.TryParse(answer, out intAnswer) || intAnswer < 1);

        return intAnswer;
    }


    public override async Task HandleAsync()
    {
        var firstPlayerName = SubmitPlayerName(1);
        var firstPlayer = SubmitPlayerType(firstPlayerName);

        var secondPalyerName = SubmitPlayerName(2);
        var secondPlayer = SubmitPlayerType(secondPalyerName);

        write("\nPlayers created, adding them to db...");
        try
        {
            await AccountService.AddEntityAsync(firstPlayer);
            await AccountService.AddEntityAsync(secondPlayer);
            write("\nPlayers added.");
        }
        catch
        {
            write("\nError on server during adding.");
        }

        read();
        var gameType = SubmitGameType();
        var timeToPlay = SubmitTimesToPlay();

        await Battlefield.SimulateSpecificBattleAsync(firstPlayer, secondPlayer, gameType, timeToPlay, OnGameCompleted);
    }
}
