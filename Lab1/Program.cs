using Lab1.Commands;
using Lab1.Commands.Core;
using Lab1.Database.Service;
using Lab1.GameAccounts;
using Lab1.Games;

namespace Lab1;

internal sealed class Program
{
    private static readonly IAsyncCommand addAccountCommand;
    private static readonly IAsyncCommand playGameCommand;
    private static readonly IAsyncCommand getAccountsCommand;
    private static readonly IAsyncCommand getStatsCommand;

    private static IService<StandardModeAccount> accountService;
    private static IService<Game> gameService;

    static Program()
    {
        Database.DIExtensions.ConfigurePersistenceLayer();
        Handlers.DIExtensions.RegisterHandlers(Console.Write, Console.ReadLine, Console.Clear);
        Commands.Core.DIExtensions.RegisterCommands();

        accountService = IocContainer.GetService<IService<StandardModeAccount>>();
        gameService = IocContainer.GetService<IService<Game>>();

        addAccountCommand = IocContainer.GetService<AddPlayerCommand>();
        playGameCommand = IocContainer.GetService<PlayGameCommand>();
        getAccountsCommand = IocContainer.GetService<GetPlayersCommand>();
        getStatsCommand = IocContainer.GetService<GetPlayerStatsCommand>();
    }

    private static bool TryAddAccount(StandardModeAccount account)
    {
        bool result = false;

        try
        {
            Accounts.Add(account);
            result = true;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"{ex.Message}");
        }

