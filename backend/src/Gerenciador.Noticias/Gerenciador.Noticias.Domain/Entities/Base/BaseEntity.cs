using Gerenciador.Noticias.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gerenciador.Noticias.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public bool IsDeleted { get; set; }
        public string Slug { get; set; } = string.Empty;
        [BsonElement("publishDate")]
        public DateTime PublishDate { get; set; } = DateTime.Now;
        [BsonElement("status")]
        public Status Status { get; set; }
    }
}
