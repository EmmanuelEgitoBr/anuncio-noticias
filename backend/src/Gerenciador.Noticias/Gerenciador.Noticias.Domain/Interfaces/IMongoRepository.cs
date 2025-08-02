namespace Gerenciador.Noticias.Domain.Interfaces;

public interface IMongoRepository<T> where T : class
{
    List<T> Get();
    T GetById(string id);
    T Create(T news);
    void Update(string id, T news);
    void Remove(string id);
}
