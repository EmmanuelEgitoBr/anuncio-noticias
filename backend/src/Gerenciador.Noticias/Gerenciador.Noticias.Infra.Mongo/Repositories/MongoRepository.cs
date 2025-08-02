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

    public List<T> Get() => _repository.Find(active => true).ToList();
    public T GetById(string id) =>
        _repository.Find<T>(news => news.Id == id).FirstOrDefault();

    public T Create(T news)
    {
        _repository.InsertOne(news);
        return news;
    }

    public void Update(string id, T newsIn) => _repository.ReplaceOne(news => news.Id == id, newsIn);

    public void Remove(string id) => _repository.DeleteOne(news => news.Id == id);
}
