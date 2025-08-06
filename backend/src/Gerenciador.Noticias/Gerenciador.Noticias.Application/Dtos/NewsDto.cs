using Gerenciador.Noticias.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Gerenciador.Noticias.Application.Dtos;

public class NewsDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? Text { get; set; }
    public string Author { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? Link { get; set; }
    public string Slug { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; } = DateTime.Now;
    public Status Status { get; set; }
}
