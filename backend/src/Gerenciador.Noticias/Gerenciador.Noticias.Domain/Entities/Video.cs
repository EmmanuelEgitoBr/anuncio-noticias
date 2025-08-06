using Gerenciador.Noticias.Domain.Entities.Base;
using Gerenciador.Noticias.Domain.Enums;
using Gerenciador.Noticias.Domain.Helpers;
using Gerenciador.Noticias.Domain.Validators;
using MongoDB.Bson.Serialization.Attributes;

namespace Gerenciador.Noticias.Domain.Entities;

public class Video : BaseEntity
{
    [BsonElement("hat")]
    public string Hat { get; set; }
    [BsonElement("title")]
    public string Title { get; set; }
    [BsonElement("author")]
    public string Author { get; set; } = string.Empty;
    [BsonElement("thumbnail")]
    public string Thumbnail { get; set; } = string.Empty;

    public Video(string hat, string title, string author, string thumbnail, Status status)
    {
        Hat = hat;
        Title = title;
        Author = author;
        Slug = SlugHelper.GenerateSlug(Title);
        Status = status;

        ValidateEntity();
    }

    public void ValidateEntity()
    {
        AssertionConcern.AssertArgumentNotEmpty(Hat, "Chapéu não pode ser vazio");
        AssertionConcern.AssertArgumentNotEmpty(Title, "Título não pode ser vazio");

        AssertionConcern.AssertArgumentLength(Title, 90, "O título não pode ultrapassar 40 caracteres");
        AssertionConcern.AssertArgumentLength(Hat, 40, "O chapéu não pode ultrapassar 40 caracteres");
    }
}
