using Lab1.GameAccounts;
using Lab1.Games;

namespace Lab1;

internal sealed class Program
{
    private static string SubmitPlayerName(int playerId)
    {
        string playerName = string.Empty;
        do
        {
            Console.Clear();
            Console.Write($"\nEnter name for {playerId} player: ");
            playerName = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(playerName));

        return playerName;
    }

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


    private static void Main(string[] args)
    {
        bool spin = true;
        int action;
        StandardModeAccount firstPlayer = null;
        StandardModeAccount secondPlayer = null;

        while (spin)
        {
            do
            {
                Console.Clear();
                Console.Write("1 - Give name for two characters." +
                    "\n2 - Simulate fight." +
                    "\n3 - Simulate bloody massacre" +
                    "\n4 - Show stats for character." +
                    "\n5 - Exit" +
                    "\nAction -> ");
                
            } while (!int.TryParse(Console.ReadLine(), out action));
            Console.Clear();
            switch (action)
            {
                case 1:
                    {
                        var firstPlayerName = SubmitPlayerName(1);
                        var secondPlayerName = SubmitPlayerName(2);
                        firstPlayer = GenerateAccount(firstPlayerName);
                        secondPlayer = GenerateAccount(secondPlayerName);

                        if (!TryAddAccount(firstPlayer) 
                            || !TryAddAccount(secondPlayer))
                        {
                            firstPlayer = null;
                            secondPlayer = null;
                            Console.WriteLine("Database will be cleared");
                            Console.ReadKey();
                            Accounts.Clear();
                        }
                        else
                        {
                            Console.WriteLine("Accounts generated successfully.");
                            Console.WriteLine($"First account type: {firstPlayer.DisplayType}");
                            Console.WriteLine($"Second account type: {secondPlayer.DisplayType}");

                            Console.ReadKey();
                        }
                    }; break;

                case 2:
                    {
                        if (firstPlayer is null
                            || secondPlayer is null)
                        {
                            Console.WriteLine("First of all, create players.");
                        }
                        else
                        {
                            Battlefield.SimulateBattle(firstPlayer, secondPlayer, 10, OnGameCompleted);
                            Console.WriteLine("Fight ended.");
                        }
                        Console.ReadKey();
                    }; break;
                case 3:
                    {
                        Accounts.Clear();

                        var players = Enumerable
                                        .Range('a', 'z' - 'a' + 1)
                                        .Select(x =>
                                        {
                                            var account = GenerateAccount(((char)x).ToString());
                                            Accounts.Add(account);
                                            return account;
                                        })
                                        .ToList();

                        Console.Clear();
                        Console.WriteLine("You stand right in the middle of bloody massacre and hear nothing but pleas for mercy and gunfire.");
                        Battlefield.SimulateBattle(players, 5);
                        Console.Clear();
                        Console.WriteLine("Echo of war and the smell of caked blood makes you wake up.");
                        Console.ReadKey();
                    }; break;
                case 4:
                    {
                        Console.Clear();
                        try
                        {
                            var name = Console.ReadLine();
                            Console.Clear();
                            var account = Accounts.GetByName(name);
                            Console.WriteLine($"\nType: {account.DisplayType}\n");
                            Console.Write($"\n{account.UserName}\nGames count: {account.GamesCount}\nRating: {account.CurrentRating}\n\nOppName\tStatus\tRating\tIndex\n\n");
                            foreach (var item in account.GetHistory())
                            {
                                Console.WriteLine(item);
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            Console.WriteLine("There is no such user in database.");
                        }

                        Console.ReadKey();
                    }; break;
                default: spin = false; break;
            }
        }
    }

    private static void OnGameCompleted(Game game)
    {
        Console.WriteLine($"* Game [{game.Index}]\n" +
            $"between [{game.FirstPlayer.UserName}] and " +
            $"[{game.SecondPlayer.UserName}]\nwith type [{game.DisplayType}]\non rating [w: {game.GetWinRating()}, l:{game.GetLooseRating()}] comleted.\n");
    }
}
