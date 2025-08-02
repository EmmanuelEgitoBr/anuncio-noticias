namespace Gerenciador.Noticias.Domain.Interfaces;

public interface IMongoRepository<T> where T : class
{
    Task<List<T>> GetAsync();
    Task<T?> GetByIdAsync(string id);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task RemoveAsync(string id);
}
