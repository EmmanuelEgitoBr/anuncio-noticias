using Gerenciador.Noticias.Domain.Entities.Base;
using Gerenciador.Noticias.Domain.Interfaces;
using Gerenciador.Noticias.Infra.Mongo.Settings;
using MongoDB.Driver;

namespace Gerenciador.Noticias.Infra.Mongo.Repositories;

public class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _repository;

    public MongoRepository(IDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);
        _repository = database.GetCollection<T>(typeof(T).Name.ToLower());
    }

    public async Task<List<T>> GetAsync()
    {
        var result = await _repository.FindAsync(_ => true);
        return await result.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        var result = await _repository.FindAsync(x => x.Id == id);
        return await result.FirstOrDefaultAsync();
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _repository.InsertOneAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(string id, T entity)
    {
        await _repository.ReplaceOneAsync(x => x.Id == id, entity);
    }

    public async Task RemoveAsync(string id)
    {
        await _repository.DeleteOneAsync(x => x.Id == id);
    }
}

