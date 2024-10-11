using Lab1.Database.Service;
using Lab1.GameAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Handlers
{
    internal class AccountHandler : HandlerBase
    {
        public AccountHandler() : base()
        { }

        public override async Task HandleAsync()
        {
            await foreach(var item in AccountService.GetAllEntitiesAsync())
            {
                write(item.ToString() + "\n");
            }
        }
    }
}
