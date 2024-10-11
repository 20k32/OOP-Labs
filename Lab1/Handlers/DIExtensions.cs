using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Handlers
{
    internal static class DIExtensions
    {
        public static void RegisterHandlers()
        {
            var creationAccountHandler = new CreationAccountHandler();
            var gameHandler = new GameHandler();
            var accountHandler = new AccountHandler();
            var statsHandler = new StatsHandler();

            IocContainer.AddService(creationAccountHandler);
            IocContainer.AddService(gameHandler);
            IocContainer.AddService(accountHandler);
            IocContainer.AddService(statsHandler);
        }
    }
}
