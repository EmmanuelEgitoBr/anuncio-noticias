using Gerenciador.Noticias.Domain.Entities.Base;
using Gerenciador.Noticias.Domain.Enums;
using Gerenciador.Noticias.Domain.Helpers;
using Gerenciador.Noticias.Domain.Validators;
using MongoDB.Bson.Serialization.Attributes;

namespace Gerenciador.Noticias.Domain.Entities;

public class News : BaseEntity
{
    [BsonElement("title")]
    public string Title { get; set; }
    
    [BsonElement("summary")]
    public string Summary { get; set; }
    
    [BsonElement("text")]
    public string Text {  get; set; }
    
    [BsonElement("author")]
    public string Author {  get; set; } = string.Empty;
    
    [BsonElement("imageUrl")]
    public IEnumerable<string> MediaUrl {  get; set; }
    
    [BsonElement("link")]
    public string? Link {  get; set; }

    [BsonElement("slug")]
    public string Slug { get; set; } = string.Empty;

    [BsonElement("categoryId")]
    public int CategoryId { get; set; }

    [BsonElement("mediaType")]
    public MediaType Media { get; set; }

    [BsonElement("status")]
    public Status Status { get; set; }

    public News(string title, 
        string summary, 
        string text, 
        string author, 
        IEnumerable<string> url, 
        string link, 
        int categoryId, 
        MediaType mediaType, 
        Status status)
    {
        Summary = summary;
        Title = title;
        Text = text;
        Author = author;
        MediaUrl = url;
        Link = link;
        Slug = SlugHelper.GenerateSlug(Title);
        CreatedAt = DateTime.Now;
        CategoryId = categoryId;
        Media = mediaType;
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
        AssertionConcern.AssertArgumentNotEmpty(Summary, "O resumo não pode ser vazio");
        AssertionConcern.AssertArgumentNotEmpty(Title, "O título não pode ser vazio");
        AssertionConcern.AssertArgumentNotEmpty(Text, "O texto não pode ser vazio");

        AssertionConcern.AssertArgumentLength(Title, 90, "O título não pode ultrapassar 40 caracteres");
        AssertionConcern.AssertArgumentLength(Summary, 40, "O resumo não pode ultrapassar 40 caracteres");
    }
}
