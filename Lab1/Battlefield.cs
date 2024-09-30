using DoubleLinkedList;
using Lab1.Database.Service;
using Lab1.GameAccounts;
using Lab1.Games;
using Lab1.Games.GameFactory;
using System;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace Lab1;

internal sealed class Battlefield
{
    private static readonly IService<StandardModeAccount> accountService;
    private static readonly IService<Game> gameService;

    private static readonly IGameFactory gameFactory;

    static Battlefield()
    {
        accountService = IocContainer.GetService<IService<StandardModeAccount>>();
        gameService = IocContainer.GetService<IService<Game>>();

        gameFactory = GameFactory.Instance;
    }

    public static async Task SimulateBattleAsync(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer, int times, Func<Game, Task> callback)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(times, 1);
        ArgumentNullException.ThrowIfNull(firstPlayer);
        ArgumentNullException.ThrowIfNull(secondPlayer);

        StandardModeAccount current = null;
        StandardModeAccount opponent = null;

        foreach (var item in Enumerable.Range(0, times))
        {
            var chooseSides = Random.Shared.Next(0, 2);
            
            if (chooseSides == 0)
            {
                opponent = firstPlayer;
                current = secondPlayer;
            }
            else
            {
                current = firstPlayer;
                opponent = secondPlayer;
            }

            Game game;
            var gameType = Random.Shared.Next(0, 3);

            switch (gameType)
            {
                case 0: game = gameFactory.CreateStandradGame(); break;
                case 1: game = gameFactory.CreateRatingToWinnerGame(); break;
                default: game = gameFactory.CreateTrainingGame(); break;
            }
            game.SetPlayers(current, opponent);

            var whoWin = Random.Shared.Next(0, 2);

            if (whoWin == 0)
            {
                await game.FirstPlayerWinAsync(accountService);
            }
            else
            {
                await game.FirstPlayerLooseAsync(accountService);
            }

            await callback(game);
        }
    }


    private static async Task SimulateBattleCoreAsync(List<StandardModeAccount> accounts, int index, int times, Func<Game, Task> callback)
    {
        var sencodPlayerIndex = 0;
        do
        {
            sencodPlayerIndex = Random.Shared.Next(0, accounts.Count);
        } while (sencodPlayerIndex == index);

        await SimulateBattleAsync(accounts[index], accounts[sencodPlayerIndex], times, callback);
    }


    public static async Task SimulateBattleAsync(int times, Func<Game, Task> callback, CancellationToken ct)
    {
        var accounts = new List<StandardModeAccount>();

        await foreach (var item in accountService.GetAllEntitiesAsync())
        {
            accounts.Add(item);
        }

        for(int i = 0; i < accounts.Count; i++)
        {
            for(int k = 0; k < times; k++)
            {
                for (int j = 0; j < accounts.Count; j++)
                {
                    ct.ThrowIfCancellationRequested();

                    await SimulateBattleCoreAsync(accounts, i, 1, callback);
                }
            }
        }
    }
}
