using Lab1.Database.DTOs;
using Lab1.GameAccounts;
using Lab1.Games;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Shared;

internal static class Resolver
{
    internal static class GameTypes
    {
        public static readonly HashSet<TypeUnit> Values = new HashSet<TypeUnit>();

        public static readonly TypeUnit StandardGame;
        public static readonly TypeUnit RatingToWinnerGame;
        public static readonly TypeUnit TrainingGame;

        static GameTypes()
        {
            StandardGame = new TypeUnit(0, "Standard", typeof(StandardGame));
            RatingToWinnerGame = new TypeUnit(1, "Rating to winner", typeof(RatingToWinnerGame));
            TrainingGame = new TypeUnit(2, "Training", typeof(TrainingGame));

            Values = new();
            Values.Add(StandardGame);
            Values.Add(RatingToWinnerGame);
            Values.Add(TrainingGame);
        }
    }

    internal static class AccountTypes
    {
        public static readonly TypeUnit StandardModeAccount;
        public static readonly TypeUnit HardModeAccount;
        public static readonly TypeUnit ArcadeModeAccount;

        public static readonly HashSet<TypeUnit> Values = new HashSet<TypeUnit>();


        static AccountTypes()
        {
            StandardModeAccount = new TypeUnit(0, "Standard", typeof(StandardModeAccount));
            HardModeAccount = new TypeUnit(1, "Hard", typeof(HardModeAccount)); ;
            ArcadeModeAccount = new TypeUnit(2, "Arcade", typeof(ArcadeModeAccount));

            Values = new();
            Values.Add(StandardModeAccount);
            Values.Add(HardModeAccount);
            Values.Add(ArcadeModeAccount);
        }
    }

    public static Type ResolveGameType(GameDTO instance) 
        => GameTypes.Values.FirstOrDefault(game => game.BaseName == instance.DisplayType).Type;

    public static TypeUnit ResolveGameType(Game instance)
    {
        var instanceType = instance.GetType();
        TypeUnit result = GameTypes.Values.First(game => game.Type == instanceType);

        return result;
    }

    public static Type ResolveAccountType(GameAccountDTO instance) 
        => AccountTypes.Values.FirstOrDefault(account => account.BaseName == instance.AccountType).Type;

    public static TypeUnit ResolveAccountType(StandardModeAccount instance)
    {
        var instanceType = instance.GetType();

        var resultType = AccountTypes.Values.FirstOrDefault(account => account.Type == instanceType);

        return resultType;
    }


    public static Type TryResolveType<T>(T instance)
    {
        Type resolvedType = null;

        if (instance is GameDTO gameDTO)
        {
            resolvedType = ResolveGameType(gameDTO);
        }
        else if (instance is GameAccountDTO accountDTO)
        {
            resolvedType = ResolveAccountType(accountDTO);
        }
        else if (instance is Game game)
        {
            resolvedType = typeof(GameDTO);
        }
        else if(instance is StandardModeAccount account)
        {
            resolvedType = typeof(GameAccountDTO);
        }

        return resolvedType;
    }
}
