using Lab1.Database;
using Lab1.Database.DTOs;
using Lab1.Database.Service;
using Lab1.GameAccounts;
using Lab1.Games;
using Lab1.Games.Logging;
using Lab1.Mapper;
using Lab1.Shared;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Channels;
using System.Xml.Linq;

namespace Lab1;

class Example
{
    private readonly int _myReadonlyField = 42;

    public int GetReadonlyFieldValue() => _myReadonlyField;
}


internal sealed class Program
{
    private static IService<StandardModeAccount> accountService;
    private static IService<Game> gameService;

    static Program()
    {
        Database.DIExtensions.ConfigurePersistenceLayer();

        accountService = IocContainer.GetService<IService<StandardModeAccount>>();
        gameService = IocContainer.GetService<IService<Game>>();
    }

    private static string SubmitPlayerName(int playerIndex)
    {
        string playerName = string.Empty;
        do
        {
            Console.Clear();
            Console.Write($"\nEnter name for {playerIndex} player: ");
            playerName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(playerName));

        return playerName;
    }

    private static StandardModeAccount ChooseAccoutType(string type, string userName) => type switch
    {
        "1" => new StandardModeAccount(userName),
        "2" => new HardModeAccount(userName, GameRules.HARD_MODE_ACCOUNT_WIN_STREAK),
        _ => new ArcadeModeAccount(userName)
    };

    private static StandardModeAccount GenerateAccount(string userName)
    {
        StandardModeAccount result;
        var index = Random.Shared.Next(0, 3);

        switch(index)
        {
            case 0: result = new ArcadeModeAccount(userName);break;
            case 1: result = new HardModeAccount(userName, 5); break;
            default: result = new(userName); break;
        }    

        return result;
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
        var existingGame= await service.GetEntityByUniqueIdentifierAsync(gameName);
        return existingGame;
    }


    private static async Task Main(string[] args)
    {
        var gameLoadingTask = gameService.LoadDataAsync();
        //gameLoadingTask.Start();

         bool spin = true;

         while (spin)
         {
             Console.Clear();
             Console.Write("q - Create characters." +
                 "\nw - Get all players." +
                 "\ne - Get all games" +
                 "\nr - Search for a player." +
                 "\nt - Search for a game" +
                 "\na - Update player" +
                 "\ns - Update game" +
                 "\nd - Delete player" +
                 "\nf - Delete game" +
                 "\nz - Add player" +
                 "\nx - Simulate bloody massacare" +
                 "\nc - Exit" +
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
                         Console.WriteLine("All players in db:\n");

                         await foreach(var gameAccount in accountService.GetAllEntitiesAsync())
                         {
                             Console.WriteLine(gameAccount);
                         }
                         Console.WriteLine("\n(end)");
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
                         var existingPlayer = await SearchPlayerAsync(accountService);
                         if(existingPlayer is null)
                         {
                             Console.WriteLine("There is no such player in database.");
                         }
                         else
                         {
                             Console.WriteLine($"Result: {existingPlayer.ToString()}");
                             var history = existingPlayer.GetHistory();
                             if (history is not null)
                             {
                                 Console.WriteLine("History");
                                 foreach (var item in history)
                                 {
                                     Console.WriteLine(item);
                                 }
                                 Console.WriteLine("(end)");
                             }
                         }                            
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

                             if(int.TryParse(ratingStr, out var rating) && rating > 1)
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
                         var userName = SubmitPlayerName(1);
                         Console.WriteLine("Enter account type:\n1 - standard\n2 - hard\n3 - arcate\nAction -> ");
                         var accontType = Console.ReadLine();
                         var account = ChooseAccoutType(accontType, userName);
                         Console.WriteLine($"You've choosen {account.DisplayType}.");
                         await accountService.AddEntityAsync(account);
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

                             if(task.IsCompleted && !cts.IsCancellationRequested)
                             {
                                 Console.WriteLine("You've survived.");
                             }
                         }
                     }; break;

                 case "c" :spin = false; break;
             }
        Console.ReadLine();
        }
    }

    private static async Task OnGameCompletedAsync(Game game)
    {
        await gameService.AddEntityAsync(game);
    }
}
