using Lab1.Database.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Database.Context;

internal interface IAsyncDbSet<T>
{
    IAsyncEnumerable<T> GetAllAsync();
    Task AddOneAsync(T entity);
    Task UpdateOneAsync(T entity);
    Task RemoveOneAsync(T entity);
    Task<T> GetOneAsync(T entity);
    Task ClearAsync();
    Task InitAsync();
}
