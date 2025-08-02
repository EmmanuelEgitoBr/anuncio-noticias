using Gerenciador.Noticias.Domain.Entities.Base;
using Gerenciador.Noticias.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Gerenciador.Noticias.Domain.Entities;

public class News : BaseEntity
{
    [BsonElement("hat")]
    public string Hat { get; set; } = string.Empty;
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;
    [BsonElement("text")]
    public string Text {  get; set; } = string.Empty;
    [BsonElement("author")]
    public string Author {  get; set; } = string.Empty;
    [BsonElement("image")]
    public string Image {  get; set; } = string.Empty;
    [BsonElement("link")]
    public string Link {  get; set; } = string.Empty;
    [BsonElement("publishDate")]
    public DateTime PublishDate { get; set; }
    [BsonElement("status")]
    public Status Status { get; set; }

    public News(string hat, string title, string text, string author, string image, string link, DateTime publishDate, Status status)
    {
        Hat = hat;
        Title = title;
        Text = text;
        Author = author;
        Image = image;
        Link = link;
        PublishDate = publishDate;
        Status = status;
    }

    public Status ChangeStatus(Status status) =>
    status switch
    {
        Status.Active => Status.Active,
        Status.Inactive => Status.Inactive,
        Status.Draft => Status.Draft,
        _ => status // ou lançar exceção, se necessário
    };
}
