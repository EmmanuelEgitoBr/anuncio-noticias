using Gerenciador.Noticias.Domain.Entities.Base;
using Gerenciador.Noticias.Domain.Interfaces;
using Gerenciador.Noticias.Infra.Mongo.Settings;
using MongoDB.Driver;

namespace Gerenciador.Noticias.Infra.Mongo.Repositories;

public class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _repository;

    public MongoRepository(IMongoDatabaseSettings settings)
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

    public async Task<(List<T> Items, long Total)> GetPagedAsync(
    FilterDefinition<T> filter,
    SortDefinition<T> sort,
    int page,
    int pageSize)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize > 100 ? 100 : pageSize;

        var total = await _repository.CountDocumentsAsync(filter);

        var items = await _repository.Find(filter)
            .Sort(sort)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<long> CountAsync(FilterDefinition<T> filter)
    {
        return await _repository.CountDocumentsAsync(filter);
    }


    public async Task<T?> GetByIdAsync(string id)
    {
        var result = await _repository.FindAsync(x => x.Id == id);
        return await result.FirstOrDefaultAsync();
    }

    public async Task<T?> GetBySlugAsync(string slug)
    {
        var result = await _repository.FindAsync(x => x.Slug == slug);
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

