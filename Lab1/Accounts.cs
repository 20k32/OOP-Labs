using DoubleLinkedList;
using Lab1.GameAccounts;

namespace Lab1;

internal sealed class Accounts
{
    private static readonly DoubleLinkedList<StandardModeAccount> _accounts;

    static Accounts()
    {
        _accounts = new();
    }

    public static StandardModeAccount GetByName(string name) => 
        _accounts
        .ReadFromHead()
        .First(gameAccount => gameAccount.UserName == name);

    public static void Clear() => _accounts.Clear();
    public static void Remove(StandardModeAccount account) => _accounts.RemoveFirstFromTail(account);
    public static void Add(StandardModeAccount account)
    {
        if(account is null)
        {
            throw new ArgumentNullException(nameof(account));
        }

        var existing = _accounts
            .ReadFromHead()
            .FirstOrDefault(existingAccount => existingAccount.Equals(account));

        if(existing is not null)
        {
            throw new InvalidOperationException($"There is such entry in database.");
        }

        _accounts.AddToEnd(account);
    }
}
