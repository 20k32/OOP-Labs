using Lab1.Database.Service;
using Lab1.GameAccounts;
using Lab1.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Handlers
{
    internal class CreationAccountHandler : HandlerBase
    {
        public CreationAccountHandler() : base()
        { }

        private StandardModeAccount ChooseAccoutType(string type, string userName) => type switch
        {
            "1" => new StandardModeAccount(userName),
            "2" => new HardModeAccount(userName, GameRules.HARD_MODE_ACCOUNT_WIN_STREAK),
             _ => new ArcadeModeAccount(userName)
        };


        public override async Task HandleAsync()
        {
            TrowExceptionIfDelegatesAreNull();

            var userName = SubmitPlayerName(1);
            write("Enter account type:\n1 - standard\n2 - hard\n3 - arcate\nAction -> ");
            var accontType = read();
            var account = ChooseAccoutType(accontType, userName);
            write($"You've choosen {account.DisplayType}.");

            await AccountService.AddEntityAsync(account);
        }
    }
}
