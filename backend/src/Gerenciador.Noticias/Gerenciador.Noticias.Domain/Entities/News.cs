using Gerenciador.Noticias.Domain.Entities.Base;
using Gerenciador.Noticias.Domain.Enums;
using Gerenciador.Noticias.Domain.Helpers;
using Gerenciador.Noticias.Domain.Validators;
using MongoDB.Bson.Serialization.Attributes;

namespace Gerenciador.Noticias.Domain.Entities;

public class News : BaseEntity
{
    [BsonElement("hat")]
    public string Hat { get; set; }
    [BsonElement("title")]
    public string Title { get; set; }
    [BsonElement("text")]
    public string Text {  get; set; }
    [BsonElement("author")]
    public string Author {  get; set; } = string.Empty;
    [BsonElement("image")]
    public string Image {  get; set; } = string.Empty;
    [BsonElement("link")]
    public string Link {  get; set; } = string.Empty;
    [BsonElement("publishDate")]
    public DateTime PublishDate { get; set; } = DateTime.Now;
    [BsonElement("status")]
    public Status Status { get; set; }

    public News(string hat, string title, string text, string author, string image, string link, Status status)
    {
        Hat = hat;
        Title = title;
        Text = text;
        Author = author;
        Image = image;
        Link = link;
        Slug = SlugHelper.GenerateSlug(Title);
        Status = status;

        ValidateEntity();
    }

    public Status ChangeStatus(Status status) =>
    status switch
    {
        Status.Active => Status.Active,
        Status.Inactive => Status.Inactive,
        Status.Draft => Status.Draft,
        _ => status // ou lançar exceção, se necessário
    };

    public void ValidateEntity()
    {
        AssertionConcern.AssertArgumentNotEmpty(Hat, "Chapéu não pode ser vazio");
        AssertionConcern.AssertArgumentNotEmpty(Title, "Título não pode ser vazio");
        AssertionConcern.AssertArgumentNotEmpty(Text, "Texto não pode ser vazio");

        AssertionConcern.AssertArgumentLength(Title, 90, "O título não pode ultrapassar 40 caracteres");
        AssertionConcern.AssertArgumentLength(Hat, 40, "O chapéu não pode ultrapassar 40 caracteres");
    }
}
