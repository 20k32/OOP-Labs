using Lab1.Database.Service;
using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Handlers
{
    internal class StatsHandler : HandlerBase
    {
        private async Task<StandardModeAccount> SearchPlayerAsync()
        {
            write("\nEnter player name -> ");
            var playerName = read();
            var existingPlayer = await AccountService.GetEntityByUniqueIdentifierAsync(playerName);
            return existingPlayer;
        }


        public override async Task HandleAsync()
        {
            var existingPlayer = await SearchPlayerAsync();
            if (existingPlayer is null)
            {
                write("\nThere is no such player in database.");
            }
            else
            {
                write($"\nResult: {existingPlayer.ToString()}");
                var history = existingPlayer.GetHistory();

                if (history is not null)
                {
                    write("\nHistory");
                    foreach (var item in history)
                    {
                        write(item.ToString());
                    }
                    write("\n(end)");
                }
            }
        }
    }
}