        return result;
    }

    private static async Task<StandardModeAccount> SearchPlayerAsync(IService<StandardModeAccount> service)
    {
        Console.WriteLine("Enter player name -> ");
        var playerName = Console.ReadLine();
        var existingPlayer = await service.GetEntityByUniqueIdentifierAsync(playerName);
        return existingPlayer;
    }

    private static async Task<Game> SearchGameAsync(IService<Game> service)
    {
        Console.WriteLine("Enter game id -> ");
        var gameName = Console.ReadLine();
        var existingGame = await service.GetEntityByUniqueIdentifierAsync(gameName);
        return existingGame;
    }


    private static async Task Main(string[] args)
    {
        var gameLoadingTask = gameService.LoadDataAsync();

        bool spin = true;

        while (spin)
        {
            Console.Clear();
            Console.Write("q - Create characters." +
                $"\nw - {getAccountsCommand.GetInfo().CommandName}" +
                "\ne - Get all games" +
                $"\nr - {getStatsCommand.GetInfo().CommandName}" +
                "\nt - Search for a game" +
                "\na - Update player" +
                "\ns - Update game" +
                "\nd - Delete player" +
                "\nf - Delete game" +
                $"\nz - {addAccountCommand.GetInfo().CommandName}" +
                "\nx - Simulate bloody massacare" +
                $"\nc - {playGameCommand.GetInfo().CommandName}" +
                "\nv - Exit" +
                "\nAction -> ");

            var action = Console.ReadLine();

            Console.Clear();

            switch (action)
            {
                case "q":
                    {
                        Console.WriteLine("Data is loading, please wait.");
                        await gameLoadingTask;
                        await accountService.LoadDataAsync();
                        Console.WriteLine("Data loaded.\nPress any key to exit.");
                    }; break;

                case "w":
                    {
                        Console.WriteLine(getAccountsCommand.ToInfoString());
                        await getAccountsCommand.ExecuteAsync();
                    }; break;
                case "e":
                    {
                        Console.WriteLine("All games in db:\n");

                        await foreach (var game in gameService.GetAllEntitiesAsync())
                        {
                            Console.WriteLine(game.ToShortString());
                        }

                        Console.WriteLine("\n(end)");
                    }; break;
                case "r":
                    {
                        Console.WriteLine(getStatsCommand.ToInfoString());
                        await getStatsCommand.ExecuteAsync();
                    }; break;
                case "t":
                    {
                        var existingGame = await SearchGameAsync(gameService);
                        if (existingGame is null)
                        {
                            Console.WriteLine("There is no such game in database.");
                        }
                        else
                        {
                            Console.WriteLine($"Result: {existingGame.ToShortString()}");
                        }
                    }; break;

                case "a":
                    {
                        var existingPlayer = await SearchPlayerAsync(accountService);
                        if (existingPlayer is null)
                        {
                            Console.WriteLine("There is no such player in database.");
                        }
                        else
                        {
                            Console.WriteLine("There is such player.");
                            Console.WriteLine("Enter new rating for player: ");
                            var ratingStr = Console.ReadLine();

                            if (int.TryParse(ratingStr, out var rating) && rating > 1)
                            {
                                existingPlayer.SetRating(rating);
                                await accountService.UpdateEntityAsync(existingPlayer);
                            }
                            else
                            {
                                Console.WriteLine("Unable to parse rating.");
                            }
                        }

                    }; break;
                case "s":
                    {
                        var existingGame = await SearchGameAsync(gameService);
                        if (existingGame is null)
                        {
                            Console.WriteLine("There is no such game in database.");
                        }
                        else
                        {
                            Console.WriteLine("There is such game.");
                            Console.WriteLine("Enter new rating: ");
                            var ratingStr = Console.ReadLine();
                            if (int.TryParse(ratingStr, out var rating) && rating > 1)
                            {
                                existingGame.SetRating(rating);
                                await gameService.UpdateEntityAsync(existingGame);
                            }
                            else
                            {
                                Console.WriteLine("Unable to parse rating.");
                            }
                        }

                    }; break;
                case "d":
                    {
                        var existingPlayer = await SearchPlayerAsync(accountService);
                        if (existingPlayer is null)
                        {
                            Console.WriteLine("There is no such player in database.");
                        }
                        else
                        {
                            Console.WriteLine("There is such player.");
                            await accountService.RemoveEntityAsync(existingPlayer);
                            Console.WriteLine("The player is removed from database.");
                        }

                    }; break;
                case "f":
                    {
                        var existingGame = await SearchGameAsync(gameService);
                        if (existingGame is null)
                        {
                            Console.WriteLine("There is no such game in database.");
                        }
                        else
                        {
                            Console.WriteLine("There is such game.");
                            await gameService.RemoveEntityAsync(existingGame);
                            Console.WriteLine("The game is removed from database.");
                        }

                    }; break;
                case "z":
                    {
                        Console.WriteLine(addAccountCommand.ToInfoString());
                        await addAccountCommand.ExecuteAsync();
                    }; break;

                case "x":
                    {
                        using (var cts = new CancellationTokenSource())
                        {
                            var task = Battlefield.SimulateBattleAsync(1, OnGameCompletedAsync, cts.Token);

                            _ = task.ContinueWith(t =>
                            {
                                if (t.IsCompleted)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Bloody massaccre sucessfully completed.");
                                }
                            }, TaskContinuationOptions.OnlyOnRanToCompletion);

                            string result = string.Empty;

                            while (!task.IsCompleted)
                            {
                                if (!cts.IsCancellationRequested)
                                {
                                    Console.WriteLine("You stand right in the middle of bloody massacre and hear nothing but pleas for mercy and gunfire.\nStop the torment and go to eternal rest by typing 'stop'.");
                                    result = Console.ReadLine();

                                    if (result == "stop")
                                    {
                                        cts.Cancel();
                                        Console.WriteLine("Echo of war and the smell of caked blood makes you wake up.");
                                    }
                                    else
                                    {
                                        Console.Clear();
                                    }
                                }
                                else
                                {
                                    Console.Write("*");
                                    await Task.Delay(5);
                                }
                            }

                            if (task.IsCompleted && !cts.IsCancellationRequested)
                            {
                                Console.WriteLine("You've survived.");
                            }
                        }
                    }; break;

                case "c":
                    {
                        Console.WriteLine(playGameCommand.ToInfoString());
                        await playGameCommand.ExecuteAsync();
                    }; break;

                case "v": spin = false; break;
            }

            Console.ReadLine();
        }

        Lab1.Handlers.DIExtensions.UnregisterHandlers(Console.Write, Console.ReadLine, Console.Clear);
    }

    private static async Task OnGameCompletedAsync(Game game)
    {
        await gameService.AddEntityAsync(game);
    }
}
