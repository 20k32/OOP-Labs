namespace Lab1.Database.Service;

internal interface IService<T>
{
    IAsyncEnumerable<T> GetAllEntitiesAsync();
    Task LoadDataAsync();
    Task<T> GetEntityByUniqueIdentifierAsync(string id);
    Task AddEntityAsync(T entity);
    Task UpdateEntityAsync(T entity);
    Task RemoveEntityAsync(T entity);
    Task ClearDatabaseAsync();
}

