﻿using DoubleLinkedList;
using Lab1.GameAccounts;
using Lab1.Games;
using Lab1.Games.GameFactory;
using System;
using System.Net.Http.Headers;

namespace Lab1;

internal sealed class Battlefield
{
    private static readonly GamePool gamePool;

    static Battlefield()
    {
        gamePool = new GamePool();
    }

    public static void SimulateBattle(StandardModeAccount firstPlayer, StandardModeAccount secondPlayer, int times, Action<Game> callback = null)
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
                case 0: game = gamePool.CreateStandradGame(current, opponent); break;
                case 1: game = gamePool.CreateRatingToWinnerGame(current, opponent); break;
                default: game = gamePool.CreateTrainingGame(current, opponent); break;
            }

            var whoWin = Random.Shared.Next(0, 2);

            var winRating = game.GetWinRating();
            var looseRating = game.GetLooseRating();

            if (whoWin == 0)
            {
                game.FirstPlayerWin();
                current.Log(new(opponent.UserName, winRating, game.Index, game.DisplayType));
                opponent.Log(new(current.UserName, -looseRating, game.Index, game.DisplayType));
            }
            else
            {
                game.FirstPlayerLoose();
                current.Log(new(opponent.UserName, -looseRating, game.Index, game.DisplayType));
                opponent.Log(new(current.UserName, winRating, game.Index, game.DisplayType));
            }

            callback?.Invoke(game);
        }
    }


    private static void SimulateBattleCore(List<StandardModeAccount> accounts, int index, int times, Action<Game> callback)
    {
        var sencodPlayerIndex = 0;
        do
        {
            sencodPlayerIndex = Random.Shared.Next(0, accounts.Count);
        } while (sencodPlayerIndex == index);

        SimulateBattle(accounts[index], accounts[sencodPlayerIndex], times, callback);
    }


    public static void SimulateBattle(List<StandardModeAccount> accounts, int times, Action<Game> callback = null)
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
