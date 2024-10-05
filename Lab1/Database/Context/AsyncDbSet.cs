using Lab1.Database.DTOs;


namespace Lab1.Database.Context;

internal class AsyncDbSet<T> : IAsyncDbSet<T>
{
    protected LinkedList<T> Data;

    public AsyncDbSet()
    { }

    public virtual async Task InitAsync()
    {
        await Task.Delay(100);
        Data = new();
    }


    public async Task AddOneAsync(T entity)
    {
        if (Data is null)
        {
            return;
        }

        await Task.Delay(100);
        Data.AddLast(entity);
    }

    public async Task ClearAsync()
    {
        if (Data is null)
        {
            return;
        }

        await Task.Delay(100);
        Data.Clear();
    }

    public async IAsyncEnumerable<T> GetAllAsync()
    {
        if (Data is null)
        {
            yield break;
        }

        foreach (var item in Data)
        {
            await Task.Delay(100);
            yield return item;
        }
    }

    public async Task<T> GetOneAsync(T game)
    {
        if (Data is null)
        {
            return default;
        }

        T result = default;

        await Task.Delay(100);
        var node = Data.Find(game);

        if (node is not null)
        {
            result = node.Value;
        }

        return result;
    }

    public async Task RemoveOneAsync(T game)
    {
        if (Data is null)
        {
            return;
        }

        await Task.Delay(100);

        var node = Data.Find(game);

        if (node is not null)
        {
            Data.Remove(node);
        }
    }

    public async Task UpdateOneAsync(T game)
    {
        if (Data is null)
        {
            return;
        }

        await Task.Delay(100);

        var node = Data.Find(game);
        if (node is not null)
        {
            Data.AddBefore(node, game);
            Data.Remove(node);
        }
    }
}
