using MongoDB.Driver;

namespace Gerenciador.Noticias.Domain.Interfaces;

public interface IMongoRepository<T> where T : class
{
    Task<List<T>> GetAsync();
    Task<(List<T> Items, long Total)> GetPagedAsync(FilterDefinition<T> filter, SortDefinition<T> sort, int page, int pageSize);
    Task<long> CountAsync(FilterDefinition<T> filter);
    Task<T?> GetByIdAsync(string id);
    Task<T?> GetBySlugAsync(string slug);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task RemoveAsync(string id);
}
