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
        public static void RegisterHandlers(Action<string> write, Func<string> read, Action clearBuffer)
        {
            var creationAccountHandler = new CreationAccountHandler();
            creationAccountHandler.Write += write;
            creationAccountHandler.Read += read;
            creationAccountHandler.ClearArea += clearBuffer;

            var gameHandler = new GameHandler();
            gameHandler.Write += write;
            gameHandler.Read += read;
            gameHandler.ClearArea += clearBuffer;

            var accountHandler = new AccountHandler();
            accountHandler.Write += write;
            accountHandler.Read += read;
            accountHandler.ClearArea += clearBuffer;

            var statsHandler = new StatsHandler();
            statsHandler.Write += write;
            statsHandler.Read += read;
            statsHandler.ClearArea += clearBuffer;

            IocContainer.AddService(creationAccountHandler);
            IocContainer.AddService(gameHandler);
            IocContainer.AddService(accountHandler);
            IocContainer.AddService(statsHandler);
        }

        public static void UnregisterHandlers(Action<string> write, Func<string> read, Action clearBuffer)
        {
            var creationAccountService = IocContainer.GetService<CreationAccountHandler>();
            creationAccountService.Write -= write;
            creationAccountService.Read -= read;
            creationAccountService.ClearArea -= clearBuffer;

            var gameHandler = IocContainer.GetService<GameHandler>();
            gameHandler.Write -= write;
            gameHandler.Read -= read;
            gameHandler.ClearArea -= clearBuffer;

            var accountHandler = IocContainer.GetService<AccountHandler>();
            accountHandler.Write -= write;
            accountHandler.Read -= read;
            accountHandler.ClearArea -= clearBuffer;

            var statsHandler = IocContainer.GetService<StatsHandler>();
            statsHandler.Write -= write;
            statsHandler.Read -= read;
            statsHandler.ClearArea -= clearBuffer;
        }
    }
}
