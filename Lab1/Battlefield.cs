using DoubleLinkedList;
using System;

namespace Lab1;

internal sealed class Battlefield
{
    public static void SimulateBattle(GameAccount firstPlayer, GameAccount secondPlayer, int times, Action<Game> callback = null)
    {
        if(times < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(times));
        }
        if(firstPlayer is null)
        {
            throw new ArgumentNullException(nameof(firstPlayer));
        }
        if(secondPlayer is null)
        {
            throw new ArgumentNullException(nameof(secondPlayer));
        }

        GameAccount current = null;
        GameAccount opponent = null;

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

            var rating = Random.Shared.Next(10, 101);
            var whoWin = Random.Shared.Next(0, 2);

            var game = new Game(current, opponent, rating);

            if (whoWin == 0)
            {
                var temp = current;
                current = opponent;
                opponent = temp;
            }

            current.OnWinGame(opponent.UserName, rating);
            opponent.OnLooseGame(current.UserName, rating);
            current.Log(new(opponent.UserName, rating, game.Index));
            opponent.Log(new(current.UserName, -rating, game.Index));

            callback?.Invoke(game);
        }
    }


    private static void SimulateBattleCore(List<GameAccount> accounts, int index, int times, Action<Game> callback)
    {
        var sencodPlayerIndex = 0;
        do
        {
            sencodPlayerIndex = Random.Shared.Next(0, accounts.Count);
        } while (sencodPlayerIndex == index);

        SimulateBattle(accounts[index], accounts[sencodPlayerIndex], times, callback);
    }


    public static void SimulateBattle(List<GameAccount> accounts, int times, Action<Game> callback = null)
    {
        for(int i = 0; i < accounts.Count; i++)
        {
            for(int k = 0; k < times; k++)
            {
                for (int j = 0; j < accounts.Count; j++)
                {
                    SimulateBattleCore(accounts, i, 1, callback);
                }
            }
            
        }
    }
}
