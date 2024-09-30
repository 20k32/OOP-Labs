namespace Lab1.Database.Repository;

internal interface IAsyncRepository<T>
{
    IAsyncEnumerable<T> GetAllAsync();
    Task LoadAsync();
    Task ClearAsync();
    Task RemoveAsync(T entity);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<T> GetByUniqueIdentifierAsync(string id);
}