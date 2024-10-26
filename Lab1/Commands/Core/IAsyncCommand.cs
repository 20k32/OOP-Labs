using Lab1.Commands.Core;

namespace Lab1.Commands.Core;

internal interface IAsyncCommand
{
    Task ExecuteAsync();
    CommandInfo GetInfo();

    string ToInfoString() => GetInfo().ToString();
}
