using DoubleLinkedList;

namespace Lab1;

internal sealed class Accounts
{
    private static DoubleLinkedList<GameAccount> _accounts;

    static Accounts()
    {
        _accounts = new();
    }

    public static GameAccount GetByName(string name) => 
        _accounts
        .ReadFromHead()
        .First(gameAccount => gameAccount.UserName == name);

    public static void Clear() => _accounts.Clear();
    public static void Remove(GameAccount account) => _accounts.RemoveFirstFromTail(account);
    public static void Add(GameAccount account)
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
